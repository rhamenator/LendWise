using LendWise.Web.Data;
using LendWise.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace LendWise.Web.Services;

public class PortfolioService(LendWiseDbContext db)
{
    public async Task<DashboardSnapshot> GetDashboardAsync()
    {
        var today = DateTime.UtcNow.Date;
        var loans = await db.Loans.Include(loan => loan.Customer).ToListAsync();
        var workItems = await db.WorkItems
            .Include(item => item.Customer)
            .Where(item => item.CompletedAt == null)
            .OrderBy(item => item.DueAt)
            .ThenByDescending(item => item.Priority)
            .Take(8)
            .ToListAsync();

        var stageCounts = loans
            .GroupBy(loan => loan.Stage)
            .Select(group => new StageCount(group.Key, group.Count(), group.Sum(loan => loan.Amount)))
            .OrderBy(stage => stage.Stage)
            .ToList();

        var recentActivity = await db.ActivityHistory
            .OrderByDescending(activity => activity.OccurredAt)
            .Take(6)
            .ToListAsync();

        return new DashboardSnapshot(
            CustomerCount: await db.Customers.CountAsync(),
            ActiveLoanCount: loans.Count(loan => loan.Stage is not LoanStage.Closed and not LoanStage.Dormant),
            OpenTaskCount: await db.WorkItems.CountAsync(item => item.CompletedAt == null),
            OverdueTaskCount: await db.WorkItems.CountAsync(item => item.CompletedAt == null && item.DueAt.Date < today),
            PipelineAmount: loans.Where(loan => loan.Stage is not LoanStage.Closed and not LoanStage.Dormant).Sum(loan => loan.Amount),
            StageCounts: stageCounts,
            DueWork: workItems,
            RecentActivity: recentActivity);
    }

    public async Task<List<CustomerListItem>> SearchCustomersAsync(string? query, string? heatLevel, string? status)
    {
        var customers = db.Customers
            .Include(customer => customer.Contacts)
            .Include(customer => customer.Loans)
            .Include(customer => customer.WorkItems)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query))
        {
            var term = query.Trim();
            customers = customers.Where(customer =>
                customer.Name.Contains(term) ||
                customer.Contacts.Any(contact => contact.FullName.Contains(term) || (contact.Email != null && contact.Email.Contains(term))) ||
                customer.Loans.Any(loan => loan.LoanNumber.Contains(term)));
        }

        if (!string.IsNullOrWhiteSpace(heatLevel))
        {
            customers = customers.Where(customer => customer.HeatLevel == heatLevel);
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            customers = customers.Where(customer => customer.Status == status);
        }

        return await customers
            .OrderByDescending(customer => customer.HeatLevel == "Hot")
            .ThenBy(customer => customer.Name)
            .Select(customer => new CustomerListItem(
                customer.Id,
                customer.Name,
                customer.Segment,
                customer.Status,
                customer.HeatLevel,
                customer.Contacts.Count,
                customer.Loans.Count,
                customer.WorkItems.Count(item => item.CompletedAt == null),
                customer.Loans
                    .Where(loan => loan.Stage != LoanStage.Dormant)
                    .OrderByDescending(loan => loan.CreatedAt)
                    .Select(loan => loan.Stage)
                    .FirstOrDefault()))
            .ToListAsync();
    }

    public async Task<Customer?> GetCustomerAsync(int id)
    {
        return await db.Customers
            .Include(customer => customer.Contacts)
            .Include(customer => customer.Properties)
                .ThenInclude(property => property.Loans)
            .Include(customer => customer.Loans)
                .ThenInclude(loan => loan.TrustDeeds)
            .Include(customer => customer.WorkItems.OrderBy(item => item.DueAt))
            .FirstOrDefaultAsync(customer => customer.Id == id);
    }

    public async Task<List<Loan>> GetPipelineAsync(LoanStage? stage)
    {
        var loans = db.Loans
            .Include(loan => loan.Customer)
            .Include(loan => loan.Property)
            .AsQueryable();

        if (stage.HasValue)
        {
            loans = loans.Where(loan => loan.Stage == stage.Value);
        }

        return await loans
            .OrderBy(loan => loan.TargetCloseDate)
            .ThenBy(loan => loan.Customer!.Name)
            .ToListAsync();
    }

    public async Task<List<WorkItem>> GetWorkQueueAsync(bool includeCompleted)
    {
        var workItems = db.WorkItems
            .Include(item => item.Customer)
            .Include(item => item.Contact)
            .AsQueryable();

        if (!includeCompleted)
        {
            workItems = workItems.Where(item => item.CompletedAt == null);
        }

        return await workItems
            .OrderBy(item => item.CompletedAt != null)
            .ThenBy(item => item.DueAt)
            .ThenByDescending(item => item.Priority)
            .ToListAsync();
    }

    public async Task CompleteWorkItemAsync(int id)
    {
        var item = await db.WorkItems.FindAsync(id);
        if (item is null || item.CompletedAt is not null)
        {
            return;
        }

        item.CompletedAt = DateTime.UtcNow;
        db.ActivityHistory.Add(new ActivityHistory
        {
            OccurredAt = DateTime.UtcNow,
            ActivityType = "Task",
            Summary = $"{item.Title} completed",
            Result = "Completed",
            Description = $"Closed by demo user from the work queue."
        });
        await db.SaveChangesAsync();
    }

    public async Task<DataModelSnapshot> GetDataModelAsync()
    {
        return new DataModelSnapshot(
            Tables:
            [
                new("Customer", await db.Customers.CountAsync(), "Household or account root, adapted from the legacy Customer and Contact split."),
                new("Contact", await db.Contacts.CountAsync(), "People related to a customer, including borrowers, co-borrowers, agents, and referral sources."),
                new("Property", await db.Properties.CountAsync(), "Addresses and subject properties for current and historical loan work."),
                new("Loan", await db.Loans.CountAsync(), "Pipeline records, programs, rates, target close dates, and stage tracking."),
                new("TrustDeed", await db.TrustDeeds.CountAsync(), "Loan security detail, lender, lien position, amount, rate, and term."),
                new("WorkItem", await db.WorkItems.CountAsync(), "Follow-ups, document requests, appointments, conditions, and import review tasks."),
                new("PickListOption", await db.PickListOptions.CountAsync(), "Controlled lookup values from the legacy picklist pattern."),
                new("Relationship", await db.Relationships.CountAsync(), "Generic links between records, preserving the legacy Related table idea."),
                new("ActivityHistory", await db.ActivityHistory.CountAsync(), "Import, workflow, and audit-style activity trail.")
            ],
            Relationships: await db.Relationships
                .OrderBy(relationship => relationship.RelationshipType)
                .ToListAsync(),
            PickLists: await db.PickListOptions
                .OrderBy(option => option.Category)
                .ThenBy(option => option.SortOrder)
                .ToListAsync());
    }
}

public record DashboardSnapshot(
    int CustomerCount,
    int ActiveLoanCount,
    int OpenTaskCount,
    int OverdueTaskCount,
    decimal PipelineAmount,
    List<StageCount> StageCounts,
    List<WorkItem> DueWork,
    List<ActivityHistory> RecentActivity);

public record StageCount(LoanStage Stage, int Count, decimal Amount);

public record CustomerListItem(
    int Id,
    string Name,
    string Segment,
    string Status,
    string HeatLevel,
    int ContactCount,
    int LoanCount,
    int OpenTaskCount,
    LoanStage CurrentStage);

public record DataModelSnapshot(
    List<TableSummary> Tables,
    List<Relationship> Relationships,
    List<PickListOption> PickLists);

public record TableSummary(string Name, int Records, string Purpose);

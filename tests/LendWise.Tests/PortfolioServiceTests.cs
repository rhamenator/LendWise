using LendWise.Web.Data;
using LendWise.Web.Models;
using LendWise.Web.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace LendWise.Tests;

public class PortfolioServiceTests
{
    [Fact]
    public async Task Demo_seed_creates_anonymized_operating_dataset()
    {
        await using var fixture = await TestFixture.CreateAsync();

        Assert.Equal(4, await fixture.Db.Customers.CountAsync());
        Assert.Equal(4, await fixture.Db.Loans.CountAsync());
        Assert.True(await fixture.Db.WorkItems.AnyAsync(item => item.Priority == WorkPriority.Urgent));
        Assert.DoesNotContain(await fixture.Db.Contacts.Select(contact => contact.Email).ToListAsync(), email => email is not null && !email.EndsWith(".test"));
    }

    [Fact]
    public async Task Customer_search_finds_names_contacts_and_loan_numbers()
    {
        await using var fixture = await TestFixture.CreateAsync();

        var byName = await fixture.Service.SearchCustomersAsync("Patel", null, null);
        var byEmail = await fixture.Service.SearchCustomersAsync("iris.chen", null, null);
        var byLoan = await fixture.Service.SearchCustomersAsync("LW-10041", null, null);

        Assert.Single(byName);
        Assert.Equal("Nora Patel", byName[0].Name);
        Assert.Single(byEmail);
        Assert.Equal("Chen Family Trust", byEmail[0].Name);
        Assert.Single(byLoan);
        Assert.Equal("Rivera Household", byLoan[0].Name);
    }

    [Fact]
    public async Task Dashboard_exposes_pipeline_and_overdue_work()
    {
        await using var fixture = await TestFixture.CreateAsync();

        var dashboard = await fixture.Service.GetDashboardAsync();

        Assert.Equal(4, dashboard.CustomerCount);
        Assert.Equal(3, dashboard.ActiveLoanCount);
        Assert.True(dashboard.PipelineAmount > 1_000_000m);
        Assert.True(dashboard.OverdueTaskCount >= 1);
        Assert.Contains(dashboard.StageCounts, count => count.Stage == LoanStage.Underwriting);
    }

    [Fact]
    public async Task Completing_work_item_updates_queue_and_activity_history()
    {
        await using var fixture = await TestFixture.CreateAsync();
        var openItem = await fixture.Db.WorkItems.FirstAsync(item => item.CompletedAt == null);

        await fixture.Service.CompleteWorkItemAsync(openItem.Id);

        var updated = await fixture.Db.WorkItems.FindAsync(openItem.Id);
        Assert.NotNull(updated?.CompletedAt);
        Assert.True(await fixture.Db.ActivityHistory.AnyAsync(activity => activity.Summary.Contains("completed")));
    }

    private sealed class TestFixture : IAsyncDisposable
    {
        private readonly SqliteConnection connection;

        private TestFixture(SqliteConnection connection, LendWiseDbContext db)
        {
            this.connection = connection;
            Db = db;
            Service = new PortfolioService(db);
        }

        public LendWiseDbContext Db { get; }
        public PortfolioService Service { get; }

        public static async Task<TestFixture> CreateAsync()
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<LendWiseDbContext>()
                .UseSqlite(connection)
                .Options;

            var db = new LendWiseDbContext(options);
            await DemoDataSeeder.SeedAsync(db);

            return new TestFixture(connection, db);
        }

        public async ValueTask DisposeAsync()
        {
            await Db.DisposeAsync();
            await connection.DisposeAsync();
        }
    }
}

using LendWise.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace LendWise.Web.Data;

public static class DemoDataSeeder
{
    private static readonly DateTime SeedNow = new(2026, 6, 18, 9, 0, 0, DateTimeKind.Utc);

    public static async Task SeedAsync(LendWiseDbContext db)
    {
        await db.Database.EnsureCreatedAsync();

        if (await db.Customers.AnyAsync())
        {
            return;
        }

        db.PickListOptions.AddRange(
            Pick("Customer status", "New", 1),
            Pick("Customer status", "Active", 2),
            Pick("Customer status", "Dormant", 3),
            Pick("Customer segment", "First-time buyer", 1),
            Pick("Customer segment", "Move-up buyer", 2),
            Pick("Customer segment", "Investor", 3),
            Pick("Loan purpose", "Purchase", 1),
            Pick("Loan purpose", "Refinance", 2),
            Pick("Loan purpose", "Cash-out refinance", 3),
            Pick("Property type", "Single family", 1),
            Pick("Property type", "Townhome", 2),
            Pick("Property type", "Condo", 3),
            Pick("Relationship type", "Co-borrower", 1),
            Pick("Relationship type", "Referral", 2),
            Pick("Relationship type", "Agent", 3));

        var rivera = new Customer
        {
            Name = "Rivera Household",
            Segment = "First-time buyer",
            Status = "Active",
            HeatLevel = "Hot",
            ReferralSource = "Open-house partner",
            DesiredRate = 6.125m,
            MarketingStartDate = SeedNow.AddDays(-36),
            Notes = "Needs weekly touchpoints and a clean document checklist before underwriting."
        };
        rivera.Contacts.AddRange([
            new Contact
            {
                FullName = "Maya Rivera",
                Role = "Primary borrower",
                Email = "maya.rivera@example.test",
                MobilePhone = "555-0104",
                PreferredLanguage = "English",
                ContactType = "Borrower",
                Status = "Active"
            },
            new Contact
            {
                FullName = "Daniel Rivera",
                Role = "Co-borrower",
                Email = "daniel.rivera@example.test",
                MobilePhone = "555-0107",
                PreferredLanguage = "English",
                ContactType = "Borrower",
                Status = "Active"
            }
        ]);
        rivera.Properties.Add(new Property
        {
            Label = "Subject property",
            Address1 = "412 Juniper Bend",
            City = "Columbus",
            State = "OH",
            Zip = "43215",
            PropertyType = "Single family",
            EstimatedValue = 382000m
        });
        rivera.Loans.Add(new Loan
        {
            LoanNumber = "LW-10041",
            Purpose = "Purchase",
            Program = "30-year fixed conventional",
            Stage = LoanStage.Underwriting,
            Amount = 344000m,
            InterestRate = 6.375m,
            TargetCloseDate = SeedNow.AddDays(18),
            CreatedAt = SeedNow.AddDays(-24),
            TrustDeeds =
            [
                new TrustDeed
                {
                    LenderName = "Harbor First Lending",
                    Position = 1,
                    LoanAmount = 344000m,
                    Rate = 6.375m,
                    TermMonths = 360
                }
            ]
        });
        rivera.WorkItems.AddRange([
            Work(WorkItemType.DocumentRequest, WorkPriority.Urgent, "Collect updated bank statement", "Missing May statement page 3.", SeedNow.AddDays(-1), "Avery Stone"),
            Work(WorkItemType.FollowUp, WorkPriority.High, "Confirm appraisal access", "Listing agent has not confirmed lockbox window.", SeedNow.AddDays(1), "Avery Stone")
        ]);

        var chen = new Customer
        {
            Name = "Chen Family Trust",
            Segment = "Investor",
            Status = "Active",
            HeatLevel = "Warm",
            ReferralSource = "CPA referral",
            DesiredRate = 6.5m,
            MarketingStartDate = SeedNow.AddDays(-72),
            Notes = "Evaluating two rental properties and prefers concise email updates."
        };
        chen.Contacts.Add(new Contact
        {
            FullName = "Iris Chen",
            Role = "Trustee",
            Email = "iris.chen@example.test",
            MobilePhone = "555-0138",
            PreferredLanguage = "English",
            ContactType = "Investor",
            Status = "Active"
        });
        chen.Properties.AddRange([
            new Property
            {
                Label = "Rental candidate",
                Address1 = "88 Beacon Row",
                City = "Madison",
                State = "WI",
                Zip = "53703",
                PropertyType = "Townhome",
                EstimatedValue = 515000m
            },
            new Property
            {
                Label = "Existing rental",
                Address1 = "1937 Crane Avenue",
                City = "Madison",
                State = "WI",
                Zip = "53711",
                PropertyType = "Condo",
                EstimatedValue = 289000m
            }
        ]);
        chen.Loans.Add(new Loan
        {
            LoanNumber = "LW-10056",
            Purpose = "Cash-out refinance",
            Program = "DSCR investor product",
            Stage = LoanStage.Processing,
            Amount = 231000m,
            InterestRate = 7.125m,
            TargetCloseDate = SeedNow.AddDays(31),
            CreatedAt = SeedNow.AddDays(-12)
        });
        chen.WorkItems.Add(Work(WorkItemType.Appointment, WorkPriority.Normal, "Review rent roll assumptions", "Confirm projected vacancy rate and reserve requirements.", SeedNow.AddDays(4), "Morgan Lee"));

        var patel = new Customer
        {
            Name = "Nora Patel",
            Segment = "Move-up buyer",
            Status = "New",
            HeatLevel = "Hot",
            ReferralSource = "Past client referral",
            DesiredRate = 6.25m,
            MarketingStartDate = SeedNow.AddDays(-8),
            Notes = "Needs preapproval letter before weekend showings."
        };
        patel.Contacts.Add(new Contact
        {
            FullName = "Nora Patel",
            Role = "Primary borrower",
            Email = "nora.patel@example.test",
            MobilePhone = "555-0164",
            PreferredLanguage = "English",
            ContactType = "Borrower",
            Status = "Active"
        });
        patel.Properties.Add(new Property
        {
            Label = "Current residence",
            Address1 = "606 Cedar Park",
            City = "Raleigh",
            State = "NC",
            Zip = "27601",
            PropertyType = "Single family",
            EstimatedValue = 438000m
        });
        patel.Loans.Add(new Loan
        {
            LoanNumber = "LW-10063",
            Purpose = "Purchase",
            Program = "Jumbo adjustable",
            Stage = LoanStage.Application,
            Amount = 742500m,
            InterestRate = 6.875m,
            TargetCloseDate = SeedNow.AddDays(45),
            CreatedAt = SeedNow.AddDays(-6)
        });
        patel.WorkItems.AddRange([
            Work(WorkItemType.FollowUp, WorkPriority.Urgent, "Issue preapproval package", "Income review is complete; send agent-ready letter.", SeedNow, "Morgan Lee"),
            Work(WorkItemType.DocumentRequest, WorkPriority.High, "Request signed purchase questionnaire", null, SeedNow.AddDays(2), "Avery Stone")
        ]);

        var moretti = new Customer
        {
            Name = "Moretti Studio",
            Segment = "Refinance",
            Status = "Dormant",
            HeatLevel = "Cool",
            ReferralSource = "Web lead",
            DesiredRate = 5.875m,
            MarketingStartDate = SeedNow.AddDays(-140),
            Notes = "Paused refinance discussion after rate movement; revisit next month."
        };
        moretti.Contacts.Add(new Contact
        {
            FullName = "Leo Moretti",
            Role = "Owner",
            Email = "leo.moretti@example.test",
            MobilePhone = "555-0191",
            PreferredLanguage = "English",
            ContactType = "Business owner",
            Status = "Dormant"
        });
        moretti.Properties.Add(new Property
        {
            Label = "Studio property",
            Address1 = "19 Waverly Place",
            City = "Pittsburgh",
            State = "PA",
            Zip = "15222",
            PropertyType = "Mixed use",
            EstimatedValue = 692000m
        });
        moretti.Loans.Add(new Loan
        {
            LoanNumber = "LW-09982",
            Purpose = "Refinance",
            Program = "Commercial-lite refinance",
            Stage = LoanStage.Dormant,
            Amount = 488000m,
            InterestRate = 7.25m,
            TargetCloseDate = SeedNow.AddDays(62),
            CreatedAt = SeedNow.AddDays(-88)
        });
        moretti.WorkItems.Add(Work(WorkItemType.FollowUp, WorkPriority.Low, "Rate watch check-in", "Send concise update if thirty-year rates improve.", SeedNow.AddDays(21), "Avery Stone"));

        db.Customers.AddRange(rivera, chen, patel, moretti);

        db.Relationships.AddRange(
            new Relationship
            {
                PrimaryEntityType = "Customer",
                PrimaryEntityId = 1,
                SecondaryEntityType = "Customer",
                SecondaryEntityId = 3,
                RelationshipType = "Referral",
                EffectiveDate = SeedNow.AddDays(-8),
                Notes = "Nora Patel was referred by the Rivera household."
            },
            new Relationship
            {
                PrimaryEntityType = "Customer",
                PrimaryEntityId = 2,
                SecondaryEntityType = "Contact",
                SecondaryEntityId = 1,
                RelationshipType = "Agent",
                EffectiveDate = SeedNow.AddDays(-12),
                Notes = "Uses the same buyer agent as the Rivera file."
            });

        db.ActivityHistory.AddRange(
            Activity("Import", "ACT export reviewed", "Completed", "Legacy import map validated against anonymized contact columns.", SeedNow.AddDays(-15)),
            Activity("Loan", "LW-10041 moved to underwriting", "Completed", "Conditions created for asset documentation and appraisal access.", SeedNow.AddDays(-2)),
            Activity("Task", "Preapproval package queued", "Open", "Nora Patel file is ready for letter generation.", SeedNow.AddHours(-3)),
            Activity("Data", "Demo database seeded", "Completed", "Synthetic data loaded for workflow testing.", SeedNow));

        await db.SaveChangesAsync();
    }

    private static PickListOption Pick(string category, string value, int sortOrder)
    {
        return new PickListOption
        {
            Category = category,
            Value = value,
            SortOrder = sortOrder,
            IsReadOnly = true
        };
    }

    private static WorkItem Work(WorkItemType type, WorkPriority priority, string title, string? description, DateTime dueAt, string assignedTo)
    {
        return new WorkItem
        {
            Type = type,
            Priority = priority,
            Title = title,
            Description = description,
            DueAt = dueAt,
            AssignedTo = assignedTo
        };
    }

    private static ActivityHistory Activity(string type, string summary, string result, string description, DateTime occurredAt)
    {
        return new ActivityHistory
        {
            ActivityType = type,
            Summary = summary,
            Result = result,
            Description = description,
            OccurredAt = occurredAt
        };
    }
}

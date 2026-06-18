using System.ComponentModel.DataAnnotations;

namespace LendWise.Web.Models;

public enum LoanStage
{
    Prospect,
    Application,
    Processing,
    Underwriting,
    ClearToClose,
    Closed,
    Dormant
}

public enum WorkPriority
{
    Low,
    Normal,
    High,
    Urgent
}

public enum WorkItemType
{
    FollowUp,
    DocumentRequest,
    Appointment,
    UnderwritingCondition,
    ImportReview
}

public class Customer
{
    public int Id { get; set; }

    [MaxLength(160)]
    public required string Name { get; set; }

    [MaxLength(64)]
    public required string Segment { get; set; }

    [MaxLength(64)]
    public required string Status { get; set; }

    [MaxLength(32)]
    public required string HeatLevel { get; set; }

    [MaxLength(120)]
    public string? ReferralSource { get; set; }

    public decimal? DesiredRate { get; set; }
    public DateTime MarketingStartDate { get; set; }

    [MaxLength(1024)]
    public string? Notes { get; set; }

    public List<Contact> Contacts { get; set; } = [];
    public List<Property> Properties { get; set; } = [];
    public List<Loan> Loans { get; set; } = [];
    public List<WorkItem> WorkItems { get; set; } = [];
}

public class Contact
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }

    [MaxLength(160)]
    public required string FullName { get; set; }

    [MaxLength(64)]
    public required string Role { get; set; }

    [MaxLength(160)]
    public string? Email { get; set; }

    [MaxLength(40)]
    public string? MobilePhone { get; set; }

    [MaxLength(32)]
    public string? PreferredLanguage { get; set; }

    [MaxLength(64)]
    public required string ContactType { get; set; }

    [MaxLength(64)]
    public required string Status { get; set; }

    public List<WorkItem> WorkItems { get; set; } = [];
}

public class Property
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }

    [MaxLength(64)]
    public required string Label { get; set; }

    [MaxLength(160)]
    public required string Address1 { get; set; }

    [MaxLength(80)]
    public required string City { get; set; }

    [MaxLength(2)]
    public required string State { get; set; }

    [MaxLength(12)]
    public required string Zip { get; set; }

    [MaxLength(64)]
    public required string PropertyType { get; set; }

    public decimal EstimatedValue { get; set; }

    public List<Loan> Loans { get; set; } = [];
}

public class Loan
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }

    public int? PropertyId { get; set; }
    public Property? Property { get; set; }

    [MaxLength(64)]
    public required string LoanNumber { get; set; }

    [MaxLength(64)]
    public required string Purpose { get; set; }

    [MaxLength(96)]
    public required string Program { get; set; }

    public LoanStage Stage { get; set; }
    public decimal Amount { get; set; }
    public decimal InterestRate { get; set; }
    public DateTime TargetCloseDate { get; set; }
    public DateTime CreatedAt { get; set; }

    public List<TrustDeed> TrustDeeds { get; set; } = [];
}

public class TrustDeed
{
    public int Id { get; set; }
    public int LoanId { get; set; }
    public Loan? Loan { get; set; }

    [MaxLength(120)]
    public required string LenderName { get; set; }

    public int Position { get; set; }
    public decimal LoanAmount { get; set; }
    public decimal Rate { get; set; }
    public int TermMonths { get; set; }
}

public class WorkItem
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public Customer? Customer { get; set; }

    public int? ContactId { get; set; }
    public Contact? Contact { get; set; }

    public WorkItemType Type { get; set; }
    public WorkPriority Priority { get; set; }

    [MaxLength(160)]
    public required string Title { get; set; }

    [MaxLength(1024)]
    public string? Description { get; set; }

    public DateTime DueAt { get; set; }
    public DateTime? CompletedAt { get; set; }

    [MaxLength(96)]
    public required string AssignedTo { get; set; }

    public bool IsOpen => CompletedAt is null;
}

public class PickListOption
{
    public int Id { get; set; }

    [MaxLength(64)]
    public required string Category { get; set; }

    [MaxLength(96)]
    public required string Value { get; set; }

    public int SortOrder { get; set; }
    public bool IsReadOnly { get; set; }
    public bool IsActive { get; set; } = true;
}

public class Relationship
{
    public int Id { get; set; }

    [MaxLength(32)]
    public required string PrimaryEntityType { get; set; }

    public int PrimaryEntityId { get; set; }

    [MaxLength(32)]
    public required string SecondaryEntityType { get; set; }

    public int SecondaryEntityId { get; set; }

    [MaxLength(64)]
    public required string RelationshipType { get; set; }

    public DateTime EffectiveDate { get; set; }

    [MaxLength(512)]
    public string? Notes { get; set; }
}

public class ActivityHistory
{
    public int Id { get; set; }
    public DateTime OccurredAt { get; set; }

    [MaxLength(64)]
    public required string ActivityType { get; set; }

    [MaxLength(160)]
    public required string Summary { get; set; }

    [MaxLength(64)]
    public required string Result { get; set; }

    [MaxLength(1024)]
    public string? Description { get; set; }
}

using LendWise.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace LendWise.Web.Data;

public class LendWiseDbContext(DbContextOptions<LendWiseDbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Contact> Contacts => Set<Contact>();
    public DbSet<Property> Properties => Set<Property>();
    public DbSet<Loan> Loans => Set<Loan>();
    public DbSet<TrustDeed> TrustDeeds => Set<TrustDeed>();
    public DbSet<WorkItem> WorkItems => Set<WorkItem>();
    public DbSet<PickListOption> PickListOptions => Set<PickListOption>();
    public DbSet<Relationship> Relationships => Set<Relationship>();
    public DbSet<ActivityHistory> ActivityHistory => Set<ActivityHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>()
            .HasMany(customer => customer.Contacts)
            .WithOne(contact => contact.Customer)
            .HasForeignKey(contact => contact.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Customer>()
            .HasMany(customer => customer.Properties)
            .WithOne(property => property.Customer)
            .HasForeignKey(property => property.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Customer>()
            .HasMany(customer => customer.Loans)
            .WithOne(loan => loan.Customer)
            .HasForeignKey(loan => loan.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Customer>()
            .HasMany(customer => customer.WorkItems)
            .WithOne(workItem => workItem.Customer)
            .HasForeignKey(workItem => workItem.CustomerId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Contact>()
            .HasMany(contact => contact.WorkItems)
            .WithOne(workItem => workItem.Contact)
            .HasForeignKey(workItem => workItem.ContactId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Property>()
            .HasMany(property => property.Loans)
            .WithOne(loan => loan.Property)
            .HasForeignKey(loan => loan.PropertyId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Loan>()
            .HasMany(loan => loan.TrustDeeds)
            .WithOne(trustDeed => trustDeed.Loan)
            .HasForeignKey(trustDeed => trustDeed.LoanId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PickListOption>()
            .HasIndex(option => new { option.Category, option.Value })
            .IsUnique();

        modelBuilder.Entity<Customer>().Property(customer => customer.DesiredRate).HasPrecision(6, 3);
        modelBuilder.Entity<Property>().Property(property => property.EstimatedValue).HasPrecision(14, 2);
        modelBuilder.Entity<Loan>().Property(loan => loan.Amount).HasPrecision(14, 2);
        modelBuilder.Entity<Loan>().Property(loan => loan.InterestRate).HasPrecision(6, 3);
        modelBuilder.Entity<TrustDeed>().Property(deed => deed.LoanAmount).HasPrecision(14, 2);
        modelBuilder.Entity<TrustDeed>().Property(deed => deed.Rate).HasPrecision(6, 3);
    }
}

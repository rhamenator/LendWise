using LendWise.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LendWise.Web.Pages.Customers;

public class IndexModel(PortfolioService portfolio) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string? Query { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? HeatLevel { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Status { get; set; }

    public List<CustomerListItem> Customers { get; private set; } = [];
    public string[] HeatLevels { get; } = ["Hot", "Warm", "Cool"];
    public string[] Statuses { get; } = ["New", "Active", "Dormant"];

    public async Task OnGetAsync()
    {
        Customers = await portfolio.SearchCustomersAsync(Query, HeatLevel, Status);
    }
}

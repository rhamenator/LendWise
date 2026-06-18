using LendWise.Web.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LendWise.Web.Pages;

public class IndexModel(PortfolioService portfolio) : PageModel
{
    public DashboardSnapshot Snapshot { get; private set; } = default!;

    public async Task OnGetAsync()
    {
        Snapshot = await portfolio.GetDashboardAsync();
    }
}

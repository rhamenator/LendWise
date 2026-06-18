using LendWise.Web.Models;
using LendWise.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LendWise.Web.Pages.Work;

public class IndexModel(PortfolioService portfolio) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public bool IncludeCompleted { get; set; }

    public List<WorkItem> WorkItems { get; private set; } = [];

    public async Task OnGetAsync()
    {
        WorkItems = await portfolio.GetWorkQueueAsync(IncludeCompleted);
    }

    public async Task<IActionResult> OnPostCompleteAsync(int id)
    {
        await portfolio.CompleteWorkItemAsync(id);
        return RedirectToPage(new { IncludeCompleted });
    }
}

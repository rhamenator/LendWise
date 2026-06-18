using LendWise.Web.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LendWise.Web.Pages.DataModel;

public class IndexModel(PortfolioService portfolio) : PageModel
{
    public DataModelSnapshot Snapshot { get; private set; } = default!;

    public async Task OnGetAsync()
    {
        Snapshot = await portfolio.GetDataModelAsync();
    }
}

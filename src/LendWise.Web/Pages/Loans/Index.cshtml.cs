using LendWise.Web.Models;
using LendWise.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LendWise.Web.Pages.Loans;

public class IndexModel(PortfolioService portfolio) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public LoanStage? Stage { get; set; }

    public List<Loan> Loans { get; private set; } = [];
    public LoanStage[] Stages { get; } = Enum.GetValues<LoanStage>();

    public async Task OnGetAsync()
    {
        Loans = await portfolio.GetPipelineAsync(Stage);
    }
}

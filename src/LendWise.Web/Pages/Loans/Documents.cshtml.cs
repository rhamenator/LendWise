using LendWise.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LendWise.Web.Pages.Loans;

public class DocumentsModel(PortfolioService portfolio) : PageModel
{
    public LoanDocumentChecklist Checklist { get; private set; } = default!;

    public async Task<IActionResult> OnGetAsync(int loanId)
    {
        var checklist = await portfolio.GetDocumentChecklistAsync(loanId);
        if (checklist is null)
        {
            return NotFound();
        }

        Checklist = checklist;
        return Page();
    }

    public async Task<IActionResult> OnPostReceiveAsync(int loanId, int documentId)
    {
        await portfolio.MarkDocumentReceivedAsync(documentId);
        return RedirectToPage(new { loanId });
    }
}

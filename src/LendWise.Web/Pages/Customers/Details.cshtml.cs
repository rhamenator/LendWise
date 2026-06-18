using LendWise.Web.Models;
using LendWise.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LendWise.Web.Pages.Customers;

public class DetailsModel(PortfolioService portfolio) : PageModel
{
    public Customer Customer { get; private set; } = default!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var customer = await portfolio.GetCustomerAsync(id);
        if (customer is null)
        {
            return NotFound();
        }

        Customer = customer;
        return Page();
    }
}

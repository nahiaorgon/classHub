using ClassHub.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClassHub.Web.Pages.Account;

public class LogoutModel(IAuthService authService) : PageModel
{
    public async Task<IActionResult> OnGetAsync()
    {
        await authService.SignOutAsync(HttpContext);
        return LocalRedirect("/Account/Login");
    }
}

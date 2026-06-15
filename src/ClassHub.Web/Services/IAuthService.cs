using ClassHub.Web.Models;

namespace ClassHub.Web.Services;

public interface IAuthService
{
    Task<AppUser?> ValidateAsync(string email, string password);
    Task SignInAsync(HttpContext context, AppUser user);
    Task SignOutAsync(HttpContext context);
}

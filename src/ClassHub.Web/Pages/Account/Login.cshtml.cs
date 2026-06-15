using ClassHub.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ClassHub.Web.Pages.Account;

public class LoginModel(IAuthService authService) : PageModel
{
    [BindProperty]
    public InputModel Input { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public class InputModel
    {
        [Required(ErrorMessage = "El email es requerido.")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida.")]
        public string Password { get; set; } = string.Empty;
    }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        if (!ModelState.IsValid) return Page();

        var user = await authService.ValidateAsync(Input.Email, Input.Password);
        if (user is null)
        {
            ErrorMessage = "Email o contraseña incorrectos.";
            return Page();
        }

        await authService.SignInAsync(HttpContext, user);

        returnUrl = returnUrl?.TrimStart('/');
        return LocalRedirect(string.IsNullOrEmpty(returnUrl) ? "/" : $"/{returnUrl}");
    }
}

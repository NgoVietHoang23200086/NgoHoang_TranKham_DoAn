using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using JobBoard.Application.DTOs;
using JobBoard.Application.Interfaces;
using JobBoard.Domain.Entities;

namespace JobBoard.Web.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IAuthService _authService;
        [BindProperty] public LoginDto LoginDto { get; set; } = new();
        public string? ErrorMessage { get; set; }

        public LoginModel(IAuthService authService) { _authService = authService; }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _authService.LoginAsync(LoginDto);
            if (user == null)
            {
                ErrorMessage = "Email hoặc mật khẩu không đúng.";
                return Page();
            }
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserRole", user.Role.ToString());
            HttpContext.Session.SetString("UserName", user.Name);
            return RedirectToPage("/Dashboard/Index");
        }
    }
}

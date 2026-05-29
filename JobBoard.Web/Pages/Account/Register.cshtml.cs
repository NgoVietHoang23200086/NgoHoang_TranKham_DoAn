using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using JobBoard.Application.DTOs;
using JobBoard.Application.Interfaces;
using JobBoard.Domain.Exceptions;

namespace JobBoard.Web.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly IAuthService _authService;
        [BindProperty] public RegisterDto RegisterDto { get; set; } = new();
        public string? ErrorMessage { get; set; }

        public RegisterModel(IAuthService authService) { _authService = authService; }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var user = await _authService.RegisterAsync(RegisterDto);
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("UserRole", user.Role.ToString());
                HttpContext.Session.SetString("UserName", user.Name);
                return RedirectToPage("/Dashboard/Index");
            }
            catch (JobApplicationException ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
        }
    }
}

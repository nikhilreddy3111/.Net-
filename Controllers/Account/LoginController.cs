using EmployeeManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.Constants;
using System.Data;
using EmployeeManagement.DapperContent;

namespace EmployeeManagement.Controllers.Account
{
    [Route("account/[Controller]")]
    public class LoginController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IDapperContent _dapperContent;

        public LoginController(SignInManager<IdentityUser> signInManager,UserManager<IdentityUser> userManager,
            IDapperContent dapperContent)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _dapperContent = dapperContent;
        }

        [Route("~/account/Login")]
        [HttpGet(nameof(Index))]
        public IActionResult Index()
        {
            return View(new Login());
        }

        [HttpPost(nameof(Login))]
        public async Task<IActionResult> Login([FromForm] Login loginDetails)
        {
            Microsoft.AspNetCore.Identity.SignInResult loggedIn = await _signInManager.PasswordSignInAsync(loginDetails.Email, loginDetails.Password, false, true);
            
            if (loggedIn.Succeeded)
            {
                IdentityUser? user= await _userManager.GetUserAsync(User);
                string query = $" SELECT 1 FROM AspNetUserRoles ur INNER JOIN AspNetRoles r ON ur.RoleId = r.Id WHERE ur.UserId = '{user.Id}' AND r.Name = '{Roles.Admin.ToString()}'";
                int result = await Task.FromResult(_dapperContent.Get<int>(query, null, commandType: CommandType.Text));
                if (result != 1)
                {
                    return RedirectToAction("Index", "Home");
                }
                return RedirectToAction("GetAllEmployees", "Employee");
            }
            loginDetails.ErrorMessage = "Login Failed";
            return View(nameof(Index),loginDetails);
        }

        [HttpGet(nameof(Logout))]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

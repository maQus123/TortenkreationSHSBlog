namespace TortenkreationSHSBlog.Controllers {

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using TortenkreationSHSBlog.Models;

    public class AuthController : Controller {

        private readonly AppConfiguration appSettings;

        public AuthController(IOptions<AppConfiguration> appSettings) : base() {
            this.appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login() {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User user) {
            //var currentUser = this.Users.Where(u => u.Username == user.Username).FirstOrDefault();
            //if (currentUser?.Password == user.Password) {
            if (this.appSettings.Username == user.Username && this.appSettings.Password == user.Password) {
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, user.Username)
                };
                var userIdentity = new ClaimsIdentity(claims, "login");
                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return Redirect("/");
            }
            return View();
        }

    }

}
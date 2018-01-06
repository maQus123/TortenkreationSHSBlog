namespace TortenkreationSHSBlog.Controllers {

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    public class AdminController : Controller {

        public AdminController() {
            //nothing to do
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index() {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout() {
            //TODO Kommt hier nie an :(
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToRoute("default");
        }

    }

}
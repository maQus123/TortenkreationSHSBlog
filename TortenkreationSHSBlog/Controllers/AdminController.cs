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

        [HttpGet, Authorize]
        public IActionResult Index() {
            return View();
        }

        [HttpPost, Authorize, ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string logout = null) {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }

    }

}
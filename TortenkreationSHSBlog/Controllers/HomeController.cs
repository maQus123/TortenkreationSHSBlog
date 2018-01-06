namespace TortenkreationSHSBlog.Controllers {

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller {

        public HomeController() : base() {
            //nothing to do
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index() {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AboutMe() {
            return View();
        }

    }

}
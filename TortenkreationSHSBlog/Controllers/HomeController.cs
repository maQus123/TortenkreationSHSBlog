namespace TortenkreationSHSBlog.Controllers {

    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller {

        public HomeController() : base() {
            //nothing to do
        }

        [HttpGet]
        public IActionResult Index() {
            return View();
        }

        [HttpGet]
        public IActionResult AboutMe() {
            return View();
        }

    }

}
namespace TortenkreationSHSBlog.Controllers {

    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using TortenkreationSHSBlog.Models;
    using TortenkreationSHSBlog.Persistence;

    public class PostsController : Controller {

        private readonly IPostRepository postRepository;

        public PostsController(IPostRepository postRepository) : base() {
            this.postRepository = postRepository;
        }

        [HttpGet]
        public async Task<IActionResult> List() {
            var posts = await this.postRepository.GetAll();
            return View(posts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Post post) {
            if (ModelState.IsValid) {
                await this.postRepository.Add(post);
                return RedirectToAction("List");
            } else {
                return View(post);
            }
        }

        [HttpGet]
        public IActionResult Create() {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Detail(string year, string month, string slug) {
            var post = await this.postRepository.GetByUrl(year, month, slug);
            if (null != post) {
                return View(post);
            } else {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Post post) {
            var existingPost = await this.postRepository.GetById(id);
            if (null != existingPost) {
                if (existingPost.Id == id && ModelState.IsValid) {
                    existingPost.UpdateFrom(post);
                    await this.postRepository.Update(existingPost);
                    return RedirectToAction("Detail", new { year = existingPost.CreatedAt.Year.ToString(), month = existingPost.CreatedAt.Month.ToString("00"), slug = existingPost.Slug() });
                }
            }
            return View(post);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id) {
            var post = await this.postRepository.GetById(id);
            if (null != post) {
                return View(post);
            } else {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id) {
            var post = await this.postRepository.GetById(id);
            if (null != post) {
                await this.postRepository.Delete(post);
                return RedirectToAction("List");
            } else {
                return NotFound();
            }
        }

    }

}
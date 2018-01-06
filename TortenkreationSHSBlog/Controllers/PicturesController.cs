namespace TortenkreationSHSBlog.Controllers {

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using System;
    using System.Threading.Tasks;
    using TortenkreationSHSBlog.Models;
    using TortenkreationSHSBlog.Persistence;

    public class PicturesController : Controller {

        private readonly IPictureRepository pictureRepository;
        private readonly AppConfiguration appSettings;

        public PicturesController(IPictureRepository pictureRepository, IOptions<AppConfiguration> appSettings) : base() {
            this.pictureRepository = pictureRepository;
            this.appSettings = appSettings.Value;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create() {
            var pictureViewModel = new PictureViewModel();
            return View("CreateOrEdit", pictureViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(PictureViewModel pictureViewModel) {
            try {
                if (!ModelState.IsValid || !pictureViewModel.IsFileValid()) {
                    return View("CreateOrEdit", pictureViewModel);
                }
                if (await this.pictureRepository.IsTitleExisting(pictureViewModel.Title)) {
                    pictureViewModel.ValidationMessage = "Titel schon vorhanden.";
                    return View("CreateOrEdit", pictureViewModel);
                }
                var picture = new Picture(pictureViewModel);
                await picture.GenerateThumbnail(appSettings.ApiKey);
                await this.pictureRepository.Add(picture);
                return RedirectToAction("List");
            } catch (Exception ex) {
                pictureViewModel.ValidationMessage = ex.Message;
                return View("CreateOrEdit", pictureViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Delete(int id, PictureViewModel pictureViewModel) {
            try {
                var picture = await this.pictureRepository.GetById(id);
                if (null == picture || picture.Id != id) {
                    pictureViewModel.ValidationMessage = "Serverfehler. Bestehendes Foto nicht vorhanden.";
                    return View("CreateOrEdit", pictureViewModel);
                }
                await this.pictureRepository.Delete(picture);
                return RedirectToAction("List");
            } catch (Exception ex) {
                pictureViewModel.ValidationMessage = ex.Message;
                return View("CreateOrEdit", pictureViewModel);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Detail(string pictureUrl) {
            var picture = await this.pictureRepository.GetByUrl(pictureUrl);
            if (null == picture) {
                return NotFound();
            }
            return File(picture.File, picture.ContentType);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id) {
            var picture = await this.pictureRepository.GetById(id);
            if (null == picture) {
                return NotFound();
            }
            var pictureViewModel = new PictureViewModel(picture);
            return View("CreateOrEdit", pictureViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, PictureViewModel pictureViewModel) {
            try {
                if (!ModelState.IsValid || !pictureViewModel.IsFileValid()) {
                    return View("CreateOrEdit", pictureViewModel);
                }
                var existingPicture = await this.pictureRepository.GetById(id);
                if (null == existingPicture || existingPicture.Id != id) {
                    pictureViewModel.ValidationMessage = "Serverfehler. Bestehendes Foto nicht vorhanden.";
                    return View("CreateOrEdit", pictureViewModel);
                }
                if ((pictureViewModel.Title != existingPicture.Title) && (await this.pictureRepository.IsTitleExisting(pictureViewModel.Title))) {
                    pictureViewModel.ValidationMessage = "Titel schon vorhanden.";
                    return View("CreateOrEdit", pictureViewModel);
                }
                var picture = new Picture(pictureViewModel);
                await picture.GenerateThumbnail(appSettings.ApiKey);
                existingPicture.UpdateFrom(picture);
                await this.pictureRepository.Update(existingPicture);
                return RedirectToAction("List");
            } catch (Exception ex) {
                pictureViewModel.ValidationMessage = ex.Message;
                return View("CreateOrEdit", pictureViewModel);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> List() {
            var pictures = await this.pictureRepository.GetAll();
            return View(pictures);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Thumbnail(string pictureUrl) {
            var picture = await this.pictureRepository.GetByUrl(pictureUrl);
            if (null == picture) {
                return NotFound();
            }
            return File(picture.ThumbnailFile, picture.ContentType);
        }

    }

}
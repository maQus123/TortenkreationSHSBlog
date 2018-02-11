namespace TortenkreationSHSBlog.Controllers {

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using TortenkreationSHSBlog.Models;
    using TortenkreationSHSBlog.Persistence;

    public class PicturesController : Controller {

        private readonly IPictureRepository pictureRepository;
        private readonly AppConfiguration appSettings;
        private readonly int MAX_BYTES = 5 * 1024 * 1024; //5MB
        private readonly string[] ACCEPTED_FILE_TYPES = new[] { ".jpg", ".jpeg", ".png", ".tif", ".bmp" };

        public PicturesController(IPictureRepository pictureRepository, IOptions<AppConfiguration> appSettings) : base() {
            this.pictureRepository = pictureRepository;
            this.appSettings = appSettings.Value;
        }

        [HttpGet, Authorize]
        public IActionResult Create() {
            var pictureViewModel = new CreatePictureViewModel();
            return View(pictureViewModel);
        }

        [HttpPost, Authorize, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePictureViewModel viewModel) {
            if (!ModelState.IsValid) {
                return View(viewModel);
            }
            if (viewModel.File == null || viewModel.File?.Length == 0) {
                ModelState.AddModelError("File", "Foto ungültig/leer.");
                return View(viewModel);
            }
            if (!this.ACCEPTED_FILE_TYPES.Any(s => s == Path.GetExtension(viewModel.File.FileName).ToLower())) {
                ModelState.AddModelError("File", "Foto-Dateiformat nicht erlaubt (nur JPG, PNG, BMP oder GIF).");
                return View(viewModel);
            }
            if (viewModel.File.Length > this.MAX_BYTES) {
                ModelState.AddModelError("File", "Foto darf nicht größer als 5MB sein.");
                return View(viewModel);
            }
            if (await this.pictureRepository.IsTitleExisting(viewModel.Title)) {
                ModelState.AddModelError("Title", "Titel bereits vergeben.");
                return View(viewModel);
            }
            var picture = new Picture(viewModel);
            await picture.GenerateThumbnail(appSettings.ApiKey);
            await this.pictureRepository.Add(picture);
            return RedirectToAction("List");
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> Delete(int id) {
            var picture = await this.pictureRepository.GetById(id);
            if (null == picture) {
                return NotFound();
            }
            var viewModel = new EditPictureViewModel(picture);
            return View(viewModel);
        }

        [HttpPost, Authorize, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, EditPictureViewModel viewModel) {
            var picture = await this.pictureRepository.GetById(id);
            if (null == picture || picture?.Id != id) {
                return NotFound();
            }
            await this.pictureRepository.Delete(picture);
            return RedirectToAction("List");
        }

        [HttpGet, AllowAnonymous]
        public async Task<IActionResult> Detail(string pictureUrl) {
            var picture = await this.pictureRepository.GetByUrl(pictureUrl);
            if (null == picture) {
                return NotFound();
            }
            return File(picture.File, picture.ContentType);
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> Edit(int id) {
            var picture = await this.pictureRepository.GetById(id);
            if (null == picture) {
                return NotFound();
            }
            var viewModel = new EditPictureViewModel(picture);
            return View(viewModel);
        }

        [HttpPost, Authorize, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditPictureViewModel viewModel) {
            if (!ModelState.IsValid) {
                return View(viewModel);
            }
            var picture = await this.pictureRepository.GetById(id);
            if (null == picture || picture?.Id != id) {
                return NotFound();
            }
            if (viewModel.Title != picture.Title && await this.pictureRepository.IsTitleExisting(viewModel.Title)) {
                ModelState.AddModelError("Title", "Titel bereits vergeben.");
                return View(viewModel);
            }
            picture.UpdateFrom(viewModel);
            await this.pictureRepository.Update(picture);
            return RedirectToAction("List");
        }

        [HttpGet, AllowAnonymous]
        public async Task<IActionResult> List(ListViewModel viewModel) {
            viewModel.Pictures = await this.pictureRepository.GetAll(User.Identity.IsAuthenticated, viewModel.Occasion);
            return View(viewModel);
        }

        [HttpGet, AllowAnonymous]
        public async Task<IActionResult> Thumbnail(string pictureUrl) {
            var picture = await this.pictureRepository.GetByUrl(pictureUrl);
            if (null == picture) {
                return NotFound();
            }
            return File(picture.ThumbnailFile, picture.ContentType);
        }

    }

}
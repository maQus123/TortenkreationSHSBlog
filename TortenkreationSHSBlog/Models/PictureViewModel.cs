﻿namespace TortenkreationSHSBlog.Models {

    using Microsoft.AspNetCore.Http;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;

    public class PictureViewModel {

        private readonly int MAX_BYTES = 5 * 1024 * 1024; //5MB
        private readonly string[] ACCEPTED_FILE_TYPES = new[] { ".jpg", ".jpeg", ".png", ".tif", ".bmp" };
        private readonly int TITLE_LENGTH = 30;

        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public int TitleLength { get; set; }

        [Required]
        public IFormFile File { get; set; }

        [Required]
        public Occasion SelectedOccasion { get; set; }

        public string ValidationMessage { get; set; }

        public PictureViewModel() {
            this.TitleLength = TITLE_LENGTH;
        }

        public PictureViewModel(Picture picture) : this() {
            this.Id = picture.Id;
            this.Title = picture.Title;
            this.SelectedOccasion = picture.Occasion;
        }

        public byte[] GetFileAsByteArray() {
            using (var memoryStream = new MemoryStream()) {
                this.File.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public bool IsFileValid() {
            var isValid = true;
            var file = this.File;
            if (file == null || file.Length == 0) {
                this.ValidationMessage = "Foto ungültig (leer).";
                isValid = false;
            } else if (!this.ACCEPTED_FILE_TYPES.Any(s => s == Path.GetExtension(file.FileName).ToLower())) {
                this.ValidationMessage = "Ungültiges Dateiformat. Erlaubt sind nur JPG, PNG, BMP und GIF.";
                isValid = false;
            } else if (file.Length > this.MAX_BYTES) {
                this.ValidationMessage = "Ungültige Dateigröße. Foto darf nicht größer als 5MB sein.";
                isValid = false;
            }
            return isValid;
        }

        public bool IsDirty() {
            var dirty = false;
            if (this.Id != 0) {
                //Id has been given by back-end -> Edit view
                dirty = true;
            }
            return dirty;
        }

    }

}
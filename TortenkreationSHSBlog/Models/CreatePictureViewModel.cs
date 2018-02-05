namespace TortenkreationSHSBlog.Models {

    using Microsoft.AspNetCore.Http;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.IO;

    public class CreatePictureViewModel {

        [Required, StringLength(50, MinimumLength = 3)]
        public string Title { get; set; }

        [Required]
        public IFormFile File { get; set; }

        [Required]
        public Occasion Occasion { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public CreatePictureViewModel() {
            this.CreatedAt = new DateTimeOffset(DateTime.UtcNow);
        }

        public byte[] GetFileAsByteArray() {
            using (var memoryStream = new MemoryStream()) {
                this.File.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

    }

}
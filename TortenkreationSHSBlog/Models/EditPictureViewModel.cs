namespace TortenkreationSHSBlog.Models {

    using System;
    using System.ComponentModel.DataAnnotations;

    public class EditPictureViewModel {

        public EditPictureViewModel() {
            this.CreatedAt = new DateTimeOffset(DateTime.UtcNow);
        }

        public EditPictureViewModel(Picture picture) : this() {
            this.Id = picture.Id;
            this.Title = picture.Title;
            this.Occasion = picture.Occasion;
            this.CreatedAt = picture.CreatedAt;
            this.ThumbnailUrl = "/img/thumbnail/" + picture.GetUrl();
            this.IsPublished = picture.IsPublished;
        }

        public int Id { get; set; }

        [Required, StringLength(50, MinimumLength = 3)]
        public string Title { get; set; }

        [Required]
        public Occasion Occasion { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string ThumbnailUrl { get; set; }

        public bool IsPublished { get; set; }

    }

}
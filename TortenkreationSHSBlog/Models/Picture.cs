namespace TortenkreationSHSBlog.Models {
    using System;
    using System.Globalization;
    using System.IO;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public class Picture : PersistentEntity {

        private readonly int THUMBNAIL_WIDTH = 400;
        private readonly int THUMBNAIL_HEIGHT = 300;

        public string Title { get; set; }

        public byte[] File { get; set; }

        public byte[] ThumbnailFile { get; set; }

        public string ContentType { get; set; }

        public string Extension { get; set; }

        public Occasion Occasion { get; set; }

        public Picture() : base() {
            //nothing to do
        }

        public Picture(PictureViewModel pictureViewModel) : this() {
            this.Title = pictureViewModel.Title;
            this.Occasion = pictureViewModel.SelectedOccasion;
            this.ContentType = pictureViewModel.File.ContentType;
            this.Extension = Path.GetExtension(pictureViewModel.File.FileName);
            this.File = pictureViewModel.GetFileAsByteArray();
        }

        public string GetUrl() {
            var filePath = (this.Title + this.Extension).ToLower();
            filePath = Regex.Replace(filePath, "ö", "oe");
            filePath = Regex.Replace(filePath, "ä", "ae");
            filePath = Regex.Replace(filePath, "ü", "ue");
            filePath = Regex.Replace(filePath, "ß", "ss");
            filePath = Regex.Replace(filePath, @"[^a-z0-9\s-.]", "");
            filePath = Regex.Replace(filePath, @"\s+", " ").Trim();
            filePath = Regex.Replace(filePath, @"\s", "-");
            return filePath;
        }

        public async Task GenerateThumbnail(string apiKey) {
            var url = $@"https://westeurope.api.cognitive.microsoft.com/vision/v1.0/generateThumbnail?width={THUMBNAIL_WIDTH}&height={THUMBNAIL_HEIGHT}&smartCropping=true";
            using (HttpClient client = new HttpClient()) {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apiKey);
                using (ByteArrayContent content = new ByteArrayContent(this.File)) {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    HttpResponseMessage response = await client.PostAsync(url, content);
                    if (response.IsSuccessStatusCode) {
                        this.ThumbnailFile = await response.Content.ReadAsByteArrayAsync();
                    } else {
                        throw new HttpRequestException("Thumbnail-Generierung fehlgeschlagen");
                    }
                }
            }
        }

        public string GetCreatedAtString() {
            return this.CreatedAt.ToString(CultureInfo.InvariantCulture);
        }

        public void UpdateFrom(Picture picture) {
            this.Title = picture.Title;
            this.File = picture.File;
            this.ThumbnailFile = picture.ThumbnailFile;
            this.Occasion = picture.Occasion;
            this.ContentType = picture.ContentType;
            this.Extension = picture.Extension;
            return;
        }

    }

}
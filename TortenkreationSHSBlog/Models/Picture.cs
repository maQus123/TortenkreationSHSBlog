namespace TortenkreationSHSBlog.Models {

    using System;
    using System.IO;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public class Picture {

        public readonly int ThumbnailWidth = 400;
        public readonly int ThumbnailHeight = 300;

        public int Id { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string Title { get; set; }

        public byte[] File { get; set; }

        public byte[] ThumbnailFile { get; set; }

        public string ContentType { get; set; }

        public string Extension { get; set; }

        public Occasion Occasion { get; set; }

        public Picture() {
            this.CreatedAt = new DateTimeOffset(DateTime.Now);
        }

        public Picture(CreatePictureViewModel pictureViewModel) : this() {
            this.Title = pictureViewModel.Title;
            this.Occasion = pictureViewModel.Occasion;
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
            var url = $@"https://westeurope.api.cognitive.microsoft.com/vision/v1.0/generateThumbnail?width={ThumbnailWidth}&height={ThumbnailHeight}&smartCropping=true";
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

        public void UpdateFrom(EditPictureViewModel picture) {
            this.Title = picture.Title;
            this.Occasion = picture.Occasion;
            return;
        }

    }

}
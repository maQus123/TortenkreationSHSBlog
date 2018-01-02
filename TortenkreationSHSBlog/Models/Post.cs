namespace TortenkreationSHSBlog.Models {

    using CommonMark;
    using System.Text.RegularExpressions;

    public class Post : PersistentEntity {

        public string Title { get; set; }

        public string Content { get; set; }

        public bool IsPublished { get; set; }

        public Post() : base() {
            //nothing to do
        }

        public string ContentAsHtml() {
            var html = CommonMarkConverter.Convert(this.Content);
            return html;
        }

        public string Slug() {
            string slug = this.Title.ToLower();
            slug = Regex.Replace(slug, "ö", "oe");
            slug = Regex.Replace(slug, "ä", "ae");
            slug = Regex.Replace(slug, "ü", "ue");
            slug = Regex.Replace(slug, "ß", "ss");
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
            slug = Regex.Replace(slug, @"\s+", " ").Trim();
            slug = slug.Substring(0, slug.Length <= 45 ? slug.Length : 45).Trim();
            slug = Regex.Replace(slug, @"\s", "-");
            return slug;
        }

        public void UpdateFrom(Post post) {
            this.Title = post.Title;
            this.Content = post.Content;
            this.IsPublished = post.IsPublished;
            return;
        }

    }

}
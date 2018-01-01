namespace TortenkreationSHSBlog.Persistence {

    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TortenkreationSHSBlog.Models;

    public class PostRepository : IPostRepository {

        private readonly DataContext dataContext;

        public PostRepository(DataContext dataContext) {
            this.dataContext = dataContext;
        }

        public async Task Add(Post post) {
            await this.dataContext.AddAsync(post);
            await this.dataContext.SaveChangesAsync();
            return;
        }

        public async Task<IEnumerable<Post>> GetAll() {
            var posts = await this.dataContext.Posts.OrderByDescending(p => p.CreatedAt).ToListAsync();
            return posts;
        }

        public async Task<Post> GetByUrl(string year, string month, string slug) {
            var post = await this.dataContext.Posts.SingleOrDefaultAsync(p => p.Slug() == slug && p.CreatedAt.ToString("yyyyMM") == string.Concat(year, month));
            return post;
        }

        public async Task<Post> GetById(int id) {
            var post = await this.dataContext.Posts.SingleOrDefaultAsync(p => p.Id == id);
            return post;
        }

        public async Task Update(Post post) {
            this.dataContext.Posts.Update(post);
            await this.dataContext.SaveChangesAsync();
            return;
        }

        public async Task Delete(Post post) {
            this.dataContext.Posts.Remove(post);
            await this.dataContext.SaveChangesAsync();
            return;
        }

    }

}
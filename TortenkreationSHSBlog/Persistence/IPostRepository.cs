namespace TortenkreationSHSBlog.Persistence {

    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TortenkreationSHSBlog.Models;

    public interface IPostRepository {

        Task<IEnumerable<Post>> GetAll();
        Task<Post> GetById(int id);
        Task Add(Post post);
        Task<Post> GetByUrl(string year, string month, string slug);
        Task Update(Post post);
        Task Delete(Post post);

    }

}
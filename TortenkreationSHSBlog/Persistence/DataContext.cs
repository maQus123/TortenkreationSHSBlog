namespace TortenkreationSHSBlog.Persistence {

    using Microsoft.EntityFrameworkCore;
    using TortenkreationSHSBlog.Models;

    public class DataContext : DbContext {

        public DbSet<Post> Posts { get; set; }

        public DbSet<Picture> Pictures { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) {
            //nohting to do
        }

    }

}
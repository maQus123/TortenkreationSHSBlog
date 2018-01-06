namespace TortenkreationSHSBlog {
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using TortenkreationSHSBlog.Persistence;

    public class Startup {

        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => {
                    options.LoginPath = "/login/";
                });
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IPictureRepository, PictureRepository>();
            services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase("Database"));
            services.AddMvc();
            services.Configure<RouteOptions>(options => { options.AppendTrailingSlash = true; options.LowercaseUrls = true; });
            services.Configure<AppConfiguration>(Configuration.GetSection("AppConfiguration"));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            app.UseAuthentication();
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            app.UseStaticFiles();
            app.UseMvc(routes => {
                //Home routes
                routes.MapRoute(
                    name: "home",
                    template: "",
                    defaults: new { controller = "Home", action = "Index" });
                routes.MapRoute(
                    name: "about-me",
                    template: "ueber-mich",
                    defaults: new { controller = "Home", action = "AboutMe" });
                //Posts routes
                routes.MapRoute(
                    name: "list-posts",
                    template: "blog",
                    defaults: new { controller = "Posts", action = "List" });
                routes.MapRoute(
                    name: "post-detail",
                    template: "{year}/{month}/{slug}",
                    constraints: new { year = @"\d{4}", month = @"\d{2}", slug = @"^[a-z0-9-]+$" },
                    defaults: new { controller = "Posts", action = "Detail" });
                routes.MapRoute(
                    name: "create-post",
                    template: "blog/create-post",
                    defaults: new { controller = "Posts", action = "Create" });
                routes.MapRoute(
                    name: "edit-post",
                    template: "blog/edit-post/{id:int}",
                    defaults: new { controller = "Posts", action = "Edit" });
                //Pictures routes
                routes.MapRoute(
                    name: "list-pictures",
                    template: "torten/{occasion?}",
                    defaults: new { controller = "Pictures", action = "List" });
                routes.MapRoute(
                    name: "create-picture",
                    template: "torten/new/picture",
                    defaults: new { controller = "Pictures", action = "Create" });
                routes.MapRoute(
                    name: "edit-picture",
                    template: "torten/edit-picture/{id:int}",
                    defaults: new { controller = "Pictures", action = "Edit" });
                routes.MapRoute(
                    name: "delete-picture",
                    template: "torten/delete-picture/{id:int}",
                    defaults: new { controller = "Pictures", action = "Delete" });
                routes.MapRoute(
                    name: "picture-detail",
                    template: "img/detail/{pictureUrl}",
                    constraints: new { pictureUrl = @"^[a-z0-9-.]+$" },
                    defaults: new { controller = "Pictures", action = "Detail" });
                routes.MapRoute(
                    name: "picture-thumbnail",
                    template: "img/thumbnail/{pictureUrl}",
                    constraints: new { pictureUrl = @"^[a-z0-9-.]+$" },
                    defaults: new { controller = "Pictures", action = "Thumbnail" });
                //Auth
                routes.MapRoute(
                    name: "login",
                    template: "login",
                    defaults: new { controller = "Auth", action = "Login" });
                //Auth
                routes.MapRoute(
                    name: "admin",
                    template: "admin",
                    defaults: new { controller = "Admin", action = "Index" });
            });
        }

    }

}
using FLRApplication.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FLRApplication.Startup))]
namespace FLRApplication
{
    public partial class Startup
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateUserAndRoles();
            //UpdateAge();
        }

        public void CreateUserAndRoles()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            if (!roleManager.RoleExists("Administrator"))
            {
                var role = new IdentityRole("Administrator");
                roleManager.Create(role);

                var user = new ApplicationUser();
                user.UserName = "admin01@foodlovers.co.za";
                user.Email = "admin01@foodlovers.co.za";
                user.FirstName = "Administrator";
                user.LastName = "Food Lovers";
                user.EmailConfirmed = true;
                string pwd = "Admin01@FLR";

                var newuser = userManager.Create(user, pwd);
                if (newuser.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Administrator");
                }
            }

            if (!roleManager.RoleExists("Customer"))
            {
                var role = new IdentityRole("Customer");
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("Driver"))
            {
                var role = new IdentityRole("Driver");
                roleManager.Create(role);
            }
        }

        //public void birthdayPromo()
        //{

        //}

        //public void UpdateAge()
        //{

        //}

    }
}

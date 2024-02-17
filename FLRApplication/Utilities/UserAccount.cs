using System.Linq;
using FLRApplication.Models;

namespace FLRApplication.Utilities
{
    public class AppUsers
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        public ApplicationUser GetUser(string email)
        {
            var user = db.Users
                    .Where(x => x.Email == email)
                    .FirstOrDefault();
            return user;
        }
    }
}
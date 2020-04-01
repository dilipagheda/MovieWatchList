using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieWatchList.Data
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.Watchlist = new HashSet<UserMovie>();
        }
        public string FirstName { get; set; }
        public virtual ICollection<UserMovie> Watchlist { get; set; }
    }
}

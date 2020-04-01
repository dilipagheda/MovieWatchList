using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieWatchList.Data
{
    public class UserMovie
    {
        public string UserId { get; set; }
        public int MovieId { get; set; }
        public bool Watched { get; set; }
        public int Rating { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Movie Movie { get; set; }
    }
}

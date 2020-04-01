using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieWatchList.ViewModels
{
    public class MovieViewModel
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public bool InWatchlist { get; set; }
        public bool Watched { get; set; }
        public int? Rating { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Business.MovieIntegration
{
    public class MovieApiMovieModel
    {
        public int id { get; set; }

        public string original_title { get; set; }

        public string overview { get; set; }

        public string title { get; set; }

        public double vote_average { get; set; }

        public int vote_count { get; set; }

        public string poster_path { get; set; }

        public double popularity { get; set; }
    }
}

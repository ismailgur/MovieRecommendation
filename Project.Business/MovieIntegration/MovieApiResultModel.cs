using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Business.MovieIntegration
{
    public class MovieApiResultModel<T>
    {
        public int page { get; set; }

        public int total_pages { get; set; }

        public int total_results { get; set; }

        public List<T> results { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.Dto
{
    public class RequestStateDto
    {
        public bool IsSuccess { get; set; }

        public object Data { get; set; }

        public string Description { get; set; }
    }
}

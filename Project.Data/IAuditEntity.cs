using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data
{
    public interface IAuditEntity
    {
        DateTime InsertDateTime { get; set; }

        DateTime? UpdateDateTime { get; set; }

        public bool IsDeleted { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Data.Domain.Logging
{
    public class Log : BaseEntity<long>
    {
        public int LogLevelId { get; set; }

        public string ShortMessage { get; set; }

        public string FullMessage { get; set; }

        public long? UserId { get; set; }

        public DateTime InsertDateTime { get; set; }

        [NotMapped]
        public virtual LogLevel LogLevel
        {
            get
            {
                return (LogLevel)this.LogLevelId;
            }
            set
            {
                this.LogLevelId = (int)value;
            }
        }
    }
}

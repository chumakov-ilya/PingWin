using System;
using System.Collections.Generic;

namespace PingWin.Entities.Models
{
    public partial class Job
    {
        public Job()
        {
            this.Logs = new List<Log>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Log> Logs { get; set; }
    }
}

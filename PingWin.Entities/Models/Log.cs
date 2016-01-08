using System;
using System.Collections.Generic;

namespace PingWin.Entities.Models
{
    public partial class Log
    {
        public int Id { get; set; }
        public int Result { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
        public int JobId { get; set; }
        public virtual Job Job { get; set; }
    }
}

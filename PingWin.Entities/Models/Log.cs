using System;
using System.Collections.Generic;

namespace PingWin.Entities.Models
{
    public partial class Log
    {
        public int Id { get; set; }
        public int Status { get; set; }
        public string ShortData { get; set; }
        public string FullData { get; set; }
        public string StackTrace { get; set; }
        public int JobRecordId { get; set; }
        public System.DateTime DateTime { get; set; }
        public virtual JobRecord JobRecord { get; set; }
    }
}

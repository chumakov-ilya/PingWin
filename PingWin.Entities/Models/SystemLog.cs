using System;
using System.Collections.Generic;

namespace PingWin.Entities.Models
{
    public partial class SystemLog
    {
        public int Id { get; set; }
        public System.DateTime DateTime { get; set; }
        public string Message { get; set; }
        public string FullData { get; set; }
        public string StackTrace { get; set; }
    }
}

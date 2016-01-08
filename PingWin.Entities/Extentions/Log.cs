using System.ComponentModel.DataAnnotations.Schema;

namespace PingWin.Entities.Models
{
	public partial class Log
	{
		[NotMapped]
		public StatusEnum StatusEnum
		{
			get { return (StatusEnum) Status; } 
			set { Status = (int) value; }
		}
	}
}
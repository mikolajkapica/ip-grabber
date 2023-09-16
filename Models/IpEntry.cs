using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ip_grabber.Models
{
	public class IpEntry
	{
		[Key]
		public int Id { get; set; }

		[Column(TypeName = "varchar(100)")]
		public string Link { get; set; }

		//[Column(TypeName = "varchar(100)")]
		public string? Data { get; set; }
	}
}

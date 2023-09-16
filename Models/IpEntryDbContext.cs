using Microsoft.EntityFrameworkCore;

namespace ip_grabber.Models
{
	public class IpEntryDbContext : DbContext
	{
		public IpEntryDbContext(DbContextOptions<IpEntryDbContext> options) : base(options)
		{}

		public DbSet<IpEntry> IpEntries { get; set; }
	}
}

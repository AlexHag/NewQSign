using Microsoft.EntityFrameworkCore;
using qsign.blob.Models;

namespace qsign.blob.Context;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    { }

    public DbSet<DocumentObject> DocumentObjects { get; set; } 
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
	{

    }
}
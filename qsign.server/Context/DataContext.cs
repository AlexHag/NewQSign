using Microsoft.EntityFrameworkCore;
using server.Models;

namespace server.Context;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    { }

    public DbSet<UserAccount> UsersAccounts { get; set; }

    public DbSet<User> Users { get; set; }
    public DbSet<DocumentInfo> DocumentInfos { get; set; }
    public DbSet<SignatureInfo> SignatureInfos { get; set; }
    public DbSet<CommunicationInfo> CommunicationInfos { get; set; } 
    public DbSet<PendingCommunication> PendingCommunications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<User>()
			.HasIndex(p => p.Email)
			.IsUnique(true);
    }
}
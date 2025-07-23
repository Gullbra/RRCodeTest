
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RRCodeTest.Server.Models;

namespace RRCodeTest.Server.DB;


public class AppDbContext : IdentityDbContext<User>
{
  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
  {
  }

  public DbSet<Book> Books { get; set; }

  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);

    builder.Entity<User>(entity =>
    {
      entity.Property(e => e.RefreshToken)
            .HasMaxLength(500);

      entity.HasIndex(e => e.Email)
            .IsUnique();
    });

    builder.Entity<Book>(entity =>
    {
      entity.HasKey(e => e.Id);

      entity.HasOne(b => b.User)
            .WithMany(u => u.Books)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    });
  }
}

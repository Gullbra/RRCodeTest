
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using api.Models;

namespace api.DB;

public class AppDbContext : IdentityDbContext<User>
{
  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
  {
  }

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
  }
}


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

      //entity.Property(e => e.DateOfPublication)
      //       .HasColumnType("TEXT");

      entity.HasOne(b => b.User)
            .WithMany(u => u.Books)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    });
  }
}



// Configure relationship with ApplicationUser
//entity.HasKey(e => e.Id);
//entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
//entity.Property(e => e.Author).IsRequired().HasMaxLength(100);
//entity.Property(e => e.ISBN).HasMaxLength(13);

//entity.HasOne(b => b.User).WithMany(u => u.Id);
//.HasForeignKey(b => b.Id)
//.OnDelete(DeleteBehavior.Cascade);


//entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
//entity.Property(e => e.Author).IsRequired().HasMaxLength(100);


//entity.HasOne(b => b.User)
//.WithMany(u => u.Books)
//.HasForeignKey(b => b.Id)
//.OnDelete(DeleteBehavior.SetNull);

//entity.Property(e => e.ISBN).HasMaxLength(13);


//entity.Property(e => e.Title)
//      .HasMaxLength(500);
//entity.Property(e => e.Author)
//      .HasMaxLength(500);
//entity.Property(e => e.Description)
//      .HasMaxLength(500);
//entity.Property(e => e.DateOfPublication)
//       .HasMaxLength(500);

//entity.HasIndex(e => e.Email)
//      .IsUnique();

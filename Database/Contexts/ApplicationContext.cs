using Database.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlTypes;
using System.Security.Cryptography;

namespace Database.Contexts
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        public DbSet<Serie> Series { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<GenreSerie> GenreSeries { get; set; }
        public DbSet<Producer> Producers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Tables
            modelBuilder.Entity<Serie>().ToTable("Series");
            modelBuilder.Entity<Genre>().ToTable("Generos");
            modelBuilder.Entity<GenreSerie>().ToTable("Genero_Series");
            modelBuilder.Entity<Producer>().ToTable("Productoras");
            #endregion

            #region Primary Keys
            modelBuilder.Entity<Serie>().HasKey(s => s.Id);
            modelBuilder.Entity<Genre>().HasKey(g => g.Id);
            modelBuilder.Entity<GenreSerie>().HasKey(sg => new { sg.SerieId, sg.GenreId }); 
            modelBuilder.Entity<Producer>().HasKey(p => p.Id);
            #endregion

            #region Relationships

            modelBuilder.Entity<Producer>()
                .HasMany<Serie>(p => p.Series)
                .WithOne(s => s.Producers)
                .HasForeignKey(s => s.ProducerId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<Serie>()
                .HasOne(s => s.Genre)
                .WithMany()
                .HasForeignKey(s => s.GenreId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<GenreSerie>()
                .HasOne(sg => sg.Series)
                .WithMany(s => s.SecondaryGenres)
                .HasForeignKey(sg => sg.SerieId);

            modelBuilder.Entity<GenreSerie>()
                .HasOne(sg => sg.Genre)
                .WithMany(g => g.SecondaryGenres)
                .HasForeignKey(sg => sg.GenreId);

            #endregion

            #region Property configurations

            #region Serie
            modelBuilder.Entity<Serie>().Property(s => s.Title).HasMaxLength(150);
            #endregion

            #region Genre
            modelBuilder.Entity<Genre>().Property(g => g.Name).HasMaxLength(150);
            #endregion

            #region Producers
            modelBuilder.Entity<Producer>().Property(p => p.Name).HasMaxLength(150);
            #endregion

            #endregion
        }
    }
}

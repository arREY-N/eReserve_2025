using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class Context: DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Designation>().HasData(
				new Designation { DesignationID = 1, DesignationName = "Administration" },
				new Designation { DesignationID = 2, DesignationName = "Faculty" },
				new Designation { DesignationID = 3, DesignationName = "Student" }
				);

			modelBuilder.Entity<Reservation>().HasOne(f => f.Facility).WithMany(rv => rv.Reservations).HasForeignKey(f => f.FacilityID);
            modelBuilder.Entity<Reservation>().HasOne(u => u.User).WithMany(rv => rv.Reservations).HasForeignKey(u => u.UserID);
            base.OnModelCreating(modelBuilder);
		}

		public DbSet<User> Users { get; set; }
        public DbSet<Facility> Facilities { get; set; }
		public DbSet<Reservation> Reservations { get; set; }
		public DbSet<Designation> Designations { get; set; }
	}
}

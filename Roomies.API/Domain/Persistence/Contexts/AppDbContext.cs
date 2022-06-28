using System;
using Microsoft.EntityFrameworkCore;
using Roomies.API.Domain.Models;
using Roomies.API.Extensions;
using Roomies.API.Publication.Domain.Models;

namespace Roomies.API.Domain.Persistence.Contexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<FavouritePost> FavouritePosts { get; set; }
        public DbSet<Landlord> Landlords { get; set; }
        public DbSet<Leaseholder> Leaseholders { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Domain.Models.Plan> Plans { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<ProfilePaymentMethod> UserPaymentMethods { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Rule> Rules { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Rule Entity
            modelBuilder.Entity<Rule>().ToTable("Rules");

            modelBuilder.Entity<Rule>().HasKey( r=> r.Id);
            modelBuilder.Entity<Rule>().Property(r => r.Id).IsRequired().ValueGeneratedOnAdd();


            //FavouritePost Entity Intermediate Table
            modelBuilder.Entity<FavouritePost>().ToTable("FavouritePosts");

            modelBuilder.Entity<FavouritePost>().HasKey
                (p => new { p.PostId, p.LeaseholderId });

            modelBuilder.Entity<FavouritePost>()
                 .HasOne(pt => pt.Post)
                 .WithMany(p => p.FavouritePosts)
                 .HasForeignKey(pt => pt.PostId);

            modelBuilder.Entity<FavouritePost>()
                .HasOne(pt => pt.Leaseholder)
                .WithMany(t => t.FavouritePosts)
                .HasForeignKey(pt => pt.LeaseholderId);


            //Profile Entity
            modelBuilder.Entity<Profile>().ToTable("Profiles");
                           
            modelBuilder.Entity<Profile>().HasKey(p => p.Id);
            modelBuilder.Entity<Profile>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            modelBuilder.Entity<Profile>().Property(p => p.Name).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Profile>().Property(p => p.LastName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Profile>().Property(p => p.CellPhone).IsRequired().HasMaxLength(9);
            modelBuilder.Entity<Profile>().Property(p => p.IdCard).IsRequired().HasMaxLength(8);
            modelBuilder.Entity<Profile>().Property(p => p.Description).IsRequired().HasMaxLength(240);
            modelBuilder.Entity<Profile>().Property(p => p.Birthday).IsRequired();
            modelBuilder.Entity<Profile>().Property(p => p.Department).IsRequired().HasMaxLength(25);
            modelBuilder.Entity<Profile>().Property(p => p.Province).IsRequired().HasMaxLength(25);
            modelBuilder.Entity<Profile>().Property(p => p.District).IsRequired().HasMaxLength(25);
            modelBuilder.Entity<Profile>().Property(p => p.Address).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Profile>().Property(p => p.StartSubscription).IsRequired();
            modelBuilder.Entity<Profile>().Property(p => p.EndSubsciption).IsRequired();

            //User Entity
            modelBuilder.Entity<User>().ToTable("Users");
                           
            modelBuilder.Entity<User>().HasKey(p => p.Id);
            modelBuilder.Entity<User>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            modelBuilder.Entity<User>().Property(p => p.FirstName).IsRequired();
            modelBuilder.Entity<User>().Property(p => p.LastName).IsRequired();
            modelBuilder.Entity<User>().Property(p => p.Username).IsRequired();
            modelBuilder.Entity<User>().Property(p => p.PasswordHash).IsRequired();

            // Relationships 
            modelBuilder.Entity<Profile>()
                .HasOne(owp => owp.User)
                .WithOne(u => u.Profile)
                .HasForeignKey<Profile>(owp => owp.UserId);


            //Landlord Entity
            modelBuilder.Entity<Landlord>().ToTable("Landlords");


            // Relationships 
            modelBuilder.Entity<Landlord>()
                .HasMany(p => p.Posts)
                .WithOne(p => p.Landlord) ///
                .HasForeignKey(p => p.LandlordId);


            //leaseholder Entity
            modelBuilder.Entity<Leaseholder>().ToTable("Leaseholders");

           
            modelBuilder.Entity<Leaseholder>()
                .HasMany(p => p.Reviews)
                .WithOne(p => p.Leaseholder)
                .HasForeignKey(p => p.LeaseholderId);


            // PaymentMethod Entity
            modelBuilder.Entity<PaymentMethod>().ToTable("PaymentMethods");

            modelBuilder.Entity<PaymentMethod>().HasKey(e => e.Id);
            modelBuilder.Entity<PaymentMethod>().Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
            modelBuilder.Entity<PaymentMethod>().Property(e => e.CVV).IsRequired().HasMaxLength(3);
            modelBuilder.Entity<PaymentMethod>().Property(e => e.ExpiryDate).IsRequired();
            //--------------------

            // Plan Entity
            modelBuilder.Entity<Domain.Models.Plan>().ToTable("Plans");

            // Constraints

            modelBuilder.Entity<Domain.Models.Plan>().HasKey(p => p.Id);
            modelBuilder.Entity<Domain.Models.Plan>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            modelBuilder.Entity<Domain.Models.Plan>().Property(p => p.Price).IsRequired();
            modelBuilder.Entity<Domain.Models.Plan>().Property(p => p.Name).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Domain.Models.Plan>().Property(p => p.Description).IsRequired().HasMaxLength(200);

            // Posts Entity

            modelBuilder.Entity<Post>().ToTable("Posts");

            modelBuilder.Entity<Post>().HasKey(p => p.Id);
            modelBuilder.Entity<Post>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            modelBuilder.Entity<Post>().Property(p => p.Title).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Post>().Property(p => p.Description).IsRequired().HasMaxLength(500);

            modelBuilder.Entity<Post>().Property(p => p.Address).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Post>().Property(p => p.Province).IsRequired().HasMaxLength(25);
            modelBuilder.Entity<Post>().Property(p => p.District).IsRequired().HasMaxLength(25);
            modelBuilder.Entity<Post>().Property(p => p.Department).IsRequired().HasMaxLength(25);
            modelBuilder.Entity<Post>().Property(p => p.Price).IsRequired();
            modelBuilder.Entity<Post>().Property(p => p.RoomQuantity).IsRequired();
            modelBuilder.Entity<Post>().Property(p => p.PostDate).IsRequired();
            

            //Relationships
            modelBuilder.Entity<Post>()
               .HasMany(p => p.Reviews)
               .WithOne(p => p.Post)
               .HasForeignKey(p => p.PostId);

            modelBuilder.Entity<Post>()
                .HasMany(p => p.Rules)
                .WithOne(r => r.Post)
                .HasForeignKey(r => r.PostId);
            //-------------------------------------

            // Review Entity

            modelBuilder.Entity<Review>().ToTable("Reviews");

            modelBuilder.Entity<Review>().HasKey(e => e.Id);
            modelBuilder.Entity<Review>().Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
            modelBuilder.Entity<Review>().Property(e => e.Content).IsRequired().HasMaxLength(300);
            modelBuilder.Entity<Review>().Property(e => e.Date).IsRequired();

            //UserPaymentMethod Entity Intermediate Table
            modelBuilder.Entity<ProfilePaymentMethod>().ToTable("ProfilePaymentMethods");

            modelBuilder.Entity<ProfilePaymentMethod>().HasKey(p => new { p.ProfileId, p.PaymentMethodId });

            modelBuilder.Entity<ProfilePaymentMethod>()
                 .HasOne(pt => pt.Profile)
                 .WithMany(p => p.ProfilePaymentMethods)
                 .HasForeignKey(pt => pt.ProfileId);

            modelBuilder.Entity<ProfilePaymentMethod>()
                .HasOne(pt => pt.PaymentMethod)
                .WithMany(t => t.UserPaymentMethods)
                .HasForeignKey(pt => pt.PaymentMethodId);

            modelBuilder.ApplySnakeCaseNamingConvention();
        }
    }
}

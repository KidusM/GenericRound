using CSMS.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSMS.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public ApplicationDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ScPricingStores>()
                .HasKey(nameof(ScPricingStores.Syscode), nameof(ScPricingStores.StoresId));

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ItemStorePrice>()
                .HasKey(nameof(ItemStorePrice.ItemCode), nameof(ItemStorePrice.Syscode), nameof(ItemStorePrice.StoreId));

            modelBuilder.Entity<ScPricingData>(entity =>
            {
                entity.Property(e => e.ItemCode).IsFixedLength();

                entity.Property(e => e.SurveyMonth).IsFixedLength();

                entity.Property(e => e.SurveyYear).IsFixedLength();
                

                entity.Property(e => e.Syscode)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Unit).IsFixedLength();

                entity.HasOne(d => d.ItemCodeNavigation)
                    .WithMany(p => p.ScPricingData)
                    .HasForeignKey(d => d.ItemCode)
                    .HasConstraintName("FK_SC_Pricing_Data_SC_Pricing_Item");
            });

            modelBuilder.Entity<ScPricingItem>(entity =>
            {
                entity.Property(e => e.ItemCode).IsFixedLength();
            });

            modelBuilder.Entity<ScStores>(entity =>
            {
                //entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.DsId)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.GpsCoordinates).IsUnicode(false);

                entity.Property(e => e.StoreAddress).IsUnicode(false);

                entity.Property(e => e.StoresDescription).IsUnicode(false);

                entity.Property(e => e.StoresId)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.StoresName).IsUnicode(false);

                entity.Property(e => e.WebAddress).IsUnicode(false);
            });

            modelBuilder.Entity<SurveyStore>(entity =>
            {
                entity.HasKey(e => new { e.PricingQid, e.StoreId });

                entity.HasOne(d => d.PricingQ)
                    .WithMany(p => p.SurveyStore)
                    .HasForeignKey(d => d.PricingQid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Survey_Store_SC_Pricing_Data");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.SurveyStore)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Survey_Store_SC_Stores");
            });

            modelBuilder.Entity<ScReport>(entity =>
            {
                entity.Property(e => e.Syscode)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Id).ValueGeneratedOnAdd()
                     .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
                //.Metadata.GetAfterSaveBehavior = PropertySaveBehavior.Throw;
            });
            modelBuilder.Entity<ScMontoringReport>(entity =>
            {
                entity.HasNoKey();
                //entity.HasKey(x => x.org);
            });


        }
        public DbSet<ApplicationUser> applicationUsers { get; set; }

        public virtual DbSet<DutyStation> DutyStations { get; set; }
        public virtual DbSet<Faq> Faqs { get; set; }
        public virtual DbSet<ScreportQuestion> ScreportQuestions { get; set; }
        public virtual DbSet<Survey> Surveys { get; set; }
        //public virtual DbSet<VwIdmsSurveys> VwIdmsSurveys { get; set; }
        public DbSet<ScChecklist> ScChecklist { get; set; }
        public virtual DbSet<ScPricingStores> ScpricingStoress { get; set; }
        //public DbSet<CSMS.Models.IccdvwTmSchHd> IccdvwTmSchHd { get; set; }
        public virtual DbSet<ScPricingItem> ScPricingItems { get; set; }
        public virtual DbSet<ItemStorePrice> ItemStorePrices { get; set; }
        public DbSet<ScPricingQuestionnaire> ScPricingQuestionnaire { get; set; }
        public DbSet<ScPricingQuestions> ScPricingQuestions { get; set; }
        public DbSet<ScPricingData> scPricingDatas{ get; set; }
        public DbSet<SurveyStore> surveyStores { get; set; }
        public DbSet<ScStores> ScStores { get; set; }
        public DbSet<CSMS.Models.ScReport> ScReport { get; set; }
        public DbSet<CSMS.Models.listOriginal> listOriginals { get; set; }
        public virtual DbSet<ScMontoringReport> scMontoringReport { get; set; }


    }
}

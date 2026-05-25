using CSMS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSMS.Data
{
    public class IDMSDbContext : IdentityDbContext
    {
        public IDMSDbContext(DbContextOptions<IDMSDbContext> options)
            : base(options)
        {
        }

        public IDMSDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<IccdvwTmSchHd>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ICCDVW_TM_SCH_HD");

                entity.Property(e => e.CountryId)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CountryName).IsUnicode(false);

                entity.Property(e => e.CurrencyId)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.DsId)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.DsName).IsUnicode(false);

                entity.Property(e => e.FsPositions).IsUnicode(false);

                entity.Property(e => e.SurveyType)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Syscode)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<IcadmCountry>(entity =>
            {
                entity.HasKey(e => e.CountryId)
                    .HasName("PK_ICAD_ICADM_COUNTRY");

                entity.HasIndex(e => e.CountryName)
                    .HasName("UN_ICAD_ICADM_COUNTRY")
                    .IsUnique();

                entity.Property(e => e.CountryId)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CntValid)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CountryName).IsUnicode(false);

                entity.Property(e => e.RegionId)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.IcadmCountry)
                    .HasForeignKey(d => d.RegionId)
                    .HasConstraintName("FK__ICADM_COU__REGIO__339FAB6E");
            });

            modelBuilder.Entity<IcadmDutystations>(entity =>
            {
                entity.HasKey(e => e.DsId)
                    .HasName("PK_ICAD_ICADM_DUTYSTATIONS");

                entity.Property(e => e.DsId)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CountryId)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.DsName).IsUnicode(false);

                entity.Property(e => e.DsNo)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.RegionId)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.IcadmDutystations)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("FK__ICADM_DUT__COUNT__3A4CA8FD");

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.IcadmDutystations)
                    .HasForeignKey(d => d.RegionId)
                    .HasConstraintName("FK_ICADM_DS_ICADM_REGION2_3A4CA8FD");
            });

            modelBuilder.Entity<IcadmRegion>(entity =>
            {
                entity.HasKey(e => e.RegionId)
                    .HasName("PK_ICAD_ICADM_REGION");

                entity.Property(e => e.RegionId)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.RegName).IsUnicode(false);
            });

            modelBuilder.Entity<IccdmDutystations>(entity =>
            {
                entity.HasIndex(e => e.DsName)
                    .HasName("UN_ICCDM_DUTYSTATIONS")
                    .IsUnique();

                entity.Property(e => e.DsId)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('Y')");

                entity.Property(e => e.BaseDs)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.BasePlace).IsUnicode(false);

                entity.Property(e => e.CountryId)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CpiDs)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.DsGroup)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.DsName).IsUnicode(false);

                entity.Property(e => e.HqFlag)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.InternalDs)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.LinkDs)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.IccdmDutystations)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("FK__ICCDM_DUT__COUNT__38B96646");
            });


            modelBuilder.Entity<IccdtTmSchHd>(entity =>
            {
                entity.HasIndex(e => new { e.DsId, e.SurveyType, e.ProposedYear, e.ProposedMonth, e.ActualYear, e.ActualMonth })
                    .HasName("UN_ICCDT_TM_SCH_HD")
                    .IsUnique();

                entity.Property(e => e.Syscode)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Cancelled)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.DataEntryFlag)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.DsId)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Followup)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.FollowupComments).IsUnicode(false);

                entity.Property(e => e.FsPositions)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.LeadAgency).IsUnicode(false);

                entity.Property(e => e.SurveyType)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.UpdationFlag)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.VerifyFlag)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('N')");

                entity.HasOne(d => d.Ds)
                    .WithMany(p => p.IccdtTmSchHd)
                    .HasForeignKey(d => d.DsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ICCDT_TM___DS_ID__689D8392");
            });



                //     modelBuilder.Entity<IccdvwTmSchHd>(eb => eb.HasNoKey());
                //    //eb.ToView("PassInfoToMicrosite");
                //    //eb.Property(v => v.DateWebStart).HasColumnName("Start Date");
                //    //eb.Property(v => v.DateWebEnd).HasColumnName("End Date");

                ////});
                modelBuilder.Ignore<IdentityUserLogin<string>>();
            modelBuilder.Ignore<IdentityUserRole<string>>();
            modelBuilder.Ignore<IdentityUserClaim<string>>();
            modelBuilder.Ignore<IdentityUserToken<string>>();
            modelBuilder.Ignore<IdentityUser<string>>();
            modelBuilder.Ignore<ApplicationUser>();

            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<IccdvwTmSchHd>()
            //    .HasKey(nameof(ScpricingStores.ActualMonth), nameof(ScpricingStores.ActualYear), nameof(ScpricingStores.DsId), nameof(ScpricingStores.StoresId));
        }

        public virtual DbSet<IccdvwTmSchHd> IccdvwTmSchHds { get; set; }
        public virtual DbSet<IccdtTmSchHd> IccdtTmSchHds { get; set; }
        public virtual DbSet<IcadmCountry> IcadmCountries { get; set; }
        public virtual DbSet<IcadmDutystations> IcadmDutystations { get; set; }
        public virtual DbSet<IcadmRegion> IcadmRegions { get; set; }
       

         
    }
}

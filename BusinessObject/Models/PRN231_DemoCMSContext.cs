using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BusinessObject.Models
{
    public partial class PRN231_DemoCMSContext : DbContext
    {
        public PRN231_DemoCMSContext()
        {
        }

        public PRN231_DemoCMSContext(DbContextOptions<PRN231_DemoCMSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<CourseRegistration> CourseRegistrations { get; set; } = null!;
        public virtual DbSet<File> Files { get; set; } = null!;
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
        public virtual DbSet<Section> Sections { get; set; } = null!;
        public virtual DbSet<Upload> Uploads { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server =(local); database = PRN231_DemoCMS;uid=ndt;pwd=16102001;TrustServerCertificate=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>(entity =>
            {
                entity.Property(e => e.CourseName).HasMaxLength(100);

                entity.Property(e => e.CreatedTime).HasColumnType("date");

                entity.Property(e => e.Image).IsUnicode(false);

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.CreatorId)
                    .HasConstraintName("FK_Courses_Users");
            });

            modelBuilder.Entity<CourseRegistration>(entity =>
            {
                entity.HasKey(e => e.RegistrationId)
                    .HasName("PK__CourseRe__6EF588101D8C6EC3");

                entity.Property(e => e.EditedCourseName).HasMaxLength(100);

                entity.Property(e => e.RegistedTime).HasColumnType("datetime");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.CourseRegistrations)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK__CourseReg__Cours__3A81B327");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CourseRegistrations)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__CourseReg__UserI__3B75D760");
            });

            modelBuilder.Entity<File>(entity =>
            {
                entity.Property(e => e.FileName).HasMaxLength(100);

                entity.Property(e => e.FileType).HasMaxLength(50);

                entity.Property(e => e.FileUrl).IsUnicode(false);
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.ExpirationTime).HasColumnType("datetime");

                entity.Property(e => e.Token).IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithOne(p => p.RefreshToken)
                    .HasForeignKey<RefreshToken>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__RefreshTo__UserI__45F365D3");
            });

            modelBuilder.Entity<Section>(entity =>
            {
                entity.Property(e => e.SectionName).HasMaxLength(100);

                entity.HasOne(d => d.Registration)
                    .WithMany(p => p.Sections)
                    .HasForeignKey(d => d.RegistrationId)
                    .HasConstraintName("FK__Sections__Regist__3E52440B");
            });

            modelBuilder.Entity<Upload>(entity =>
            {
                entity.Property(e => e.UploadName).HasMaxLength(100);

                entity.Property(e => e.UploadTime).HasColumnType("datetime");

                entity.HasOne(d => d.File)
                    .WithMany(p => p.Uploads)
                    .HasForeignKey(d => d.FileId)
                    .HasConstraintName("FK__FileUploa__FileI__440B1D61");

                entity.HasOne(d => d.Section)
                    .WithMany(p => p.Uploads)
                    .HasForeignKey(d => d.SectionId)
                    .HasConstraintName("FK__FileUploa__Secti__4316F928");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.FullName).HasMaxLength(100);

                entity.Property(e => e.Password).HasMaxLength(100);

                entity.Property(e => e.Role).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace QLTV2.Models;

public partial class LibraryDbContext : DbContext
{
    public LibraryDbContext()
    {
    }

    public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TbAccount> TbAccounts { get; set; }

    public virtual DbSet<TbBook> TbBooks { get; set; }

    public virtual DbSet<TbBookReview> TbBookReviews { get; set; }

    public virtual DbSet<TbBorrow> TbBorrows { get; set; }

    public virtual DbSet<TbBorrowDetail> TbBorrowDetails { get; set; }

    public virtual DbSet<TbCategory> TbCategories { get; set; }

    public virtual DbSet<TbContact> TbContacts { get; set; }

    public virtual DbSet<TbRole> TbRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("data source= ADMIN-PC\\SQLEXPRESS; initial catalog=LibraryDB; integrated security=True; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TbAccount>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__tb_Accou__349DA5A6A22B533D");

            entity.ToTable("tb_Account");

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastLogin).HasColumnType("datetime");
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Username).HasMaxLength(50);

            entity.HasOne(d => d.Role).WithMany(p => p.TbAccounts)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__tb_Accoun__RoleI__398D8EEE");
        });

        modelBuilder.Entity<TbBook>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("PK__tb_Book__3DE0C20745A915AB");

            entity.ToTable("tb_Book");

            entity.Property(e => e.Author).HasMaxLength(100);
            entity.Property(e => e.CreatedBy).HasMaxLength(100);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Image).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ModifiedBy).HasMaxLength(100);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(250);

            entity.HasOne(d => d.Category).WithMany(p => p.TbBooks)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__tb_Book__Categor__403A8C7D");
        });

        modelBuilder.Entity<TbBookReview>(entity =>
        {
            entity.HasKey(e => e.BookReviewId).HasName("PK__tb_BookR__02F5EA55849B83FD");

            entity.ToTable("tb_BookReview");

            entity.Property(e => e.Comment).HasMaxLength(1000);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.TbBookReviews)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__tb_BookRe__Accou__4D94879B");

            entity.HasOne(d => d.Book).WithMany(p => p.TbBookReviews)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("FK__tb_BookRe__BookI__4E88ABD4");
        });

        modelBuilder.Entity<TbBorrow>(entity =>
        {
            entity.HasKey(e => e.BorrowId).HasName("PK__tb_Borro__4295F83F350BB2DB");

            entity.ToTable("tb_Borrow");

            entity.Property(e => e.BorrowDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ReturnDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Đang mượn");

            entity.HasOne(d => d.Account).WithMany(p => p.TbBorrows)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__tb_Borrow__Accou__440B1D61");
        });

        modelBuilder.Entity<TbBorrowDetail>(entity =>
        {
            entity.HasKey(e => e.BorrowDetailId).HasName("PK__tb_Borro__2D670146FF65A7DB");

            entity.ToTable("tb_BorrowDetail");

            entity.HasOne(d => d.Book).WithMany(p => p.TbBorrowDetails)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("FK__tb_Borrow__BookI__4AB81AF0");

            entity.HasOne(d => d.Borrow).WithMany(p => p.TbBorrowDetails)
                .HasForeignKey(d => d.BorrowId)
                .HasConstraintName("FK__tb_Borrow__Borro__49C3F6B7");
        });

        modelBuilder.Entity<TbCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__tb_Categ__19093A0BE2FFA106");

            entity.ToTable("tb_Category");

            entity.Property(e => e.Alias).HasMaxLength(150);
            entity.Property(e => e.CreatedBy).HasMaxLength(100);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ModifiedBy).HasMaxLength(100);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(150);
        });

        modelBuilder.Entity<TbContact>(entity =>
        {
            entity.HasKey(e => e.ContactId).HasName("PK__tb_Conta__5C66259B39D868CD");

            entity.ToTable("tb_Contact");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.IsRead).HasDefaultValue(false);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(20);
        });

        modelBuilder.Entity<TbRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__tb_Role__8AFACE1A8A128E96");

            entity.ToTable("tb_Role");

            entity.Property(e => e.Description).HasMaxLength(250);
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

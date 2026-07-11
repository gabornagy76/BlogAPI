using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Models;

public partial class BlogContext : DbContext
{
    public BlogContext()
    {
    }

    public BlogContext(DbContextOptions<BlogContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Blogger> Bloggers { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("server=localhost;database=blog;user=root;password=");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blogger>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("blogger");

            entity.HasIndex(e => e.Email, "email").IsUnique();

            entity.HasIndex(e => e.UserName, "userName").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("updatedAt");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .HasColumnName("userName");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("post");

            entity.HasIndex(e => e.BloggerId, "fk_post_blogger");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BloggerId).HasColumnName("bloggerID");
            entity.Property(e => e.Content)
                .HasColumnType("text")
                .HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("createdAT");
            entity.Property(e => e.Title)
                .HasMaxLength(150)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("updatedAT");

            entity.HasOne(d => d.Blogger).WithMany(p => p.Posts)
                .HasForeignKey(d => d.BloggerId)
                .HasConstraintName("fk_post_blogger");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

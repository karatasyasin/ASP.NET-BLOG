namespace Blog.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class blogDB : DbContext
    {
        public blogDB()
            : base("name=blogDB")
        {
        }

        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<Auth> Auths { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>()
                .HasMany(e => e.Tags)
                .WithMany(e => e.Articles)
                .Map(m => m.ToTable("ArticleTag").MapLeftKey("ArticleId").MapRightKey("TagId"));

            modelBuilder.Entity<Comment>()
                .Property(e => e.Date)
                .IsFixedLength();
        }
    }
}

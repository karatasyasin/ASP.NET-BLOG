namespace Blog.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Article")]
    public partial class Article
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Article()
        {
            Comments = new HashSet<Comment>();
            Tags = new HashSet<Tag>();
        }

        public int ArticleId { get; set; }

        [StringLength(100)]
        public string Title { get; set; }

        public string Content { get; set; }

        [StringLength(100)]
        public string Photo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }

        public int? Displayed { get; set; }

        public int? CategoryId { get; set; }

        public int? UserId { get; set; }

        public virtual Category Category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tag> Tags { get; set; }
    }
}

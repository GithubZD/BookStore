namespace BookStore.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Cart
    {
        [Key]
        public int RecordId { get; set; }

        [Required]
        [StringLength(100)]
        public string CartId { get; set; }

        public int BookId { get; set; }

        public int Count { get; set; }

        public DateTime DeteCreated { get; set; }

        public virtual Book Book { get; set; }
    }
}

namespace BookStore.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OrderDetail
    {
        public int OrderDetailId { get; set; }

        public int OrderId { get; set; }

        [Display(Name = "图片")]
        public int BookId { get; set; }

        [Display(Name = "数量")]
        public int Quantity { get; set; }

        [Display(Name = "总价")]
        public decimal UnitPrice { get; set; }

        public virtual Book Book { get; set; }

        public virtual Order Order { get; set; }
    }
}

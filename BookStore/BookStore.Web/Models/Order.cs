namespace BookStore.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int OrderId { get; set; }

        [Display(Name = "订单日期")]
        public DateTime OrderDate { get; set; }

        //[Required]
        [StringLength(50)]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "全名")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "详细地址")]
        public string Address { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "城市")]
        public string City { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "省")]
        public string Province { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "电话")]
        [Phone]
        public string Phone { get; set; }

        [Display(Name = "总价")]
        public decimal Total { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}

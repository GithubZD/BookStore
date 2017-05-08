namespace CarsRent.Models
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

        [Required]
        [Display(Name = "订单时间")]
        public DateTime OrderTime { get; set; }

        [Display(Name = "取消时间")]
        public DateTime? CancelTime { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [Display(Name = "付款情况")]
        public int PayYesNo { get; set; }

        [Required]
        [Display(Name = "车辆状态")]
        public int Status { get; set; }

        [Required]
        public int UserManager { get; set; }

        [Required]
        public int AdminManager { get; set; }

        [Display(Name = "订单操作")]
        public int Evaluate { get; set; }

        [StringLength(400)]
        [Display(Name = "备注")]
        public string Remark { get; set; }

        public virtual User User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}

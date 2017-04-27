namespace CarsRent.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CanaelOrder")]
    public partial class CanaelOrder
    {
        [Key]
        public int CancelOrderId { get; set; }

        public int OrderId { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "取消时间")]
        public DateTime CancelTime { get; set; }

        [Display(Name = "赔偿金")]
        public decimal? Compensation { get; set; }

        public int UserManager { get; set; }

        public int AdminManager { get; set; }
    }
}

namespace CarsRent.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Car
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Car()
        {
            OrderDetails = new HashSet<OrderDetail>();
            Evaluates = new HashSet<Evaluate>();
        }

        public int CarId { get; set; }

        public int CategroyId { get; set; }

        public int BrandId { get; set; }

        public int SeatNumId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "车名")]
        public string CarName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "车牌号")]
        public string PlateNumber { get; set; }

        [Display(Name = "租金")]
        public decimal RentPrice { get; set; }

        [Display(Name = "总数量")]
        public int Number { get; set; }

        [Display(Name = "现有数量")]
        public int NowNumber { get; set; }

        [StringLength(100)]
        [Display(Name = "图片")]
        public string ImageUrl { get; set; }

        [Required]
        [StringLength(400)]
        [Display(Name = "详细介绍")]
        public string Details { get; set; }

        public virtual Brand Brand { get; set; }

        public virtual Categroy Categroy { get; set; }

        public virtual SeatNum SeatNum { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Evaluate> Evaluates { get; set; }
    }
}

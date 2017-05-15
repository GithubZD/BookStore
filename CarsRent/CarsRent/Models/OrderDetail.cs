namespace CarsRent.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OrderDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OrderDetail()
        {
            Evaluates = new HashSet<Evaluate>();
        }

        [Key]
        public int OrderDetailsId { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int CarId { get; set; }

        [Required]
        [Column(TypeName = "date")]
        [Display(Name = "用车时间")]
        public DateTime AwayTime { get; set; }

        [Display(Name = "归还时间")]
        public DateTime? ReturnTime { get; set; }

        [Required]
        [Display(Name = "租用天数")]
        public int? Days { get; set; }

        [Required]
        [Display(Name = "押金")]
        public decimal? Deposit { get; set; }

        [Required]
        [Display(Name = "总租金")]
        public decimal? Money { get; set; }

        [Required]
        [Display(Name = "租用辆数")]
        public int? Number { get; set; }
        [Display(Name = "赔偿金")]
        public decimal? Compensation { get; set; }
        public virtual Car Car { get; set; }

        public virtual Order Order { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Evaluate> Evaluates { get; set; }
    }
}

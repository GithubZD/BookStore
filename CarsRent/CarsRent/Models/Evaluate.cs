namespace CarsRent.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    public partial class Evaluate
    {

        [Key]
        public int EvaluateId { get; set; }
        public int OrderDetailsId { get; set; }
        public int CarId { get; set; }
        public int UserId { get; set; }
        
        [Display(Name = "评价")]
        public string EvaluateContent { get; set; }

        public virtual OrderDetail OrderDetail { get; set; }
        public virtual Car Car { get; set; }
        public virtual User User { get; set; }
    }
}
namespace CarsRent.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Orders= new HashSet<Order>();
            Evaluates = new HashSet<Evaluate>();
        }

        public int UserId { get; set; }

        [StringLength(100)]
        public string Icon { get; set; }

        [StringLength(20)]
        [Display(Name = "登录名")]
        public string LoginName { get; set; }

        [StringLength(20)]
        [Display(Name = "真实姓名")]
        public string RealName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [StringLength(18, ErrorMessage = "身份证号位数不对")]
        [Display(Name = "身份证号")]
        public string CardNumber { get; set; }

        [Phone]
        [StringLength(11, ErrorMessage = "电话号码位数不对")]
        [Display(Name = "电话号码")]
        public string Iphone { get; set; }

        [Display(Name = "年龄")]
        public int? Age { get; set; }

        [StringLength(10)]
        [Display(Name = "性别")]
        public string Sex { get; set; }

        [StringLength(200)]
        [Display(Name = "家庭住址")]
        public string Address { get; set; }

        public int RoleId { get; set; }

        public virtual Role Role{ get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders{ get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Evaluate> Evaluates { get; set; }
    }
}

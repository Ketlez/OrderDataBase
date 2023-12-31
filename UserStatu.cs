namespace OrderDataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserStatu
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        [StringLength(60)]
        public string UserName { get; set; }

        [Required]
        [StringLength(60)]
        public string Password { get; set; }

        public virtual Client Client { get; set; }

        public virtual Employee Employee { get; set; }
    }
}

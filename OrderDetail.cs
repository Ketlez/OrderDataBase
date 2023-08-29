namespace OrderDataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OrderDetail
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OrderID { get; set; }

        public int? Count { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductID { get; set; }

        public int? UserID { get; set; }

        [StringLength(20)]
        public string Total { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual OrderPC OrderPC { get; set; }

        public virtual Product Product { get; set; }
    }
}

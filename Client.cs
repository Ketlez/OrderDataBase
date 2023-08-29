namespace OrderDataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Client")]
    public partial class Client
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Client()
        {
            OrderPCs = new HashSet<OrderPC>();
        }

        [Required]
        [StringLength(200)]
        public string ClientName { get; set; }

        [Required]
        [StringLength(200)]
        public string Address { get; set; }

        [StringLength(200)]
        public string Phone { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderPC> OrderPCs { get; set; }

        public virtual UserStatu UserStatu { get; set; }

        public override string ToString()
        {
            return ClientName;
        }
    }
}

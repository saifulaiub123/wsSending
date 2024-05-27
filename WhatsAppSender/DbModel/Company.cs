using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WhatsAppSender.DbModel
{
    [Table("Company")]
    public class Company
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool Active { get; set; }

        // Navigation property for related Queue entities
        public virtual ICollection<Queue> Queues { get; set; }
    }
}

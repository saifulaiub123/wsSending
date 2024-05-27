using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;


namespace WhatsAppSender.DbModel
{
    [Table("Settings")]
    public class Settings
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Key { get; set; }

        [Required]
        [MaxLength(250)]
        public string Value { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool Active { get; set; }

        [ForeignKey("Company")]
        public int CompanyId { get; set; }

        // Navigation property
        public virtual Company Company { get; set; }
    }
}

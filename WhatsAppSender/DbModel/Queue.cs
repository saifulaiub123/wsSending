using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhatsAppSender.DbModel
{
    [Table("Queue")]
    public class Queue
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int TaskId { get; set; }

        [ForeignKey("Company")]
        public int CompanyId { get; set; }

        public DateTime RequestedDate { get; set; }

        [MaxLength(250)]
        public string RequestingUser { get; set; }

        [Required]
        [MaxLength(50)]
        public string PhoneNumber { get; set; }

        public string Message { get; set; }

        public string AttachmentPath { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool Sent { get; set; }

        public DateTime? SentDate { get; set; }

        public string Result { get; set; }

        // Navigation property
        public virtual Company Company { get; set; }
    }
}

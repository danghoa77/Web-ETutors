using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eTutoring.Models
{
    public class Room
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public required string Name { get; set; }
        
        [Required]
        public required string Description { get; set; }
        
        [Required]
        public required string MeetingLink { get; set; }
        
        public DateTime TimeSlot { get; set; }
        
        [Required]
        public required string TutorId { get; set; }
        
        [ForeignKey("TutorId")]
        public virtual required Tutor Tutor { get; set; }

        public virtual ICollection<Document> Documents { get; set; } = new List<Document>();
        public virtual ICollection<MeetingRecord> MeetingRecords { get; set; } = new List<MeetingRecord>();
    }
}
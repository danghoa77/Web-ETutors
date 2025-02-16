using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eTutoring.Models
{
    public class TutorRequest
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public required string StudentId { get; set; }
        
        [ForeignKey("StudentId")]
        public virtual required Student Student { get; set; }
        
        [Required]
        public required string Subject { get; set; }
        
        [Required]
        public required string TutoringType { get; set; }
        
        [Required]
        public required string Status { get; set; } // e.g., Pending, Approved, Rejected
        
        public DateTime CreatedAt { get; set; }
    }
}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace eTutoring.Models
{
    public class Tutor
    {
        [Key]
        [ForeignKey("User")]
        public required string Id { get; set; }
        
        [Required]
        public required string FullName { get; set; }
        
        [Required]
        public required string Education { get; set; }
        
        [Required]
        public required string Expertise { get; set; }
        
        public int TeachingExperience { get; set; }
        
        [Required]
        public required string TutoringType { get; set; }
        
        public virtual required IdentityUser User { get; set; }
        
        // A tutor may be linked to multiple Rooms.
        public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
    }
}
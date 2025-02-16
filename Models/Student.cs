using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace eTutoring.Models
{
    public class Student
    {
        [Key]
        [ForeignKey("User")]
        public required string Id { get; set; }
        
        [Required]
        public required string FullName { get; set; }
        
        [Required]
        public required string Education { get; set; }
        
        [Required]
        public required string TutoringType { get; set; }
        
        // Optional relationship if a Student is assigned to a Room.
        public int? RoomId { get; set; }
        public virtual Room? Room { get; set; }
        
        public virtual required IdentityUser User { get; set; }
    }
}
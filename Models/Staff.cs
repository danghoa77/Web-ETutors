using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace eTutoring.Models
{
    public class Staff
    {
        [Key]
        [ForeignKey("User")]
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string Position { get; set; }
        
        public virtual required IdentityUser User { get; set; }
    }
}
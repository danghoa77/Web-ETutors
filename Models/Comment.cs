using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace eTutoring.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public required string Content { get; set; }
        
        public DateTime Date { get; set; }
        
        public int PostId { get; set; }
        
        [ForeignKey("PostId")]
        public virtual required Post Post { get; set; }
        
        [Required]
        public required string AuthorId { get; set; }
        
        [ForeignKey("AuthorId")]
        public virtual required IdentityUser Author { get; set; }
    }
}
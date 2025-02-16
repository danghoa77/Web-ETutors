using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace eTutoring.Models
{
    public class Blog
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public required string Title { get; set; }
        
        [Required]
        public required string Content { get; set; }
        
        public DateTime PostedDate { get; set; }
        
        [Required]
        public required string AuthorId { get; set; }
        
        [ForeignKey("AuthorId")]
        public virtual required IdentityUser Author { get; set; }
        
        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}
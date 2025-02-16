using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eTutoring.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public required string Content { get; set; }
        
        public DateTime Date { get; set; }
        
        public int BlogId { get; set; }
        
        [ForeignKey("BlogId")]
        public virtual required Blog Blog { get; set; }
        
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
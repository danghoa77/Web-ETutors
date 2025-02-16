using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace eTutoring.Models
{
    public class Document
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public required string FileName { get; set; }
        
        [Required]
        public required string FilePath { get; set; }
        
        public DateTime UploadedDate { get; set; }
        
        [Required]
        public required string UploadedBy { get; set; }
        
        [ForeignKey("UploadedBy")]
        public virtual required IdentityUser UploadedByUser { get; set; }
        
        public int RoomId { get; set; }
        
        [ForeignKey("RoomId")]
        public virtual required Room Room { get; set; }
    }
}
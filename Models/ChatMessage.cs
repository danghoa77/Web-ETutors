using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace eTutoring.Models
{
    public class ChatMessage
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public required string Content { get; set; }
        
        public DateTime SentDate { get; set; }
        
        [Required]
        public required string SenderId { get; set; }
        
        [ForeignKey("SenderId")]
        public virtual required IdentityUser Sender { get; set; }
        
        [Required]
        public required string ReceiverId { get; set; }
        
        [ForeignKey("ReceiverId")]
        public virtual required IdentityUser Receiver { get; set; }
    }
}
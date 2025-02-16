using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eTutoring.Models
{
    public class MeetingRecord
    {
        [Key]
        public int Id { get; set; }
        
        public int RoomId { get; set; }
        
        [ForeignKey("RoomId")]
        public virtual required Room Room { get; set; }
        
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        
        [Required]
        public required string RecordingLink { get; set; }
        
        [Required]
        public required string GoogleDriveFileId { get; set; }
        
        [Required]
        public required string Notes { get; set; }
    }
}
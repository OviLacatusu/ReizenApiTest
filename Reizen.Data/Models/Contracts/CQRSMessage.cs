using ReizenApi.Contracts;
using System.ComponentModel.DataAnnotations;

namespace Reizen.Data.Contracts
{
    public sealed class CQRSMessage : ICQRSCommandContract
    {
        [Required]
        public string Value { get; init; }

        [Key]
        public Guid Id
        {
            get; set;
        }
        [Required]
        public string Type { get; init; }

        [Required]
        public MessageStatus Status
        {
            get;set;
        }

        [Required]
        public DateTime CreatedAt
        {
            get; init; 
        }
        public DateTime? ProcessedAt
        {
            get; set;
        }

        public enum MessageStatus
        {
            Pending, Processing, Completed, Failed
        }
    }
}
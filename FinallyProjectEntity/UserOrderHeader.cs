using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinallyProjectEntity
{
    public class UserOrderHeader : BaseModel
    {
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser ApplicationUser { get; set; }
        [Required]
        public DateTime DateOfOrder { get; set; }
        [Required]
        public DateTime DateOfShipped { get; set; }
        [Required]
        public double TotalOrderAmount { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Carrier { get; set; }
        public string? OrderStatus { get; set; }
        public string? PaymentStatus { get; set; }
        public DateTime? PaymentProccessDate { get; set; }
        public string? TransactionId { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string DeliveryStreetAddress { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        public string? PostalCode { get; set; }
        [Required]
        public string Name { get; set; }
    }
}

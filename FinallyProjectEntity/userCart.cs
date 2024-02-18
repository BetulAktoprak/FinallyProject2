using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinallyProjectEntity
{
    public class userCart : BaseModel
    {
        public int? ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product product { get; set; }
        public string userId { get; set; }
        [ForeignKey("userId")]
        public ApplicationUser ApplicationUser { get; set; }
        [Range(1, 100, ErrorMessage = "1'den 100'e kadar")]
        public int Quantity { get; set; }
        [NotMapped]
        public double Price { get; set; }
    }
}

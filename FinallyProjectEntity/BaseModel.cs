using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinallyProjectEntity
{
    public class BaseModel
    {
        [Required]
        public int Id { get; set; }
        public DateTime? AddDate { get; set; }
        public DateTime? UpdateDate { get; set;}
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinallyProjectEntity
{
    public class Category : BaseModel
    {
        [Required]
        [StringLength(30, ErrorMessage = "En fazla 30 karakter giriniz.")]
        public string Name { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinallyProjectEntity.ViewModel
{
    public class HomePageVM
    {
        public IEnumerable<Product> ProductList { get; set; }
        public IEnumerable<Category> categories { get; set;}
    }
}

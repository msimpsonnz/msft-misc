using OouiForms.Helpers;
using OouiForms.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace OouiForms.ViewModels
{
    public class ShopViewModel
    {
        public ObservableCollection<Product> Products { get; set; }

        public ShopViewModel()
        {
            Products = DataHelper.GetData();
        }

    }
}

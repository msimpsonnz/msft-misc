using OouiForms.Helpers;
using OouiForms.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OouiForms.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ShopPage : ContentPage
	{
		public ShopPage ()
		{
			InitializeComponent ();

            BindingContext = new ShopViewModel();
        }

        //ToDo
        private async void BuyNowBtn_Clicked(object sender, EventArgs e)
        {
            Console.WriteLine(sender.ToString());
        }
    }
}
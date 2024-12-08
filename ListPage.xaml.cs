using DumitriuConstantinLab7.Models;
namespace DumitriuConstantinLab7;

public partial class ListPage : ContentPage
{
	public ListPage()
	{
		InitializeComponent();
	}
    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var slist = (ShopList)BindingContext;
        slist.Date = DateTime.UtcNow;
        await App.Database.SaveShopListAsync(slist);
        await Navigation.PopAsync();
    }
    async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var slist = (ShopList)BindingContext;
        await App.Database.DeleteShopListAsync(slist);
        await Navigation.PopAsync();
    }
    async void OnChooseButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProductPage((ShopList) this.BindingContext)
        {
            BindingContext = new Product()
        });

    }

    async void OnChooseDeleteButtonClicked(object sender, EventArgs e)
    {
        if (listView.SelectedItem != null)
        {
            var selectedProduct = listView.SelectedItem as Product;
            if (selectedProduct != null)
            {
                var currentShopList = (ShopList)BindingContext;

                var listProduct = await App.Database.GetListProductAsync(currentShopList.ID, selectedProduct.ID);
                if (listProduct != null)
                {
                    await App.Database.DeleteListProductAsync(listProduct);

                    listView.ItemsSource = await App.Database.GetListProductsAsync(currentShopList.ID);
                }
            }
            await Navigation.PopAsync();
        }
    }



    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var shopl = (ShopList)BindingContext;

        listView.ItemsSource = await App.Database.GetListProductsAsync(shopl.ID);
    }

}
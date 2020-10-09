using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XF_CityProvince.Data;
using XF_CityProvince.Models;
using XF_CityProvince.Utilities;

namespace XF_CityProvince
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        App thisApp;
        List<Province> provinces; 

        public MainPage()
        {
            InitializeComponent();
            provinces = new List<Province>();
            thisApp = Application.Current as App;
            thisApp.needCityRefresh = true;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await showProvinces();
            if (thisApp.needCityRefresh)
            {
                refreshCities();
            }
            else
            {
                cityList.IsVisible = true;
            }
            cityList.SelectedItem = null;
        }

        void AddClicked(object sender, EventArgs e)
        {
            City newCity = new City();
            var cityDetailPage = new CityDetailsPage();
            cityDetailPage.BindingContext = newCity;
            cityList.IsVisible = false;
            Navigation.PushAsync(cityDetailPage);

        }
        void citySelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var city = (City)e.SelectedItem;
                var cityDetailPage = new CityDetailsPage();
                cityDetailPage.BindingContext = city;
                cityList.IsVisible = false;
                Navigation.PushAsync(cityDetailPage);
            }
        }

        private void refreshCities()
        {
            thisApp = Application.Current as App;
            if (ddlProvinces.SelectedIndex<1)
            {
                showCities(null);
            }
            else
            {
                string selProvince = ddlProvinces.Items[ddlProvinces.SelectedIndex];
                int? id = thisApp.AllProvinces.Where(d => 
                    d.Name == selProvince)
                    .SingleOrDefault().ID;
                showCities(id.GetValueOrDefault());
            }
            thisApp.needCityRefresh = false;
        }
        private async Task showProvinces()
        {
            if (!(thisApp.AllProvinces?.Count > 0))
            {
                //Get the provinces
                ProvinceRepository r = new ProvinceRepository();
                try
                {
                    
                    provinces.Add(new Province { ID = 0, Code = "00", Name = "All Provinces" });
                    thisApp.AllProvinces = await r.GetProvinces();
                    foreach (Province p in thisApp.AllProvinces.OrderBy(d => d.Name))
                    {
                        provinces.Add(p);
                    }

                    ddlProvinces.ItemsSource = provinces;
                    ddlProvinces.SelectedIndex = 0;
                }

                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        if (ex.GetBaseException().Message.Contains("connection with the server"))
                        {

                            await DisplayAlert("Error", "No connection with the server. Check that the Web Service is running and available and then click the Refresh button.", "Ok");
                        }
                        else
                        {
                            await DisplayAlert("Error", "If the problem persists, please call your system administrator.", "Ok");
                        }
                    }
                    else
                    {
                        if (ex.Message.Contains("NameResolutionFailure"))
                        {
                            await DisplayAlert("Internet Access Error ", "Cannot resolve the Uri: " + Jeeves.DBUri.ToString(), "Ok");
                        }
                        else
                        {
                            await DisplayAlert("General Error ", ex.Message, "Ok");
                        }

                    }
                }
            }
        }
        private async void showCities(int? ProvinceID)
        {
            //Get the cities
            CityRepository r = new CityRepository();
            try
            {
                List<City> cities;
                if (ProvinceID.GetValueOrDefault() > 0)
                {
                    cities = await r.GetCitiesByProvince(ProvinceID.GetValueOrDefault());
                }
                else
                {
                    cities = await r.GetCities();
                }
                cityList.ItemsSource = cities;
                cityList.IsVisible = true;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.GetBaseException().Message.Contains("connection with the server"))
                    {

                        await DisplayAlert("Error", "No connection with the server. Check that the Web Service is running and available and then click the Refresh button.", "Ok");
                    }
                    else
                    {
                        await DisplayAlert("Error", "If the problem persists, please call your system administrator.", "Ok");
                    }
                }
                else
                {
                    await DisplayAlert("General Error", "If the problem persists, please call your system administrator.", "Ok");
                }

            }

        }

        private void ddlProvinces_SelectedIndexChanged(object sender, EventArgs e)
        {
            refreshCities();
        }

    }
}

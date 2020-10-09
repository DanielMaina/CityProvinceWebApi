using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF_CityProvince.Data;
using XF_CityProvince.Models;
using XF_CityProvince.Utilities;

namespace XF_CityProvince
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CityDetailsPage : ContentPage
    {
        City city;
        App thisApp;

        public CityDetailsPage()
        {
            InitializeComponent();
            thisApp = Application.Current as App;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            city = (City)this.BindingContext;
            setProvince();
            if (city.ID == 0)//Adding New
            {
                this.Title = "Add New City";
                btnDelete.IsEnabled = false;
            }
            else
            {
                this.Title = "Edit City Details";
                btnDelete.IsEnabled = true;
            }
        }

        void CancelClicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        void setProvince()
        {
            int assignedProv = 0;
            int count = 0;
            foreach (Province d in thisApp.AllProvinces.OrderBy(d => d.Name))
            {
                ddlProvinces.Items.Add(d.Name);
                if (d.ID == city.ProvinceID)
                {
                    assignedProv = count;
                }
                count++;
            }
            if(city.Province!=null)
            {
                ddlProvinces.SelectedItem = city.Province.Name;
            }
            else
            {
                ddlProvinces.SelectedItem = "Select a Province";
            }
        }

        int getProvinceID()
        {
            int id;
            string selProv = ddlProvinces.Items[ddlProvinces.SelectedIndex];
            if(selProv!= "Select a Province")
            {
                id = thisApp.AllProvinces.Where(d => d.Name == selProv).SingleOrDefault().ID;
            }
            else
            {
                id = 0;
            }
            return id;
        }
        async void SaveClicked(object sender, EventArgs e)
        {
            try
            {
                //city.Province = null;
                city.ProvinceID = getProvinceID();
                if(city.ProvinceID>0)
                {
                    CityRepository r = new CityRepository();
                    if (city.ID == 0)
                    {
                        await r.AddCity(city);
                    }
                    else
                    {
                        await r.UpdateCity(city);
                    }
                    thisApp.needCityRefresh = true;
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Province Not Selected:", "You must set the province for the city.", "Ok");
                }
                
            }
            catch (AggregateException ex)
            {
                string errMsg = "";
                foreach (var exception in ex.InnerExceptions)
                {
                    errMsg += Environment.NewLine + exception.Message;
                }
                await DisplayAlert("One or more exceptions has occurred:", errMsg, "Ok");
            }
            catch (ApiException apiEx)
            {
                var sb = new StringBuilder();
                sb.AppendLine("Errors:");
                foreach (var error in apiEx.Errors)
                {
                    sb.AppendLine("-" + error);
                }
                thisApp.needCityRefresh = true;
                await DisplayAlert("Problem Saving the City:", sb.ToString(), "Ok");
            }
            catch (Exception ex)
            {
                if (ex.GetBaseException().Message.Contains("connection with the server"))
                {
                    await DisplayAlert("Error", "No connection with the server.", "Ok");
                }
                else
                {
                    await DisplayAlert("Error", "Could not complete operation.", "Ok");
                }
            }
        }

        async void DeleteClicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Confirm Delete", "Are you certain that you want to delete " + city.Summary + "?", "Yes", "No");
            if (answer == true)
            {
                try
                {
                    //city.Province = null;
                    CityRepository er = new CityRepository();
                    await er.DeleteCity(city);
                    thisApp.needCityRefresh = true;
                    await Navigation.PopAsync();
                }
                catch (AggregateException ex)
                {
                    string errMsg = "";
                    foreach (var exception in ex.InnerExceptions)
                    {
                        errMsg += Environment.NewLine + exception.Message;
                    }
                    await DisplayAlert("One or more exceptions has occurred:", errMsg, "Ok");
                }
                catch (ApiException apiEx)
                {
                    var sb = new StringBuilder();
                    sb.AppendLine("Errors:");
                    foreach (var error in apiEx.Errors)
                    {
                        sb.AppendLine("-" + error);
                    }
                    await DisplayAlert("Problem Deleting the City:", sb.ToString(), "Ok");
                }
                catch (Exception ex)
                {
                    if (ex.GetBaseException().Message.Contains("connection with the server"))
                    {
                        await DisplayAlert("Error", "No connection with the server.", "Ok");
                    }
                    else
                    {
                        await DisplayAlert("Error", "Could not complete operation.", "Ok");
                    }
                }
            }
        }
    }
}
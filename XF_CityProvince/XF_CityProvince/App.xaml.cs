using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF_CityProvince.Models;

namespace XF_CityProvince
{
    public partial class App : Application
    {
        public List<Province> AllProvinces;
        public bool needCityRefresh;

        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
            var nav = new NavigationPage(new MainPage());
            MainPage = nav;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

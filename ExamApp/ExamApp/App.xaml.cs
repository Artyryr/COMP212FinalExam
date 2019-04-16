using ExamApp.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace ExamApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new Helpers.MasterPageView();
        }

        private const string KEY = "a2b003481632a966c5f43b9671f70a9b";
        private static HttpClient flickrClient = new HttpClient();
        Task<string> flickrTask = null;
        //CHANGE THE TYPE OF USED 
        List<FlickrResult> flickrPhotos = new List<FlickrResult>() { };

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}

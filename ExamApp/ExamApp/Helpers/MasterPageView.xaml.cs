using ExamApp.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ExamApp.Helpers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterPageView : ContentPage
    {
        private const string KEY = "a2b003481632a966c5f43b9671f70a9b";
        private static HttpClient flickrClient = new HttpClient();
        Task<string> flickrTask = null;
        List<FlickrResult> flickrPhotos = new List<FlickrResult>() { };
        List<MasterNavigationItem> masterNavigationItems = new List<MasterNavigationItem>() { };

        public MasterPageView()
        {
            InitializeComponent();
        }

        private async void BtnSearch_Clicked(object sender, EventArgs e)
        {
            // if flickrTask already running, prompt user 
            if (flickrTask?.Status != TaskStatus.RanToCompletion)
            {
                flickrClient.CancelPendingRequests();
            }

            // Flickr's web service URL for searches                         
            var flickrURL = "https://api.flickr.com/services/rest/?method=" +
               $"flickr.photos.search&api_key={KEY}&" +
               $"tags={txtSearch.Text.Replace(" ", ",")}" +
               "&tag_mode=all&per_page=500&privacy_filter=1";

            ListView.ItemsSource = null;
            flickrPhotos = new List<FlickrResult>() { };
            masterNavigationItems = new List<MasterNavigationItem>() { };

            flickrTask = flickrClient.GetStringAsync(flickrURL);

            XDocument flickrXML = XDocument.Parse(await flickrTask);

            flickrPhotos =
            (from photo in flickrXML.Descendants("photo")
             let id = photo.Attribute("id").Value
             let title = photo.Attribute("title").Value
             let secret = photo.Attribute("secret").Value
             let server = photo.Attribute("server").Value
             let farm = photo.Attribute("farm").Value
             select new FlickrResult
             {
                 Title = title,
                 URL = $"https://farm{farm}.staticflickr.com/" +
                   $"{server}/{id}_{secret}.jpg"
             }).ToList();

            if (flickrPhotos.Any())
            {
                await Task.Factory.StartNew(() => {
                    ParallelLoopResult loopResult = Parallel.ForEach<FlickrResult>(flickrPhotos, photo =>
                    {
                        MasterNavigationItem item = new MasterNavigationItem();
                        item.Icon = photo.URL;
                        item.Title = photo.Title;

                        masterNavigationItems.Add(item);
                    });
                });
               ListView.ItemsSource = masterNavigationItems;
            }
            else // no matches were found
            {
                //imagesListBox.Items.Add("No matches");
            }
        }
    }
}
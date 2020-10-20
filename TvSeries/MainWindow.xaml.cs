using MyCouch;
using MyCouch.Net;
using MyCouch.Requests;
using MyCouch.Responses;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace TvSeries
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void ReadButton_Click(object sender, RoutedEventArgs e)
        {
            using (var store = new MyCouchStore("http://admin:admin@localhost:5984", "tv-series"))
            {
                //Get hardcoded document ID.
                var retrieved = await store.GetByIdAsync(SearchIDTxt.Text);

                ReadTxt.Text = retrieved;

                ReadTxt.Text = ReadTxt.Text.Replace(",", "," + System.Environment.NewLine + "    ");
                ReadTxt.Text = ReadTxt.Text.Replace("{", "{" + System.Environment.NewLine + "    ");
                ReadTxt.Text = ReadTxt.Text.Replace("}", System.Environment.NewLine + "}");
                ReadTxt.Text = ReadTxt.Text.Replace(":", ": ");
                ReadTxt.Text = ReadTxt.Text.Replace("[", " [" + System.Environment.NewLine + "    ");
                ReadTxt.Text = ReadTxt.Text.Replace("]", System.Environment.NewLine + "    " +  "]");

                SearchIDTxt.IsEnabled = false;
            }
        }

        private async void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            using (var client = new MyCouchClient("http://admin:admin@localhost:5984", "tv-series"))
            {
                //POST with server generated id & Rev
                await client.Documents.PostAsync("{\"title\": \"" + TitleText.Text + "\", \"creator\":\"" + CreatorTxt.Text + "\",\"stars\":\"" + StarsTxt.Text + 
                    "\",\"seasons\":\"" + SeasonsTxt.Text + "\",\"mpa_rating\":\"" + MPARatingTxt.Text + "\",\"imbd_rating\":\"" + IMBDRatingTxt.Text + "/10\"}");

                TitleText.Text = "";
                CreatorTxt.Text = "";
                StarsTxt.Text = "";
                SeasonsTxt.Text = "";
                MPARatingTxt.Text = "";
                IMBDRatingTxt.Text = "";
            }
        }

        private async void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            using (var client = new MyCouchClient("http://admin:admin@localhost:5984", "tv-series"))
            {
                //PUT for updates

                await client.Documents.PutAsync(SearchIDTxt.Text, ReadTxt.Text);
            }
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            using (var store = new MyCouchStore("http://admin:admin@localhost:5984", "tv-series"))
            {
                //Delete a document

                await store.DeleteAsync(SearchIDTxt.Text);

                SearchIDTxt.Text = "";
                ReadTxt.Text = "";
                SearchIDTxt.IsEnabled = true;
            }

        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            SearchIDTxt.Text = "";
            SearchIDTxt.IsEnabled = true;
            ReadTxt.Text = "";
            TitleText.Text = "";
            CreatorTxt.Text = "";
            StarsTxt.Text = "";
            SeasonsTxt.Text = "";
            MPARatingTxt.Text = "";
            IMBDRatingTxt.Text = "";
        }
    }
}


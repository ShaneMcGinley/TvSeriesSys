using MyCouch;
using MyCouch.Net;
using MyCouch.Requests;
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





                /* using (var client = new MyCouchClient("http://admin:admin@localhost:5984", "tv-series"))
                {

                }

                */
            }
        }

        private async void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            using (var client = new MyCouchClient("http://admin:admin@localhost:5984", "tv-series"))
            {
                //POST with server generated id & Rev
                await client.Documents.PostAsync("{\"title\": \"" + TitleText.Text + "\", \"creator\":\"" + CreatorTxt.Text + "\",\"stars\":\"" + StarsTxt.Text + 
                    "\",\"seasons\":\"" + SeasonsTxt.Text + "\",\"mpa_rating\":\"" + MPARatingTxt.Text + "\",\"imbd_rating\":\"" + IMBDRatingTxt.Text + "/10\"}");
            }
        }

        private async void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            using (var client = new MyCouchClient("http://admin:admin@localhost:5984", "tv-series"))
            {
                //PUT for updates
                await client.Documents.PutAsync("e4862b8c65a4677d4f4fa3d4cf002228", "1-6824e4a536fdf83a246/eca26b943dfe0", "{\"name\":\"Daniel Wertheim\"}");
            }
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            using (var store = new MyCouchStore("http://admin:admin@localhost:5984", "tv-series"))
            {
                //Delete a document
                //await client.Documents.DeleteAsync("e4862b8c65a4677d4f4fa3d4cf002228", "2-9b42031d85a6c397b4f2d51a4b8698ec");

                await store.DeleteAsync(SearchIDTxt.Text);

                SearchIDTxt.Text = "";
                ReadTxt.Text = "";

                //PUT for client generated id
                //await client.Documents.PutAsync("e4862b8c65a4677d4f4fa3d4cf002228", "{\"name\":\"Donald Trump\"}");

                /*//PUT for updates with _rev in JSON
                await client.Documents.PutAsync("someId", "{\"_rev\": \"docRevision\", \"name\":\"Daniel Wertheim\"}");

                *//*//Using entities
                var me = new Person { Id = "SomeId", Name = "Daniel" };
                await client.Entities.PutAsync(me);*//*

                //Using anonymous entities
                await client.Entities.PostAsync(new { Name = "Daniel" });*/
            }

        }

    }
}


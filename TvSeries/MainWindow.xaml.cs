using MyCouch;
using MyCouch.Requests;
using MyCouch.Responses;
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
            Loaded += MainWindow_Loaded;

            CenterProgramOnLaunch();
        }

        // Keeps the Program in the center of the screen when launched.
        private void CenterProgramOnLaunch()
        {
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            /* Query / map reduce for counting the number of tv series documents.
              This is so that the number of tv-serie documents is shown when the program is opened   
            */
            //Connecting to the database / fauxton using MyCouch
            using (var client = new MyCouchClient("http://admin:admin@localhost:5984", "tv-series"))
            {
                var personQuery = new QueryViewRequest("series", "CountNoTvSeries").Configure(query2 => query2
                .Reduce(false));
                ViewQueryResponse result2 = await client.Views.QueryAsync(personQuery);

                NoTvSeriesTxt.Text = result2.RowCount.ToString();
            }
        }

        private async void ReadButton_Click(object sender, RoutedEventArgs e)
        {
            using (var store = new MyCouchStore("http://admin:admin@localhost:5984", "tv-series"))
            {
                //Get User given document ID with MyCouch.
                var retrieved = await store.GetByIdAsync(SearchIDTxt.Text);

                ReadTxt.Text = retrieved;

                //Aligning the text to be in JSON structure.
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
                //POST (create) with server generated id & Rev
                await client.Documents.PostAsync("{\"title\": \"" + TitleText.Text + "\", \"creator\":\"" + CreatorTxt.Text + "\",\"stars\":\"" + StarsTxt.Text + 
                    "\",\"seasons\":\"" + SeasonsTxt.Text + "\",\"mpa_rating\":\"" + MPARatingTxt.Text + "\",\"imbd_rating\":\"" + IMBDRatingTxt.Text + "/10\"}");

                MessageBox.Show("Document Successfully Created", "Tv-Series");

                // Clears all text boxes when a document is created.
                TitleText.Clear();
                CreatorTxt.Clear();
                StarsTxt.Clear();
                SeasonsTxt.Clear();
                MPARatingTxt.Clear();
                IMBDRatingTxt.Clear();

                // Refreshes the number of tv-serie documents when one is created.
                var personQuery = new QueryViewRequest("series", "CountNoTvSeries").Configure(query2 => query2
                .Reduce(false));
                ViewQueryResponse result2 = await client.Views.QueryAsync(personQuery);

                NoTvSeriesTxt.Text = result2.RowCount.ToString();
            }
        }

        private async void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            using (var client = new MyCouchClient("http://admin:admin@localhost:5984", "tv-series"))
            {
                //PUT for updates with MyCouch

                await client.Documents.PutAsync(SearchIDTxt.Text, ReadTxt.Text);

                MessageBox.Show("Document Successfully Updated", "Tv-Series");

                ReadTxt.Clear();

                SearchIDTxt.IsEnabled = true;
            }
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            using (var store = new MyCouchStore("http://admin:admin@localhost:5984", "tv-series"))
            {
                //Delete a document using user given ID

                await store.DeleteAsync(SearchIDTxt.Text);

                MessageBox.Show("Document Successfully Deleted", "Tv-Series");

                SearchIDTxt.Text = "";
                ReadTxt.Text = "";
                SearchIDTxt.IsEnabled = true;
            }

            // Refreshes the number of tv-serie documents when one is removed.
            using (var client = new MyCouchClient("http://admin:admin@localhost:5984", "tv-series"))
            {
                var personQuery = new QueryViewRequest("series", "CountNoTvSeries").Configure(query2 => query2
                .Reduce(false));
                ViewQueryResponse result2 = await client.Views.QueryAsync(personQuery);

                NoTvSeriesTxt.Text = result2.RowCount.ToString();
            }

        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            // Clears all boxes in case of mistake etc..

            SearchIDTxt.Text = "";
            SearchIDTxt.IsEnabled = true;
            ReadTxt.Text = "";
            TitleText.Text = "";
            CreatorTxt.Text = "";
            StarsTxt.Text = "";
            SeasonsTxt.Text = "";
            MPARatingTxt.Text = "";
            IMBDRatingTxt.Text = "";

            MessageBox.Show("All Boxes Have Been Reset", "Tv-Series");
        }
    }
}


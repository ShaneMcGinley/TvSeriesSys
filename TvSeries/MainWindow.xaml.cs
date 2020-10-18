﻿using MyCouch;
using System;
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

        private async void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            using (var client = new MyCouchClient("http://admin:admin@localhost:5984", "tv-series"))
            {
                //POST with server generated id
                await client.Documents.PostAsync("{\"name\":\"Daniel\"}");
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
            using (var client = new MyCouchClient("http://admin:admin@localhost:5984", "tv-series"))
            {
                //Delete a document
                await client.Documents.DeleteAsync("e4862b8c65a4677d4f4fa3d4cf002228", "2-9b42031d85a6c397b4f2d51a4b8698ec");

                //Read a document - Not yet working
                example.Text = Convert.ToString(await client.Documents.GetAsync("e4862b8c65a4677d4f4fa3d4cf00755e"));

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

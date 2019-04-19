using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileApplications
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            recipeList.ItemTapped += (sender, args) =>
            {
                if (recipeList.SelectedItem == null) return;
                ;
                Navigation.PushAsync(new NewRecipePage((Recipe)recipeList.SelectedItem));
                recipeList.SelectedItem = null;
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Recipe>();

                var recipe = conn.Table<Recipe>().ToList();
                recipeList.ItemsSource = recipe;

            }

        }

        private void ToolbarItem_Activated(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NewRecipePage());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;
using MobileApplications.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace MobileApplications
{
    public partial class NewRecipePage : ContentPage
    {
        private Recipe RecipeDetails { get; set; }
        MediaFile SelectedFile { get; set; }

        public NewRecipePage(Recipe details = null)
        {
            InitializeComponent();

            if (details != null)
            {
                RecipeDetails = details;
                nameEntry.Text = details.Name;
                durationEntry.Text = details.Duration;

                ingredientEntry.Text = details.Ingredient;
                ingredientEntry2.Text = details.Ingredient2;
                ingredientEntry3.Text = details.Ingredient3;
                ingredientEntry4.Text = details.Ingredient4;
                ingredientEntry5.Text = details.Ingredient5;
                ingredientEntry6.Text = details.Ingredient6;
                ingredientEntry7.Text = details.Ingredient7;
                ingredientEntry8.Text = details.Ingredient8;

                stepEntry1.Text = details.Step1;
                stepEntry2.Text = details.Step2;
                stepEntry3.Text = details.Step3;
                stepEntry4.Text = details.Step4;
                stepEntry5.Text = details.Step5;
                stepEntry6.Text = details.Step6;
                stepEntry7.Text = details.Step7;
                stepEntry8.Text = details.Step8;

                if (details.ItemData != null)
                {
                    Stream stream = new MemoryStream(details.ItemData);
                    img.Source = ImageSource.FromStream(() =>
                    {
                        var s = stream;
                        return s;
                    });
                }

                btnSave.Text = "Update";
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Recipe>();

                int numberOfRows = 0;
                string taskName = string.Empty;
                byte[] imagedata = null;

                if (SelectedFile != null)
                {
                    imagedata = Converter.ReadFully(SelectedFile.GetStream());
                }

                if (RecipeDetails == null)
                {
                    Recipe recipe = new Recipe()
                    {
                        Name = nameEntry.Text,
                        Duration = durationEntry.Text,
                        ItemData = imagedata,

                        Ingredient = ingredientEntry.Text,
                        Ingredient2 = ingredientEntry2.Text,
                        Ingredient3 = ingredientEntry3.Text,
                        Ingredient4 = ingredientEntry4.Text,
                        Ingredient5 = ingredientEntry5.Text,
                        Ingredient6 = ingredientEntry6.Text,
                        Ingredient7 = ingredientEntry7.Text,
                        Ingredient8 = ingredientEntry8.Text,

                        Step1 = stepEntry1.Text,
                        Step2 = stepEntry2.Text,
                        Step3 = stepEntry3.Text,
                        Step4 = stepEntry4.Text,
                        Step5 = stepEntry5.Text,
                        Step6 = stepEntry6.Text,
                        Step7 = stepEntry7.Text,
                        Step8 = stepEntry8.Text
                    };

                        numberOfRows = conn.Insert(recipe);
                        taskName = "added";
                }
                else
                {
                    RecipeDetails.Name = nameEntry.Text;
                    RecipeDetails.Duration = durationEntry.Text;
                    RecipeDetails.ItemData = imagedata ?? RecipeDetails.ItemData;

                    RecipeDetails.Ingredient = ingredientEntry.Text;
                    RecipeDetails.Ingredient2 = ingredientEntry2.Text;
                    RecipeDetails.Ingredient3 = ingredientEntry3.Text;
                    RecipeDetails.Ingredient4 = ingredientEntry4.Text;
                    RecipeDetails.Ingredient5 = ingredientEntry5.Text;
                    RecipeDetails.Ingredient6 = ingredientEntry6.Text;
                    RecipeDetails.Ingredient7 = ingredientEntry7.Text;
                    RecipeDetails.Ingredient8 = ingredientEntry8.Text;

                    RecipeDetails.Step1 = stepEntry1.Text;
                    RecipeDetails.Step2 = stepEntry2.Text;
                    RecipeDetails.Step3 = stepEntry3.Text;
                    RecipeDetails.Step4 = stepEntry4.Text;
                    RecipeDetails.Step5 = stepEntry5.Text;
                    RecipeDetails.Step6 = stepEntry6.Text;
                    RecipeDetails.Step7 = stepEntry7.Text;
                    RecipeDetails.Step8 = stepEntry8.Text;

                    numberOfRows = conn.Update(RecipeDetails);
                    taskName = "updated";
                }

                if (numberOfRows > 0)
                {
                    DisplayAlert("Success", $"Recipe Successfully {taskName} to Database", "Done");
                    Navigation.PopAsync();
                }
                else
                    DisplayAlert("Failed", $"Recipe not {taskName} to the Database", "Done");

            }

            ClearData();
        }

        private void ClearData()
        {
            nameEntry.Text = string.Empty;
            durationEntry.Text = string.Empty;
            img.Source = null;

            ingredientEntry.Text = string.Empty;
            ingredientEntry2.Text = string.Empty;
            ingredientEntry3.Text = string.Empty;
            ingredientEntry4.Text = string.Empty;
            ingredientEntry5.Text = string.Empty;
            ingredientEntry6.Text = string.Empty;
            ingredientEntry7.Text = string.Empty;
            ingredientEntry8.Text = string.Empty;

            stepEntry1.Text = string.Empty;
            stepEntry2.Text = string.Empty;
            stepEntry3.Text = string.Empty;
            stepEntry4.Text = string.Empty;
            stepEntry5.Text = string.Empty;
            stepEntry6.Text = string.Empty;
            stepEntry7.Text = string.Empty;
            stepEntry8.Text = string.Empty;
        }

        public string PersonalFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

        public void WriteLocalFile(string FileName, byte[] Data)
        {
            string filePath = Path.Combine(PersonalFolderPath, FileName);
            File.WriteAllBytes(filePath, Data);
        }

        public void SelectImage(object sender, EventArgs e)
        {
            ImagePicker((MediaFile file) =>
            {
                if (file == null)
                {
                    return;
                }
                else
                {
                    SelectedFile = file;
                    img.Source = ImageSource.FromStream(() =>
                    {
                        var stream = file.GetStream();
                        return stream;
                    });

                }
            });
        }

        public async void ImagePicker(Action<MediaFile> result)
        {
            try
            {
                MediaFile mediaFile = new MediaFile(null, null);
                var action = "";
                action = await App.Current.MainPage.DisplayActionSheet("Select option", "Cancel", null, "Take Photo", "Camera Roll");

                if (action == "Take Photo")
                {
                    var status = await RuntimePermission.RuntimePermissionStatus(Plugin.Permissions.Abstractions.Permission.Camera);
                    var Storegestatus = await RuntimePermission.RuntimePermissionStatus(Plugin.Permissions.Abstractions.Permission.Storage);
                    if (status == Plugin.Permissions.Abstractions.PermissionStatus.Granted && Storegestatus == Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                            {
                                await App.Current.MainPage.DisplayAlert("Alert", "Camera not available.", "OK");
                                return;
                            }

                            mediaFile = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                            {
                                Directory = "Sample",
                                Name = "test.jpg",
                                CompressionQuality = 70,
                                PhotoSize = PhotoSize.Medium
                            });

                            result.Invoke(mediaFile);

                        });
                    }
                    else if (status != Plugin.Permissions.Abstractions.PermissionStatus.Unknown)
                    {
                        await App.Current.MainPage.DisplayAlert("Alert", "You have not permission to access Camera.", "OK");
                        result.Invoke(null);
                    }
                    else
                    {
                        result.Invoke(null);
                    }
                }
                else if (action == "Camera Roll")
                {
                    var PhotosStatus = await RuntimePermission.RuntimePermissionStatus(Plugin.Permissions.Abstractions.Permission.Camera);
                    var Storegestatus = await RuntimePermission.RuntimePermissionStatus(Plugin.Permissions.Abstractions.Permission.Storage);
                    if (PhotosStatus == Plugin.Permissions.Abstractions.PermissionStatus.Granted && Storegestatus == Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            if (!CrossMedia.Current.IsPickPhotoSupported)
                            {
                                await App.Current.MainPage.DisplayAlert("Alert", "Camera not available.", "OK");
                                return;
                            }
                            mediaFile = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                            {
                                CompressionQuality = 70,
                                PhotoSize = PhotoSize.Medium
                            });
                            result.Invoke(mediaFile);

                        });
                    }
                    else if (PhotosStatus != Plugin.Permissions.Abstractions.PermissionStatus.Unknown)
                    {
                        await App.Current.MainPage.DisplayAlert("Alert", "You have not permission to access Photos.", "OK");
                        result.Invoke(null);
                    }
                    else
                    {
                        result.Invoke(null);
                    }
                }
                else
                {
                    result.Invoke(null);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                result.Invoke(null);
            }
        }
    }
}

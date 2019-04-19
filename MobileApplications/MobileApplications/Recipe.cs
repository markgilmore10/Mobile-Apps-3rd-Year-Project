using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace MobileApplications
{
    public class Recipe
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Duration { get; set; }

        public byte[] ItemData { get; set; }
    

        public string Ingredient { get; set; }

        public string Ingredient2 { get; set; }

        public string Ingredient3 { get; set; }

        public string Ingredient4 { get; set; }

        public string Ingredient5 { get; set; }

        public string Ingredient6 { get; set; }

        public string Ingredient7 { get; set; }

        public string Ingredient8 { get; set; }


        public string Step1 { get; set; }

        public string Step2 { get; set; }

        public string Step3 { get; set; }

        public string Step4 { get; set; }

        public string Step5 { get; set; }

        public string Step6 { get; set; }

        public string Step7 { get; set; }

        public string Step8 { get; set; }


        public ImageSource ImgSource
        {
            get
            {
                if (ItemData != null)
                {
                    Stream stream = new MemoryStream(ItemData);
                    return ImageSource.FromStream(() =>
                    {
                        var s = stream;
                        return s;
                    });
                }
                return null;
            }
        }
    }
}

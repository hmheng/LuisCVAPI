using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuisCVApi.Models
{
    public class ImageMetadata
    {
        /// <summary>
        /// Build from an image path, storing the full local path, but using the filename as ID.
        /// </summary>
        /// <param name="imageFilePath">Local file path.</param>
        public ImageMetadata(string imageFilePath, string fileName)
        {
            this.LocalFilePath = imageFilePath;
            this.FileName = fileName;
            this.Id = this.FileName; // TODO: Worry about collisions, but ID can't handle slashes.
        }

        /// <summary>
        /// Public parameterless constructor for serialization-friendliness.
        /// </summary>
        public ImageMetadata()
        {

        }

        /// <summary>
        /// Store the ImageInsights into the metadata - pulls out tags and caption, stores number of faces and face details.
        /// </summary>
        /// <param name="insights"></param>
        public void AddInsights(ImageInsights insights)
        {
            this.Caption = insights.Caption;
            this.TranslatedCaption = insights.TranslatedCaption; //Marvin
            this.Tags = insights.Tags;

        }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        public Uri BlobUri { get; set; }

        public string LocalFilePath { get; set; }

        public string FileName { get; set; }

        public string Caption { get; set; }

        public string TranslatedCaption { get; set; }//Marvin

        public string[] Tags { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class ImageInsights
    {
        public string ImageId { get; set; }
        public string Caption { get; set; }
        public string TranslatedCaption { get; set; }
        public string[] Tags { get; set; }

    }
}

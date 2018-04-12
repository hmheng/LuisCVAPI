using LuisCVApi.Models;
using LuisCVApi.Repository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace LuisCVApi.Controllers
{
    [Route("api/[controller]")]
    public class ImageController
    {
        public ImageController()
        {
            DocumentDBRepository<ImageMetadata>.Initialize();
        }

        private IEnumerable<ImageMetadata> IndexAsync()
        {
            var items = DocumentDBRepository<ImageMetadata>.FindMatchingDocuments<ImageMetadata>("select * from Images");
            return items;
        }

        [HttpGet("{param}")]
        public async Task<IEnumerable<string>> GetImages(string param)
        {
            string LuisEndPoint = "https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/06e8aeb7-dd4d-4805-976f-b932bf3ff7c6?subscription-key=39001e52b8ab4854ab752211b97ec933&verbose=true&timezoneOffset=0&q=";
            List<string> images = new List<string>();
            HttpClient _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await _httpClient.GetAsync(LuisEndPoint + param);
            if (response.IsSuccessStatusCode)
            {
                string str = await response.Content.ReadAsStringAsync();
                var a = JsonConvert.DeserializeObject<LuisData>(str);

                foreach (var b in IndexAsync().Select(x => x))
                {
                    foreach (var c in b.Tags)
                    {
                        foreach (var ent in a.entities)
                        {
                            if (c.Contains(ent.entity))
                            {
                                images.Add(b.BlobUri.ToString()); //return Url of Images
                            }
                        }
                    }
                }


            }
            return images.Distinct();
        }
    }
}

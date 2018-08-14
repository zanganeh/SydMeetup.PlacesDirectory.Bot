using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace SydMeetup.PlacesDirectory.Bot.LUIS
{
    public class LUISProvider
    {
        public static async Task<LUISResponse> GetEntityFromLUIS(string query)
        {
            query = Uri.EscapeDataString(query);
            LUISResponse Data = new LUISResponse();
            using (HttpClient client = new HttpClient())
            {
                var congetiveServiceUrl = $"{WebConfigurationManager.AppSettings["congetiveServiceUrl"]}&q={query}";
                var msg = await client.GetAsync(congetiveServiceUrl);

                if (msg.IsSuccessStatusCode)
                {
                    var JsonDataResponse = await msg.Content.ReadAsStringAsync();
                    Data = JsonConvert.DeserializeObject<LUISResponse>(JsonDataResponse);
                }
            }
            return Data;
        }
    }
}

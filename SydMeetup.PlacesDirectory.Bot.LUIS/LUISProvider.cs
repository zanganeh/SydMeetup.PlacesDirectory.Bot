using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SydMeetup.PlacesDirectory.Bot.LUIS
{
    public class LUISProvider
    {
        public static async Task<LUISResponse> GetEntityFromLUIS(string Query)
        {
            Query = Uri.EscapeDataString(Query);
            LUISResponse Data = new LUISResponse();
            using (HttpClient client = new HttpClient())
            {
                string RequestURI = "https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/3b94668d-ce9a-4b1e-9785-b4b24f2414c7?subscription-key=357aae3d3ed940468a2e2456919826ad&staging=true&verbose=true&timezoneOffset=600&q=" + Query;
                HttpResponseMessage msg = await client.GetAsync(RequestURI);

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

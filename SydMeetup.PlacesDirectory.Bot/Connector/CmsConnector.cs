using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace SydMeetup.PlacesDirectory.Bot.Connector
{
    public class CmsConnector
    {
        public async Task<QueryResult> GetPlace(string name)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("Accept-Language", "en");
                var response = await client.GetAsync($"{WebConfigurationManager.AppSettings["cmsUrl"]}/api/episerver/v1.0/search/content?top=1&filter=tolower(Name) eq '{name}'");

                if (response != null && response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    return QueryResult.FromJson(responseContent);
                }

                return null;
            }
        }

        public async Task<QueryResult> GetPlaces()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("Accept-Language", "en");
                var response = await client.GetAsync($"{WebConfigurationManager.AppSettings["cmsUrl"]}/api/episerver/v1.0/search/content?top=100&filter=ContentType/any(t:t eq 'PlacePage')");

                if (response != null && response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    return QueryResult.FromJson(responseContent);
                }

                return null;
            }
        }
    }
}
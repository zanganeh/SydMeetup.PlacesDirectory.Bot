using SydMeetup.PlacesDirectory.Bot.Connector;
using SydMeetup.PlacesDirectory.Bot.LUIS;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SydMeetup.PlacesDirectory.Bot.IntentProviders
{
    public class GetHourProvider : IIntentProvider
    {
        private readonly CmsConnector _cmsConnector;

        public GetHourProvider(CmsConnector cmsConnector)
        {
            _cmsConnector = cmsConnector;
        }

        public async Task<string> Execute(IEnumerable<EntityType> entities)
        {
            var placeName = entities.FirstOrDefault(a => a.Type == "Places.PlaceName")?.Entity;

            if (placeName != null)
            {
                var resturant = (await _cmsConnector.GetResturant(placeName))?.Results?.FirstOrDefault();

                return resturant?.TradingHours?.ValueString;
            }

            return null;
        }
    }
}
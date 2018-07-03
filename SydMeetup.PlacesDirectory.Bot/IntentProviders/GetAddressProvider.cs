using SydMeetup.PlacesDirectory.Bot.Connector;
using SydMeetup.PlacesDirectory.Bot.LUIS;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SydMeetup.PlacesDirectory.Bot.IntentProviders
{
    public class GetAddressProvider : IIntentProvider
    {
        private readonly CmsConnector _cmsConnector;

        public GetAddressProvider(CmsConnector cmsConnector)
        {
            _cmsConnector = cmsConnector;
        }

        public async Task<string> Execute(IEnumerable<EntityType> entities)
        {
            var placeName = entities.FirstOrDefault(a => a.Type == "Places.PlaceName")?.Entity;

            if (placeName != null)
            {
                var place = (await _cmsConnector.GetPlace(placeName))?.Results?.FirstOrDefault();

                return place?.Address?.ValueString;
            }

            return null;
        }
    }
}
using SydMeetup.PlacesDirectory.Bot.Connector;
using SydMeetup.PlacesDirectory.Bot.LUIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SydMeetup.PlacesDirectory.Bot.IntentProviders
{
    public class GetListProvider : IIntentProvider
    {
        private readonly CmsConnector _cmsConnector;

        public GetListProvider(CmsConnector cmsConnector)
        {
            _cmsConnector = cmsConnector;
        }

        public async Task<string> Execute(IEnumerable<EntityType> entities)
        {
            var places = await _cmsConnector.GetPlaces();

            if (places != null && places.Results != null && places.Results.Any())
            {
                return $"I have bellow places:{Environment.NewLine} {string.Join(Environment.NewLine, places.Results.Select(a => a.Name))}";
            }

            return null;
        }
    }
}
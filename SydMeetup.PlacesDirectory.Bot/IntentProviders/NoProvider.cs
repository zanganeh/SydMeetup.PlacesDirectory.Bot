using SydMeetup.PlacesDirectory.Bot.LUIS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SydMeetup.PlacesDirectory.Bot.IntentProviders
{
    public class NoProvider : IIntentProvider
    {
        public Task<string> Execute(IEnumerable<EntityType> entities)
        {
            return Task.FromResult("Sorry, I am not getting you...");
        }
    }
}
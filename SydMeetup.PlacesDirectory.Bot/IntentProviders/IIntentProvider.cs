using SydMeetup.PlacesDirectory.Bot.LUIS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SydMeetup.PlacesDirectory.Bot.IntentProviders
{
    public interface IIntentProvider
    {
        Task<string> Execute(IEnumerable<EntityType> entities);
    }
}
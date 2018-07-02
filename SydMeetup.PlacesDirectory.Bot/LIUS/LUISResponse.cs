using System.Collections.Generic;

namespace SydMeetup.PlacesDirectory.Bot.LUIS
{
    public class LUISResponse
    {
        public string Query { get; set; }
        public IntentType TopScoringIntent { get; set; }
        public IEnumerable<EntityType> Entities { get; set; }
    }

    public class IntentType
    {
        public string Intent { get; set; }
        public float Score { get; set; }
    }

    public class EntityType
    {
        public string Entity { get; set; }
        public string Type { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public float Score { get; set; }
    }
}

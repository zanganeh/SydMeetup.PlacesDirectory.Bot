﻿namespace SydMeetup.PlacesDirectory.Bot.LUIS
{
    public class LUISResponse
    {
        public string query { get; set; }
        //public Intent topScoringIntent { get; set; }
        //public Entity[] entities { get; set; }
    }

    public class Intent
    {
        public string intent { get; set; }
        public float score { get; set; }
    }

    public class Entity
    {
        public string entity { get; set; }
        public string type { get; set; }
        public int startIndex { get; set; }
        public int endIndex { get; set; }
        public float score { get; set; }
    }
}

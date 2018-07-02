using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Globalization;

namespace SydMeetup.PlacesDirectory.Bot.Connector
{
    public partial class QueryResult
    {
        [JsonProperty("totalMatching")]
        public long TotalMatching { get; set; }

        [JsonProperty("results")]
        public Result[] Results { get; set; }
    }

    public partial class Result
    {
        [JsonProperty("contentLink")]
        public EntLink ContentLink { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("language")]
        public Language Language { get; set; }

        [JsonProperty("existingLanguages")]
        public Language[] ExistingLanguages { get; set; }

        [JsonProperty("masterLanguage")]
        public Language MasterLanguage { get; set; }

        [JsonProperty("contentType")]
        public string[] ContentType { get; set; }

        [JsonProperty("parentLink")]
        public EntLink ParentLink { get; set; }

        [JsonProperty("routeSegment")]
        public string RouteSegment { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("changed")]
        public DateTimeOffset Changed { get; set; }

        [JsonProperty("created")]
        public DateTimeOffset Created { get; set; }

        [JsonProperty("startPublish")]
        public DateTimeOffset StartPublish { get; set; }

        [JsonProperty("stopPublish")]
        public object StopPublish { get; set; }

        [JsonProperty("saved")]
        public DateTimeOffset Saved { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("metaKeywords")]
        public StringType MetaKeywords { get; set; }

        [JsonProperty("hideSiteHeader")]
        public StringType HideSiteHeader { get; set; }

        [JsonProperty("overview")]
        public StringType Overview { get; set; }

        [JsonProperty("menuItems")]
        public StringType MenuItems { get; set; }

        [JsonProperty("disableIndexing")]
        public StringType DisableIndexing { get; set; }

        [JsonProperty("teaserText")]
        public StringType TeaserText { get; set; }

        [JsonProperty("pageImage")]
        public StringType PageImage { get; set; }

        [JsonProperty("hideSiteFooter")]
        public StringType HideSiteFooter { get; set; }

        [JsonProperty("metaDescription")]
        public StringType MetaDescription { get; set; }

        [JsonProperty("featureImage")]
        public StringType FeatureImage { get; set; }

        [JsonProperty("category")]
        public StringType Category { get; set; }

        [JsonProperty("address")]
        public StringType Address { get; set; }

        [JsonProperty("tradingHours")]
        public StringType TradingHours { get; set; }

        [JsonProperty("metaTitle")]
        public StringType MetaTitle { get; set; }
    }

    public partial class StringType
    {
        [JsonProperty("PropertyDataType$$string")]
        public string PropertyDataTypeString { get; set; }

        [JsonProperty("___types")]
        public string[] Types { get; set; }

        [JsonProperty("$type")]
        public string Type { get; set; }

        [JsonProperty("Value$$string", NullValueHandling = NullValueHandling.Ignore)]
        public string ValueString { get; set; }

        [JsonProperty("Value", NullValueHandling = NullValueHandling.Ignore)]
        public ValueUnion? Value { get; set; }
    }

    public partial class ValueElement
    {
        [JsonProperty("DisplayOption$$string")]
        public string DisplayOptionString { get; set; }

        [JsonProperty("___types")]
        public string[] Types { get; set; }

        [JsonProperty("$type")]
        public string Type { get; set; }

        [JsonProperty("ContentLink")]
        public Value ContentLink { get; set; }
    }

    public partial class Value
    {
        [JsonProperty("WorkId$$number")]
        public long WorkIdNumber { get; set; }

        [JsonProperty("$type")]
        public string Type { get; set; }

        [JsonProperty("___types")]
        public TypeElement[] Types { get; set; }

        [JsonProperty("Id$$number")]
        public long IdNumber { get; set; }

        [JsonProperty("GuidValue")]
        public string GuidValue { get; set; }
    }

    public partial class EntLink
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("workId")]
        public long WorkId { get; set; }

        [JsonProperty("guidValue")]
        public string GuidValue { get; set; }

        [JsonProperty("providerName")]
        public object ProviderName { get; set; }
    }

    public partial class Language
    {
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public enum TypeElement { EPiServerContentApiCoreContentModelReference, SystemObject };

    public partial struct ValueUnion
    {
        public Value Value;
        public ValueElement[] ValueElementArray;

        public bool IsNull => ValueElementArray == null && Value == null;
    }

    public partial class QueryResult
    {
        public static QueryResult FromJson(string json) => JsonConvert.DeserializeObject<QueryResult>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this QueryResult self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                ValueUnionConverter.Singleton,
                TypeElementConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class ValueUnionConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(ValueUnion) || t == typeof(ValueUnion?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.StartObject:
                    var objectValue = serializer.Deserialize<Value>(reader);
                    return new ValueUnion { Value = objectValue };
                case JsonToken.StartArray:
                    var arrayValue = serializer.Deserialize<ValueElement[]>(reader);
                    return new ValueUnion { ValueElementArray = arrayValue };
            }
            throw new Exception("Cannot unmarshal type ValueUnion");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (ValueUnion)untypedValue;
            if (value.ValueElementArray != null)
            {
                serializer.Serialize(writer, value.ValueElementArray);
                return;
            }
            if (value.Value != null)
            {
                serializer.Serialize(writer, value.Value);
                return;
            }
            throw new Exception("Cannot marshal type ValueUnion");
        }

        public static readonly ValueUnionConverter Singleton = new ValueUnionConverter();
    }

    internal class TypeElementConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(TypeElement) || t == typeof(TypeElement?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "EPiServer.ContentApi.Core.ContentModelReference":
                    return TypeElement.EPiServerContentApiCoreContentModelReference;
                case "System.Object":
                    return TypeElement.SystemObject;
            }
            throw new Exception("Cannot unmarshal type TypeElement");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (TypeElement)untypedValue;
            switch (value)
            {
                case TypeElement.EPiServerContentApiCoreContentModelReference:
                    serializer.Serialize(writer, "EPiServer.ContentApi.Core.ContentModelReference");
                    return;
                case TypeElement.SystemObject:
                    serializer.Serialize(writer, "System.Object");
                    return;
            }
            throw new Exception("Cannot marshal type TypeElement");
        }

        public static readonly TypeElementConverter Singleton = new TypeElementConverter();
    }
}
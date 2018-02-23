using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Car.Dto
{
    public class ConfigCallback
    {
        [JsonProperty("returncode")]
        public int ReturnCode { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("time")]
        public int Time { get; set; }

        [JsonProperty("result")]
        public ConfigCallbackResult ConfigCallbackResult { get; set; }
    }

    public class ConfigCallbackResult
    {
        [JsonProperty("configtypeitems")]
        public IList<ConfigParam> ConfigParams { get; set; }
    }
    
    //主/被动安全装备到空调/冰箱
    public class ConfigParam
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("configitems")]
        public IList<ConfigItem> ConfigItems { get; set; }  
    }

    public class ConfigItem
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("valueitems")]
        public IList<ConfigValueItem> ConfigValueItems { get; set; }
    }

    public class ConfigValueItem
    {
        [JsonProperty("specid")]
        public int SpecId { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("price")]
        public IList<Price> Prices { get; set; }
    }

    public class Price
    {
        [JsonProperty("subname")]
        public string SubName { get; set; }

        [JsonProperty("price")]
        public int PriceNum { get; set; }
    }
}
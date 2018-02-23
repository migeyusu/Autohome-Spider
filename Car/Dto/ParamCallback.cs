using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Car.Dto
{
    public class ParamCallback
    {
        [JsonProperty("returncode")]
        public int ReturnCode { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("time")]
        public int Time { get; set; }

        [JsonProperty("result")]
        public ParamCallbackResult ParamCallbackResult { get; set; }
        
    }

    /* 包含基本参数、车身、发动机、变速箱、底盘转向、车轮制动
 */

    public class ParamCallbackResult
    {
        [JsonProperty("paramtypeitems")]
        public IList<ModelParam> ModelParams { get; set; }
    }

    public class ModelParam
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("paramitems")]
        public IList<ParamItem> ParamItems { get; set; }
    }

    public class ParamItem
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("valueitems")]
        public IList<ValueItem> ValueItems { get; set; }
    }

    public class ValueItem
    {
        [JsonProperty("specid")]
        public int SpecId { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
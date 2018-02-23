using System.Collections.Generic;
using Newtonsoft.Json;

namespace Car.Dto
{
    public class SubModelGroup1Dto:Dto
    {
        [JsonProperty("List")]
        public IList<SubModel> SubModels { get; set; }
    }
}
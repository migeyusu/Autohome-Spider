using System.Collections.Generic;
using Newtonsoft.Json;

namespace Car.Dto
{
    public class SubModelGroup0Dto:Dto
    {
        [JsonProperty("List")]
        public IList<SubModelGroup1Dto> SubModelGroup1Dtos { get; set; }
    }
}
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Car.Dto
{
    public class ModelDto:Dto
    {
        [JsonProperty("List")]
        public IList<SubModelGroup0Dto> SubModelGroup0Dtos { get; set; }    
    }
}
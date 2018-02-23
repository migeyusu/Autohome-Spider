using Newtonsoft.Json;

namespace Car.Dto
{
    public class Dto
    {
        [JsonProperty("I")]
        public int Code { get; set; }
        
        [JsonProperty("N")]
        public string Name { get; set; }
    }
}
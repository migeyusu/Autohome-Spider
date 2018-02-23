using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;

namespace Car
{
    public class Series:Entity
    {
        [JsonProperty("List")]
        public virtual IList<Model> Models { get; set; }

        public Series()
        {
            Models=new List<Model>();
        }
    }
}
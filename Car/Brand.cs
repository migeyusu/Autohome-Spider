using System.Collections.Generic;
using Newtonsoft.Json;

namespace Car
{

    /// <summary>
    /// 品牌
    /// </summary>
    public class Brand:Entity
    {
        [JsonProperty("List")]
        public virtual IList<Series> Series { get; set; }

        public Brand()
        {
            Series = new List<Series>();
        }
    }
}
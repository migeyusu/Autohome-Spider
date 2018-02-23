using System.Collections.Generic;
using Newtonsoft.Json;

namespace Car
{
    public class Model : Entity
    {
        [JsonProperty("List")]
        public virtual IList<SubModel> SubModels { get; set; }

        public Model()
        {
            SubModels = new List<SubModel>();
        }
    }
}
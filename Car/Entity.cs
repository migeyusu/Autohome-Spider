using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Car
{
    public class Entity

    {  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [JsonProperty("I")]
        public int Code { get; set; }

        [JsonProperty("N")]
        public string Name { get; set; }
    }
}
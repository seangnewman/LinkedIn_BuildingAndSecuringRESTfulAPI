using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LandonApi.Models
{
    public abstract class Resource
    {
        [JsonProperty(Order = -2)]           // This property is at top of all serialized responses
        public string   Href { get; set; }      //  Every resource will include URI of resource, acts as id
    }
}

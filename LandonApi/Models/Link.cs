using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace LandonApi.Models
{
    public class Link
    {
        public const string GetMethod = "GET";


        [JsonProperty(Order = -4)]
        public string Href { get; set; }

        [JsonProperty(Order = -3, PropertyName ="rel", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Relations { get; set; }

        [JsonProperty(Order = -3,  DefaultValueHandling = DefaultValueHandling.Ignore,  NullValueHandling =NullValueHandling.Ignore)]
        [DefaultValue(GetMethod)]
        public string Method { get; set; }


        // Stores the route name before rewritting by LinkRewritingFilter
        [JsonIgnore]
        public string RouteName { get; set; }

        // Stores the route parameters before rewritting by LinkRewritingFilter
        [JsonIgnore]
        public object RouteValues { get; set; }

        public static Link To(string routeName, object routeValues = null) => new Link
        {
            RouteName = routeName,
            RouteValues = routeValues,
            Method = GetMethod,
            Relations = null

        };
    }

}

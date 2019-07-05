// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Chest.Client.AutorestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class RootModel
    {
        /// <summary>
        /// Initializes a new instance of the RootModel class.
        /// </summary>
        public RootModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the RootModel class.
        /// </summary>
        public RootModel(string title = default(string), string version = default(string), string os = default(string), int processId = default(int))
        {
            Title = title;
            Version = version;
            Os = os;
            ProcessId = processId;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "version")]
        public string Version { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "os")]
        public string Os { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "process_id")]
        public int? ProcessId { get; set; }

    }
}

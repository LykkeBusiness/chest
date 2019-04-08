// (c) Lykke Corporation 2019 - All rights reserved. No copying, adaptation, decompiling, distribution or any other form of use permitted.

// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Chest.Client.AutorestClient.Models
{
    using Microsoft.Rest;
    using Newtonsoft.Json;
    using System.Linq;

    public partial class MetadataModel
    {
        /// <summary>
        /// Initializes a new instance of the MetadataModel class.
        /// </summary>
        public MetadataModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the MetadataModel class.
        /// </summary>
        public MetadataModel(string data, string keywords = default(string))
        {
            Data = data;
            Keywords = keywords;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public string Data { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "keywords")]
        public string Keywords { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (Data == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Data");
            }
            if (Data != null)
            {
                if (Data.Length < 1)
                {
                    throw new ValidationException(ValidationRules.MinLength, "Data", 1);
                }
            }
        }
    }
}

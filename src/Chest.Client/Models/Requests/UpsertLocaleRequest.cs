using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Chest.Client.Models.Requests
{
    public class UpsertLocaleRequest : UserRequest
    {
        [Required]
        public string Id { get; set; }
        [Required(ErrorMessage = "The IsDefault field is required.")]
        [JsonProperty("isDefault")]
        public bool? IsDefaultRequired { get; set; }

        [JsonIgnore]
        public bool IsDefault => IsDefaultRequired ?? default;
    }
}
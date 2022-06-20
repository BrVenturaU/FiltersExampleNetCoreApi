using System.Text.Json.Serialization;

namespace FiltersExample.Models
{
    public class Meta
    {
        public string SomeProp { get; set; }
        [JsonIgnore]
        public string TransactionId { get; set; } = string.Empty;
        [JsonIgnore]
        public string ApplicationSource { get; set; } = string.Empty;
    }
}

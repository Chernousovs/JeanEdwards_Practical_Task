using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace JE_PracticalTask.Models
{
    public class MovieSearchModel
    {
        [JsonPropertyName("Search")]
        public List<MovieShortInfo> Movies { get; set; }
        public string TotalResults { get; set; }
        public string Response { get; set; }
        public string Error { get; set; }
    }
}

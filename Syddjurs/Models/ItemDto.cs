using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Syddjurs.Models
{
    public class ItemDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }


        [JsonPropertyName("name")]
        public string Name { get; set; }


        [JsonPropertyName("color")]
        public string Color { get; set; }


        [JsonPropertyName("description")]       
        public string Description { get; set; }

        [JsonPropertyName("sex")]
        public string Sex { get; set; }

        [JsonPropertyName("number")]
        public int Number { get; set; }

        [JsonPropertyName("lendable")]
        public bool Lendable { get; set; }

        [JsonPropertyName("size")]
        public string Size { get; set; }

        [JsonPropertyName("categoryid")]
        public int? CategoryId { get; set; }

        [JsonPropertyName("categorytext")]
        public string? CategoryText { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Syddjurs.Models
{
    public  class LoanListDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("lender")]
        public string Lender { get; set; }

        [JsonPropertyName("loanDate")]
        public string LoanDate { get; set; }
    }
}

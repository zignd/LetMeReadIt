using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetMeReadIt.Models
{
    public class ParsedPage
    {
        [JsonProperty(PropertyName = "domain")]
        public string Domain { get; set; }

        [JsonProperty(PropertyName = "next_page_id")]
        public string NextPageId { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "short_url")]
        public string ShortUrl { get; set; }

        [JsonProperty(PropertyName = "author")]
        public string Author { get; set; }

        [JsonProperty(PropertyName = "excerpt")]
        public string Excerpt { get; set; }

        [JsonProperty(PropertyName = "direction")]
        public string Direction { get; set; }

        [JsonProperty(PropertyName = "word_count")]
        public int WordCount { get; set; }

        [JsonProperty(PropertyName = "total_pages")]
        public int TotalPages { get; set; }

        [JsonProperty(PropertyName = "content")]
        public string Content { get; set; }

        [JsonProperty(PropertyName = "date_published")]
        public DateTime DatePublished { get; set; }

        [JsonProperty(PropertyName = "dek")]
        public string Dek { get; set; }

        [JsonProperty(PropertyName = "lead_image_url")]
        public string LeadImageUrl { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "rendered_pages")]
        public int RenderedPages { get; set; }

        public override string ToString()
        {
            return Url;
        }
    }
}

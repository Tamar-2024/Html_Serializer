using System.Text.Json;

namespace Htmlserializer
{
    public class HtmlHelper
    {
        public List<string> HtmlTags { get; set; }
        public List<string> HtmlVoidTags { get; set; }

        public readonly static HtmlHelper _instance = new HtmlHelper();             
        public static HtmlHelper Instance { get { return _instance; } }           

        private HtmlHelper()
        {
            string htmlTagsJson = File.ReadAllText("HtmlTags.json");
            HtmlTags = JsonSerializer.Deserialize<List<string>>(htmlTagsJson);   

            string HtmlVoidTagsJson = File.ReadAllText("HtmlVoidTags.json");
            HtmlVoidTags = JsonSerializer.Deserialize<List<string>>(HtmlVoidTagsJson);
        }
    }
}
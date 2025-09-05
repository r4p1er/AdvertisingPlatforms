namespace AdvertisingPlatforms.Models
{
    public class PlatformsTreeNode(string location = "")
    {
        public string Location { get; set; } = location;

        public List<string> Platforms { get; set; } = new();

        public List<PlatformsTreeNode> Children { get; set; } = new();
    }
}

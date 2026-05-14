using System.Text.Json.Serialization;

namespace AFKardsMini
{
    public class SteamJson
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; } = false;

        [JsonPropertyName("data")]
        public Data Data { get; set; } = new Data();
    }

    public class Data
    {
        [JsonPropertyName("type")]
        public string SteamAppType { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("steam_appid")]
        public int SteamAppid { get; set; }

        [JsonPropertyName("required_age")]
        public object RequiredAge { get; set; } = new object();

        [JsonPropertyName("is_free")]
        public bool IsFree { get; set; }

        [JsonPropertyName("controller_support")]
        public string ControllerSupport { get; set; } = string.Empty;

        [JsonPropertyName("detailed_description")]
        public string DetailedDescription { get; set; } = string.Empty;

        [JsonPropertyName("about_the_game")]
        public string AboutTheGame { get; set; } = string.Empty;

        [JsonPropertyName("short_description")]
        public string ShortDescription { get; set; } = string.Empty;

        [JsonPropertyName("supported_languages")]
        public string SupportedLanguages { get; set; } = string.Empty;

        [JsonPropertyName("header_image")]
        public string HeaderImage { get; set; } = string.Empty;

        [JsonPropertyName("website")]
        public string Website { get; set; } = string.Empty;

        [JsonPropertyName("pc_requirements")]
        public object PCRequirements { get; set; } = new object(); // L'ho convertito in object perché da problemi

        [JsonPropertyName("mac_requirements")]
        public object MacRequirements { get; set; } = new object(); // L'ho convertito in object perché da problemi

        [JsonPropertyName("linux_requirements")]
        public object LinuxRequirements { get; set; } = new object(); // L'ho convertito in object perché da problemi

        [JsonPropertyName("legal_notice")]
        public string LegalNotice { get; set; } = string.Empty;

        [JsonPropertyName("developers")]
        public string[] Developers { get; set; } = [];

        [JsonPropertyName("publishers")]
        public string[] Publishers { get; set; } = [];

        [JsonPropertyName("price_overview")]
        public PriceOverview PriceOverview { get; set; } = new PriceOverview();

        [JsonPropertyName("packages")]
        public int[] Packages { get; set; } = [];

        [JsonPropertyName("package_groups")]
        public PackageGroups[] PackageGroups { get; set; } = [];

        [JsonPropertyName("platforms")]
        public Platforms Platforms { get; set; } = new Platforms();

        [JsonPropertyName("metacritic")]
        public Metacritic Metacritic { get; set; } = new Metacritic();

        [JsonPropertyName("categories")]
        public Category[] Categories { get; set; } = [];

        [JsonPropertyName("genres")]
        public Genre[] Genres { get; set; } = [];

        [JsonPropertyName("screenshots")]
        public Screenshot[] Screenshots { get; set; } = [];

        [JsonPropertyName("movies")]
        public Movie[] Movies { get; set; } = [];

        [JsonPropertyName("recommendations")]
        public Recommendations Recommendations { get; set; } = new Recommendations();

        [JsonPropertyName("achievements")]
        public Achievements Achievements { get; set; } = new Achievements();

        [JsonPropertyName("release_date")]
        public ReleaseDate ReleaseDate { get; set; } = new ReleaseDate();

        [JsonPropertyName("support_info")]
        public SupportInfo SupportInfo { get; set; } = new SupportInfo();

        [JsonPropertyName("background")]
        public string Background { get; set; } = string.Empty;

        [JsonPropertyName("background_raw")]
        public string BackgroundRaw { get; set; } = string.Empty;

        [JsonPropertyName("content_descriptors")]
        public ContentDescriptors ContentDescriptors { get; set; } = new ContentDescriptors();
    }

    // L'ho convertito in object perché da problemi
    public class HardwareRequirements
    {
        [JsonPropertyName("minimum")]
        public string Minimum { get; set; } = string.Empty;

        [JsonPropertyName("recommended")]
        public string Recommended { get; set; } = string.Empty;
    }

    public class PriceOverview
    {
        [JsonPropertyName("currency")]
        public string Currency { get; set; } = string.Empty;

        [JsonPropertyName("initial")]
        public int Initial { get; set; }

        [JsonPropertyName("final")]
        public int Final { get; set; }

        [JsonPropertyName("discount_percent")]
        public int Discountpercent { get; set; }

        [JsonPropertyName("initial_formatted")]
        public string InitialFormatted { get; set; } = string.Empty;

        [JsonPropertyName("final_formatted")]
        public string FinalFormatted { get; set; } = string.Empty;
    }

    public class Platforms
    {
        [JsonPropertyName("windows")]
        public bool Windows { get; set; }

        [JsonPropertyName("mac")]
        public bool Mac { get; set; }

        [JsonPropertyName("linux")]
        public bool Linux { get; set; }
    }

    public class Metacritic
    {
        [JsonPropertyName("score")]
        public int Score { get; set; }

        [JsonPropertyName("url")]
        public string URL { get; set; } = string.Empty;
    }

    public class Recommendations
    {
        [JsonPropertyName("total")]
        public int Total { get; set; }
    }

    public class Achievements
    {
        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("highlighted")]
        public Highlighted[] Highlighted { get; set; } = [];
    }

    public class Highlighted
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("path")]
        public string Path { get; set; } = string.Empty;
    }

    public class ReleaseDate
    {
        [JsonPropertyName("coming_soon")]
        public bool ComingSoon { get; set; }

        [JsonPropertyName("date")]
        public string Date { get; set; } = string.Empty;
    }

    public class SupportInfo
    {
        [JsonPropertyName("url")]
        public string URL { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;
    }

    public class ContentDescriptors
    {
        [JsonPropertyName("ids")]
        public object[] IDs { get; set; } = [];

        [JsonPropertyName("notes")]
        public object Notes { get; set; } = new object();
    }

    public class PackageGroups
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("selection_text")]
        public string SelectionText { get; set; } = string.Empty;

        [JsonPropertyName("save_text")]
        public string SaveText { get; set; } = string.Empty;

        [JsonPropertyName("display_type")]
        public int? DisplayType { get; set; }

        [JsonPropertyName("is_recurring_subscription")]
        public string IsRecurringSubscription { get; set; } = string.Empty;

        [JsonPropertyName("subs")]
        public Sub[]? Subs { get; set; }
    }

    public class Sub
    {
        [JsonPropertyName("packageid")]
        public int? PackageID { get; set; }

        [JsonPropertyName("percent_savings_text")]
        public string PercentSavingsText { get; set; } = string.Empty;

        [JsonPropertyName("percent_savings")]
        public int? PercentSavings { get; set; }

        [JsonPropertyName("option_text")]
        public string OptionText { get; set; } = string.Empty;

        [JsonPropertyName("option_description")]
        public string OptionDescription { get; set; } = string.Empty;

        [JsonPropertyName("can_get_free_license")]
        public string CanGetFreeLicense { get; set; } = string.Empty;

        [JsonPropertyName("is_free_license")]
        public bool? IsFreeLicense { get; set; }

        [JsonPropertyName("price_in_cents_with_discount")]
        public int? PriceInCentsWithDiscount { get; set; }
    }

    public class Category
    {
        [JsonPropertyName("id")]
        public int? ID { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
    }

    public class Genre
    {
        [JsonPropertyName("id")]
        public string ID { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
    }

    public class Screenshot
    {
        [JsonPropertyName("id")]
        public int? ID { get; set; }

        [JsonPropertyName("path_thumbnail")]
        public string PathThumbnail { get; set; } = string.Empty;

        [JsonPropertyName("path_full")]
        public string PathFull { get; set; } = string.Empty;
    }

    public class Movie
    {
        [JsonPropertyName("id")]
        public int? ID { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("thumbnail")]
        public string Thumbnail { get; set; } = string.Empty;

        [JsonPropertyName("webm")]
        public Webm? Webm { get; set; }

        [JsonPropertyName("mp4")]
        public Mp4? Mp4 { get; set; }

        [JsonPropertyName("highlight")]
        public bool? Highlight { get; set; }
    }

    public class Webm
    {
        [JsonPropertyName("480")]
        public string Px480 { get; set; } = string.Empty;

        [JsonPropertyName("max")]
        public string Max { get; set; } = string.Empty;
    }

    public class Mp4
    {
        [JsonPropertyName("480")]
        public string Px480 { get; set; } = string.Empty;

        [JsonPropertyName("max")]
        public string Max { get; set; } = string.Empty;
    }




    public class OtherDetails
    {
        [JsonPropertyName("response")]
        public Response Response { get; set; } = new Response();
    }

    public class Response
    {
        [JsonPropertyName("apps")]
        public App[] Apps { get; set; } = [];
    }

    public class App
    {
        [JsonPropertyName("appid")]
        public int Appid { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("icon")]
        public string Icon { get; set; } = string.Empty;

        [JsonPropertyName("community_visible_stats")]
        public bool CommunityVisibleStats { get; set; }

        [JsonPropertyName("propagation")]
        public string Propagation { get; set; } = string.Empty;

        [JsonPropertyName("app_type")]
        public int AppType { get; set; }

        [JsonPropertyName("content_descriptorids")]
        public int[] ContentDescriptorIds { get; set; } = [];

        [JsonPropertyName("content_descriptorids_including_dlc")]
        public int[] ContentDescriptorIdsIncludingDlc { get; set; } = [];
    }
}

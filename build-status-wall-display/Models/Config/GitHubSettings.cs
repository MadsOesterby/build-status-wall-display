namespace Red_and_Green_boxes.Models.Config
{
    /// <summary>
    /// Configuration settings for GitHub API access and workflows to display
    /// </summary>
    public class GitHubSettings
    {
        public const string SectionName = "GitHub";
        
        public string AccessToken { get; set; } = string.Empty;
        public List<GitHubWorkflow> Workflows { get; set; } = new List<GitHubWorkflow>();
    }
}
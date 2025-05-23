using System.Text.Json.Serialization;

namespace BuildStatusWallDisplay.Models
{
    /// <summary>
    /// Represents a GitHub workflow configuration from the JSON file
    /// </summary>
    public class GitHubWorkflow
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        
        [JsonPropertyName("owner")]
        public string Owner { get; set; } = string.Empty;
        
        [JsonPropertyName("repo")]
        public string Repo { get; set; } = string.Empty;
        
        [JsonPropertyName("workflowFileName")]
        public string WorkflowFileName { get; set; } = string.Empty;
        
        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; } = string.Empty;
    }
}
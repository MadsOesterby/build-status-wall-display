using System.Text.Json.Serialization;

namespace Red_and_Green_boxes.Models
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
        
        [JsonPropertyName("workflowId")]
        public string WorkflowId { get; set; } = string.Empty;
        
        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; } = string.Empty;
    }
}
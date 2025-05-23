namespace BuildStatusWallDisplay.Models
{
    /// <summary>
    /// Represents the current status of a GitHub workflow
    /// </summary>
    public class WorkflowStatus
    {
        public string Name { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public BuildStatus Status { get; set; }
        public bool IsBuilding { get; set; }
        public DateTime LastUpdated { get; set; }
        
        /// <summary>
        /// Error message explaining why the build status could not be determined
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;
        
        /// <summary>
        /// Indicates if there's an error explanation available
        /// </summary>
        public bool HasError => Status == BuildStatus.Unknown && !string.IsNullOrEmpty(ErrorMessage);

        /// <summary>
        /// URL to the build details page on GitHub
        /// </summary>
        public string BuildDetailsUrl { get; set; } = string.Empty;
    }

    public enum BuildStatus
    {
        Success,
        Failure,
        Unknown
    }
}
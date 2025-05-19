namespace Red_and_Green_boxes.Models
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
    }

    public enum BuildStatus
    {
        Success,
        Failure,
        Unknown
    }
}
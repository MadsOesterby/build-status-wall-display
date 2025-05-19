using Red_and_Green_boxes.Models;

namespace Red_and_Green_boxes.Services
{
    /// <summary>
    /// Interface for GitHub API service
    /// </summary>
    public interface IGitHubService
    {
        /// <summary>
        /// Gets the current status of all configured workflows
        /// </summary>
        /// <returns>A list of workflow statuses</returns>
        Task<List<WorkflowStatus>> GetWorkflowStatusesAsync();
    }
}
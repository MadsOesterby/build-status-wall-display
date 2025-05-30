using Microsoft.Extensions.Options;
using Octokit;
using BuildStatusWallDisplay.Models;
using BuildStatusWallDisplay.Models.Config;

namespace BuildStatusWallDisplay.Services
{
    /// <summary>
    /// Service to interact with GitHub API
    /// </summary>
    public class GitHubService : IGitHubService
    {
        private readonly GitHubClient _githubClient;
        private readonly ILogger<GitHubService> _logger;
        private readonly ConfigurationHelper _configHelper;
        private readonly GitHubSettings _settings;

        public GitHubService(
            IOptions<GitHubSettings> settings, 
            ILogger<GitHubService> logger,
            ConfigurationHelper configHelper)
        {
            _settings = settings.Value;
            _logger = logger;
            _configHelper = configHelper;

            _githubClient = new GitHubClient(new ProductHeaderValue("BuildStatusWallDisplay"));
            
            if (!string.IsNullOrEmpty(_settings.AccessToken))
            {
                _githubClient.Credentials = new Credentials(_settings.AccessToken);
            }
        }

        /// <summary>
        /// Gets the current status of all configured workflows
        /// </summary>
        /// <returns>A list of workflow statuses</returns>
        public async Task<List<WorkflowStatus>> GetWorkflowStatusesAsync()
        {
            var statuses = new List<WorkflowStatus>();
            
            // Read workflows from the configuration file
            var workflows = _configHelper.ReadWorkflowsFromFile();
            
            // If no workflows are configured in the file, fall back to appsettings.json
            if (workflows.Count == 0)
            {
                workflows = _settings.Workflows;
            }

            foreach (var workflow in workflows)
            {
                try
                {
                    var workflowRuns = await _githubClient.Actions.Workflows.Runs.ListByWorkflow(
                        workflow.Owner, 
                        workflow.Repo,
                        workflow.WorkflowFileName);

                    if (workflowRuns.TotalCount > 0)
                    {
                        var latestRun = workflowRuns.WorkflowRuns.OrderByDescending(r => r.CreatedAt).First();
                        
                        var status = new WorkflowStatus
                        {
                            Name = workflow.Name,
                            DisplayName = !string.IsNullOrEmpty(workflow.DisplayName) ? workflow.DisplayName : workflow.Name,
                            Status = GetBuildStatus(latestRun.Conclusion),
                            IsBuilding = latestRun.Status.Value == WorkflowRunStatus.InProgress,
                            LastUpdated = latestRun.UpdatedAt.DateTime,
                            BuildDetailsUrl = latestRun.HtmlUrl
                        };
                        
                        // If status is unknown, add error information
                        if (status.Status == BuildStatus.Unknown && latestRun.Conclusion != null)
                        {
                            status.ErrorMessage = $"Workflow concluded with status: {latestRun.Conclusion.Value.StringValue}";
                        }
                        
                        statuses.Add(status);
                    }
                    else
                    {
                        _logger.LogWarning("No workflow runs found for {WorkflowName}", workflow.Name);
                        
                        statuses.Add(new WorkflowStatus
                        {
                            Name = workflow.Name,
                            DisplayName = !string.IsNullOrEmpty(workflow.DisplayName) ? workflow.DisplayName : workflow.Name,
                            Status = BuildStatus.Unknown,
                            IsBuilding = false,
                            LastUpdated = DateTime.UtcNow,
                            ErrorMessage = "No workflow runs found for this configuration",
                            BuildDetailsUrl = $"https://github.com/{workflow.Owner}/{workflow.Repo}/actions/workflows/{workflow.WorkflowFileName}"
                        });
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error fetching workflow status for {WorkflowName}", workflow.Name);
                    
                    // Create a status object with error details
                    var errorMessage = ex.Message;
                    if (ex is ApiException apiEx)
                    {
                        errorMessage = $"GitHub API error: {apiEx.Message} (Status: {apiEx.StatusCode})";
                    }
                    
                    statuses.Add(new WorkflowStatus
                    {
                        Name = workflow.Name,
                        DisplayName = !string.IsNullOrEmpty(workflow.DisplayName) ? workflow.DisplayName : workflow.Name,
                        Status = BuildStatus.Unknown,
                        IsBuilding = false,
                        LastUpdated = DateTime.UtcNow,
                        ErrorMessage = errorMessage,
                        BuildDetailsUrl = $"https://github.com/{workflow.Owner}/{workflow.Repo}/actions/workflows/{workflow.WorkflowFileName}"
                    });
                }
            }

            return statuses;
        }

        private BuildStatus GetBuildStatus(StringEnum<WorkflowRunConclusion>? conclusion)
        {
            if (conclusion == null)
                return BuildStatus.Unknown;

            return conclusion.Value.Value switch
            {
                WorkflowRunConclusion.Success => BuildStatus.Success,
                WorkflowRunConclusion.Failure => BuildStatus.Failure,
                _ => BuildStatus.Unknown
            };
        }
    }
}

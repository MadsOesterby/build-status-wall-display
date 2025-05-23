using System.Text.Json;
using BuildStatusWallDisplay.Models;
using BuildStatusWallDisplay.Models.Config;

namespace BuildStatusWallDisplay.Services
{
    /// <summary>
    /// Helper service for working with the configuration file
    /// </summary>
    public class ConfigurationHelper
    {
        private readonly ILogger<ConfigurationHelper> _logger;
        private readonly IWebHostEnvironment _environment;

        public ConfigurationHelper(ILogger<ConfigurationHelper> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }

        /// <summary>
        /// Reads GitHub workflow configuration from workflows.json file
        /// </summary>
        public List<GitHubWorkflow> ReadWorkflowsFromFile()
        {
            try
            {
                var filePath = Path.Combine(_environment.ContentRootPath, "workflows.json");
                
                if (!File.Exists(filePath))
                {
                    _logger.LogWarning("Workflows configuration file not found: {FilePath}", filePath);
                    return new List<GitHubWorkflow>();
                }

                var json = File.ReadAllText(filePath);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var config = JsonSerializer.Deserialize<WorkflowsConfig>(json, options);
                return config?.Workflows ?? new List<GitHubWorkflow>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading workflows configuration file");
                return new List<GitHubWorkflow>();
            }
        }

        private class WorkflowsConfig
        {
            public List<GitHubWorkflow> Workflows { get; set; } = new List<GitHubWorkflow>();
        }
    }
}

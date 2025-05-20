using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using BuildStatusWallDisplay.Models;
using BuildStatusWallDisplay.Services;

namespace BuildStatusWallDisplay.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGitHubService _gitHubService;
        private readonly IMemoryCache _memoryCache;
        private const string CacheKey = "WorkflowStatuses";
        private const int CacheExpirationMinutes = 2;

        public HomeController(
            ILogger<HomeController> logger,
            IGitHubService gitHubService,
            IMemoryCache memoryCache)
        {
            _logger = logger;
            _gitHubService = gitHubService;
            _memoryCache = memoryCache;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Try to get the workflow statuses from cache
                if (!_memoryCache.TryGetValue(CacheKey, out List<WorkflowStatus> statuses))
                {
                    // If not in cache, fetch from GitHub
                    statuses = await _gitHubService.GetWorkflowStatusesAsync();
                    
                    // Store in cache
                    var cacheOptions = new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(CacheExpirationMinutes));
                    
                    _memoryCache.Set(CacheKey, statuses, cacheOptions);
                }

                return View(statuses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching workflow statuses");
                return View(new List<WorkflowStatus>());
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

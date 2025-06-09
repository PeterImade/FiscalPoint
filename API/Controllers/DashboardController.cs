using Application.Features.Transactions;
using Application.Interfaces.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/dashboard")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [Authorize]
        [HttpGet("summary")]
        public async Task<IActionResult> GetDashboardSummary(CancellationToken cancellationToken)
        {
            var dashboardSummary = await _dashboardService.GetDashboardSummary(cancellationToken);

            return Ok(new APIResponse { Message = "Dashboard summary retrieved successfully!", Data = dashboardSummary });
        }

        [Authorize]
        [HttpGet("income-vs-expense")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetIncomeVsExpenseTrend(CancellationToken cancellationToken)
        {
            var result = await _dashboardService.GetIncomeVsExpenseTrendAsync(cancellationToken);
            return Ok(new APIResponse { Message = "Income and Expense trend retrieved successfully!", Data = result });
        }

        [Authorize]
        [HttpGet("category-breakdown")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCategoryBreakdown(CancellationToken cancellationToken)
        {
            var categoryBreakdown = await _dashboardService.GetCategoryBreakdownAsync(cancellationToken);

            return Ok(new APIResponse {Message = "Category breakdown retrieved successfully", Data = categoryBreakdown });
        }
        
        [Authorize]
        [HttpGet("recent-transactions")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRecentTransactions(CancellationToken cancellationToken)
        {
            var recentTransactions = await _dashboardService.GetRecentTransactions(cancellationToken);

            return Ok(new APIResponse {Message = "Recent transactions retrieved successfully", Data = recentTransactions });
        }


    }
}

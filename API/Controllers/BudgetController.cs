using Application.Features.Budgets;
using Application.Features.Transactions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/budgets")]
    [ApiController]
    public class BudgetController : ControllerBase
    {
        private readonly IBudgetService _budgetService;

        public BudgetController(IBudgetService budgetService)
        {
            _budgetService = budgetService; 
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateBudget(CreateBudgetDto createBudgetDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid budget data.", Errors = ModelState.Values.SelectMany(v => v.Errors) });
            }

            var budget = await _budgetService.CreateAsync(createBudgetDto, cancellationToken);

            return StatusCode(201, new APIResponse { Message = "Budget created successfully!", Data = budget });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllBudgets(CancellationToken cancellationToken, [FromQuery] BudgetFilterDto budgetFilterDto)
        {
            var budgets = await _budgetService.GetAllAsync(budgetFilterDto, cancellationToken);

            return Ok(new APIResponse { Message = "Budgets retrieved successfully!", Data = budgets});
        }

        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetBudget([FromRoute] int id, CancellationToken cancellationToken)
        {
            var budget = await _budgetService.GetAsync(id, cancellationToken);

            return Ok(new APIResponse { Data = budget, Message = "Budget retrieved successfully!" });
        }

        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateBudget([FromRoute] int id, [FromBody] CreateBudgetDto updateBudgetDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid budget data.", Errors = ModelState.Values.SelectMany(v => v.Errors) });
            }

            var updateBudget = await _budgetService.UpdateAsync(id, updateBudgetDto, cancellationToken);

            return Ok(new APIResponse { Data = updateBudget, Message = "Budget updated successfully!" });
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteBudget([FromRoute] int id, CancellationToken cancellationToken)
        {
            await _budgetService.DeleteAsync(id, cancellationToken);

            return NoContent();
        }
    }
}
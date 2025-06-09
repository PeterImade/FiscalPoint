using Application.Features.Transactions;
using Application.Features.User;
using Application.Interfaces.Services;
using Domain.Entities;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/transactions")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateTransaction(CreateTransactionDto createTransactionDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid transaction data.", Errors = ModelState.Values.SelectMany(v => v.Errors) });
            }

            var transaction = await _transactionService.CreateAsync(createTransactionDto, cancellationToken);

            return StatusCode(201, new APIResponse { Message = "Transaction created successfully!", Data = transaction });
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllTransactions([FromQuery] TransactionFilterDto filterDto, CancellationToken cancellationToken)
        { 
            var transactions = await _transactionService.GetPagedAndFilteredTransactions(filterDto, cancellationToken);
            return Ok(transactions);
        }

        [Authorize]
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTransaction([FromRoute] int id, CancellationToken cancellationToken)
        {
            var transaction = await _transactionService.GetAsync(id, cancellationToken);
            return Ok(new APIResponse { Data = transaction, Message = "Transaction retrieved successfully!"});
        }
        
        [Authorize]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateTransaction([FromRoute] int id, [FromBody] CreateTransactionDto updateTransactionDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid transaction data.", Errors = ModelState.Values.SelectMany(v => v.Errors) });
            }

            var updatedTransaction = await _transactionService.UpdateAsync(id, updateTransactionDto, cancellationToken);
            
            return Ok(new APIResponse { Data = updatedTransaction, Message = "Transaction updated successfully!"});
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteTransaction([FromRoute] int id, CancellationToken cancellationToken)
        {
            await _transactionService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}

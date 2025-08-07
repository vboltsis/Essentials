using HybridCachePoC.Models;
using HybridCachePoC.Services;
using Microsoft.AspNetCore.Mvc;

namespace HybridCachePoC.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;
    private readonly ILogger<CustomersController> _logger;

    public CustomersController(ICustomerService customerService, ILogger<CustomersController> logger)
    {
        _customerService = customerService;
        _logger = logger;
    }

    /// <summary>
    /// Get all customers with cache-aside pattern
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Customer>), 200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<IEnumerable<Customer>>> GetAllCustomers()
    {
        try
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return Ok(customers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all customers");
            return StatusCode(500, "An error occurred while retrieving customers");
        }
    }

    /// <summary>
    /// Get a specific customer by ID with cache-aside pattern
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(Customer), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<Customer>> GetCustomer(Guid id)
    {
        try
        {
            var customer = await _customerService.GetCustomerAsync(id);
            if (customer == null)
            {
                return NotFound($"Customer with ID {id} not found");
            }

            return Ok(customer);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving customer {CustomerId}", id);
            return StatusCode(500, "An error occurred while retrieving the customer");
        }
    }

    /// <summary>
    /// Create a new customer and invalidate cache
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Customer), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<Customer>> CreateCustomer([FromBody] CreateCustomerRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = await _customerService.CreateCustomerAsync(request);
            return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating customer");
            return StatusCode(500, "An error occurred while creating the customer");
        }
    }

    /// <summary>
    /// Update a customer and invalidate cache
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(Customer), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<Customer>> UpdateCustomer(Guid id, [FromBody] UpdateCustomerRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = await _customerService.UpdateCustomerAsync(id, request);
            if (customer == null)
            {
                return NotFound($"Customer with ID {id} not found");
            }

            return Ok(customer);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating customer {CustomerId}", id);
            return StatusCode(500, "An error occurred while updating the customer");
        }
    }

    /// <summary>
    /// Delete a customer and invalidate cache
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult> DeleteCustomer(Guid id)
    {
        try
        {
            var deleted = await _customerService.DeleteCustomerAsync(id);
            if (!deleted)
            {
                return NotFound($"Customer with ID {id} not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting customer {CustomerId}", id);
            return StatusCode(500, "An error occurred while deleting the customer");
        }
    }

    /// <summary>
    /// Get cache information for a specific customer
    /// </summary>
    [HttpGet("{id:guid}/cache-info")]
    [ProducesResponseType(typeof(CustomerCacheInfo), 200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<CustomerCacheInfo>> GetCustomerCacheInfo(Guid id)
    {
        try
        {
            var cacheInfo = await _customerService.GetCustomerCacheInfoAsync(id);
            return Ok(cacheInfo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving cache info for customer {CustomerId}", id);
            return StatusCode(500, "An error occurred while retrieving cache information");
        }
    }

    /// <summary>
    /// Get cache statistics for all customers
    /// </summary>
    [HttpGet("cache-statistics")]
    [ProducesResponseType(typeof(IEnumerable<CustomerCacheInfo>), 200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<IEnumerable<CustomerCacheInfo>>> GetCacheStatistics()
    {
        try
        {
            var cacheStats = await _customerService.GetCacheStatisticsAsync();
            return Ok(cacheStats);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving cache statistics");
            return StatusCode(500, "An error occurred while retrieving cache statistics");
        }
    }

    /// <summary>
    /// Clear all cache entries
    /// </summary>
    [HttpPost("clear-cache")]
    [ProducesResponseType(200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult> ClearCache()
    {
        try
        {
            await _customerService.ClearCacheAsync();
            return Ok(new { message = "Cache cleared successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error clearing cache");
            return StatusCode(500, "An error occurred while clearing the cache");
        }
    }

    /// <summary>
    /// Warm the cache by pre-loading all customers
    /// </summary>
    [HttpPost("warm-cache")]
    [ProducesResponseType(200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult> WarmCache()
    {
        try
        {
            await _customerService.WarmCacheAsync();
            return Ok(new { message = "Cache warming completed successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error warming cache");
            return StatusCode(500, "An error occurred while warming the cache");
        }
    }
} 
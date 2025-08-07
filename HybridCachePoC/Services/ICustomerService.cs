using HybridCachePoC.Models;

namespace HybridCachePoC.Services;

public interface ICustomerService
{
    Task<Customer?> GetCustomerAsync(Guid id);
    Task<IEnumerable<Customer>> GetAllCustomersAsync();
    Task<Customer> CreateCustomerAsync(CreateCustomerRequest request);
    Task<Customer?> UpdateCustomerAsync(Guid id, UpdateCustomerRequest request);
    Task<bool> DeleteCustomerAsync(Guid id);
    Task<CustomerCacheInfo> GetCustomerCacheInfoAsync(Guid id);
    Task<IEnumerable<CustomerCacheInfo>> GetCacheStatisticsAsync();
    Task ClearCacheAsync();
    Task WarmCacheAsync();
} 
using Microsoft.EntityFrameworkCore;

namespace Benchmark;

/*
|                Method |     Mean |   Error |  StdDev |   Gen0 | Allocated |
|---------------------- |---------:|--------:|--------:|-------:|----------:|
|         GetBlogsAsync | 235.8 us | 4.71 us | 8.85 us | 0.9766 |  10.85 KB |
| GetBlogsNoChecksAsync | 228.2 us | 4.51 us | 9.30 us | 0.9766 |   10.2 KB |
*/

[MemoryDiagnoser]
public class ThreadSafetyDisabledVsNormal
{
    private readonly BlogContext _context;
    private readonly BlogThreadContext _threadContext;

    public ThreadSafetyDisabledVsNormal()
    {
        _context = new BlogContext();
        _threadContext = new BlogThreadContext();
    }

    [Benchmark]
    public async Task<Blog> GetBlogsAsync()
    {
        return await _context.Blogs.AsNoTracking().FirstOrDefaultAsync(b => b.BlogId == 4000);
    }

    [Benchmark]
    public async Task<Blog> GetBlogsNoChecksAsync()
    {
        return await _threadContext.Blogs.AsNoTracking().FirstOrDefaultAsync(b => b.BlogId == 4000);
    }
}

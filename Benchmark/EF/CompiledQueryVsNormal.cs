using Microsoft.EntityFrameworkCore;

namespace Benchmark;

/*
|              Method |     Mean |   Error |  StdDev |   Gen0 | Allocated |
|-------------------- |---------:|--------:|--------:|-------:|----------:|
|       GetBlogsAsync | 195.6 us | 3.67 us | 3.61 us | 0.7324 |   7.91 KB |
| GetBlogsNormalAsync | 217.7 us | 1.69 us | 1.50 us | 1.2207 |  10.85 KB |

-----------optionsBuilder.EnableThreadSafetyChecks(false);-----------------

|              Method |     Mean |   Error |  StdDev |   Gen0 | Allocated |
|-------------------- |---------:|--------:|--------:|-------:|----------:|
|       GetBlogsAsync | 194.0 us | 1.90 us | 1.77 us | 0.7324 |   7.14 KB |
| GetBlogsNormalAsync | 216.4 us | 0.54 us | 0.42 us | 1.2207 |  10.09 KB |
*/

[MemoryDiagnoser]
public class CompiledQueryVsNormal
{
    private readonly BlogContext _context;

    private static readonly Func<BlogContext, int, Task<Blog>> GetBlog
        = EF.CompileAsyncQuery(
            (BlogContext context, int id) => 
            context.Blogs.AsNoTracking().FirstOrDefault(b => b.BlogId == id));

    public CompiledQueryVsNormal()
    {
        _context = new BlogContext();
    }

    [Benchmark]
    public async Task<Blog> GetBlogsAsync()
    {
        return await GetBlog(_context, 4000);
    }

    [Benchmark]
    public async Task<Blog> GetBlogsNormalAsync()
    {
        return await _context.Blogs.AsNoTracking().FirstOrDefaultAsync(b => b.BlogId == 4000);
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.ObjectPool;

namespace Benchmark;

/*
| Method                               | Mean     | Error   | StdDev   | Median   | Gen0   | Allocated |
|------------------------------------- |---------:|--------:|---------:|---------:|-------:|----------:|
| ToListAsyncBenchmark                 | 158.8 us | 3.17 us |  5.64 us | 160.4 us | 1.4648 |  14.38 KB |
| AsAsyncEnumerableWithPooling         | 161.8 us | 3.16 us |  3.64 us | 163.1 us | 1.4648 |  13.94 KB |
| AsAsyncEnumerableWithCompiledPooling | 133.1 us | 3.66 us | 10.78 us | 126.3 us | 1.2207 |     10 KB | 
*/

[MemoryDiagnoser]
public class AsyncEnumerableVsToList
{
    private static readonly Func<BlogContext, IAsyncEnumerable<Blog>> _compiledQuery =
        EF.CompileAsyncQuery((BlogContext context) =>
            context.Blogs.Take(10));

    private readonly BlogContext _dbContext;
    private readonly ObjectPool<List<Blog>> _pool;

    public AsyncEnumerableVsToList()
    {
        _dbContext = new BlogContext();
        var provider = new DefaultObjectPoolProvider();
        _pool = provider.Create<List<Blog>>();
    }

    [Benchmark]
    public async Task<List<Blog>> ToListAsyncBenchmark()
    {
        var blogs = await _dbContext.Blogs.Take(10).ToListAsync();
        return blogs;
    }

    [Benchmark]
    public async Task<List<Blog>> AsAsyncEnumerableWithPooling()
    {
        var list = _pool.Get();
        try
        {
            await foreach (var blog in _dbContext.Blogs.Take(10).AsAsyncEnumerable())
            {
                list.Add(blog);
            }
            return list;
        }
        finally
        {
            list.Clear();
            _pool.Return(list);
        }
    }

    [Benchmark]
    public async Task<List<Blog>> AsAsyncEnumerableWithCompiledPooling()
    {
        var list = _pool.Get();
        try
        {
            await foreach (var blog in _compiledQuery(_dbContext))
            {
                list.Add(blog);
            }
            return list;
        }
        finally
        {
            list.Clear();
            _pool.Return(list);
        }
    }
}

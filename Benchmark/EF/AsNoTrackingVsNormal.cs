﻿using Microsoft.EntityFrameworkCore;

namespace Benchmark;

[MemoryDiagnoser] 
public class AsNoTrackingVsNormal
{
    [Benchmark]
    public List<Blog> Blogs()
    {
        using var context = new BlogContext();
        var blogs = context.Blogs.ToList();

        return blogs;
    }

    [Benchmark]
    public List<Blog> BlogsAsNoTracking()
    {
        using var context = new BlogContext();
        var blogs = context.Blogs.AsNoTracking().ToList();

        return blogs;
    }

    [Benchmark]
    public async Task<List<Blog>> GetBlogsAsync()
    {
        using var context = new BlogContext();
        return await context.Blogs.ToListAsync();
    }
}

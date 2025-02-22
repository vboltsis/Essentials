using System.Collections.Concurrent;
using System.Drawing;

namespace FeatureExamples;

internal class WeakReferenceExample
{
    public static void Example()
    {
        var imageCache = new ImageCache();

        // Load and cache images
        var image1 = LoadImage("http://example.com/image1.jpg");
        var image2 = LoadImage("http://example.com/image2.jpg");

        imageCache.Add("image1", image1);
        imageCache.Add("image2", image2);

        var memoryPressureList = new List<byte[]>(10);
        for (int i = 0; i < 10; i++)
        {
            memoryPressureList.Add(CreateMemoryPressure());
        }

        // Simulate garbage collection
        GC.Collect();
        GC.WaitForPendingFinalizers();

        // Retrieve images from the cache
        if (imageCache.TryGet("image1", out var cachedImage1))
        {
            Console.WriteLine("Image 1 retrieved from cache.");
        }
        else
        {
            Console.WriteLine("Image 1 has been collected.");
        }

        if (imageCache.TryGet("image2", out var cachedImage2))
        {
            Console.WriteLine("Image 2 retrieved from cache.");
        }
        else
        {
            Console.WriteLine("Image 2 has been collected.");
        }

        foreach (var array in memoryPressureList)
        {
            Console.WriteLine(array.Length);
        }
    }

    static Image LoadImage(string url)
    {
        // Simulate loading an image from a URL
        Console.WriteLine($"Loading image from {url}");
        return new Bitmap(100, 100); // Placeholder for actual image loading
    }

    static byte[] CreateMemoryPressure()
    {
        // Allocate a large array to create memory pressure
        var largeArray = new byte[1024 * 1024 * 100]; // 100 MB
        for (int i = 0; i < largeArray.Length; i++)
        {
            largeArray[i] = 0xFF;
        }

        return largeArray;
    }
}


class ImageCache
{
    private readonly ConcurrentDictionary<string, WeakReference<Image>> _cache = new();

    public void Add(string key, Image image)
    {
        _cache[key] = new WeakReference<Image>(image);
    }

    public bool TryGet(string key, out Image image)
    {
        if (_cache.TryGetValue(key, out var weakRef) && weakRef.TryGetTarget(out image))
        {
            return true;
        }

        image = null;
        return false;
    }
}

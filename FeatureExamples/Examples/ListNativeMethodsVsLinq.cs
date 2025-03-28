namespace FeatureExamples;

internal class ListNativeMethodsVsLinq
{
    public static void Example()
    {
        var list = new List<int> { 1, 2, 3, 4, 5 };

        list.Exists(i => i > 0); // replace with list.Any(i => i > 0);
        list.FindAll(i => i > 5); // replace with list.Where(i => i > 5).ToList();
        list.Find(i => i > 0); // replace with list.FirstOrDefault(i => i > 0);
        list.FindIndex(i => i > 0);
        list.FindLast(i => i > 0);
        list.FindLastIndex(i => i > 0);
        list.TrueForAll(i => i > 0); // replace with list.All(i => i > 0);
        var strings = list.ConvertAll(i => i.ToString()); // replace with list.Select(i => i.ToString()).ToList();

    }
}

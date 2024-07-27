using System.Collections.Frozen;

namespace FeatureExamples;

public class FrozenCollectionExample
{
    FrozenDictionary<int, string> frozenDic = 
        new Dictionary<int, string> { { 6, "six" }, { 9, "nine" } }.ToFrozenDictionary();

    public void TestFrozenDictionary()
    {
        var hasSix = frozenDic.ContainsKey(6); // true
        var hasNine = frozenDic.ContainsKey(9); // true
    }
}

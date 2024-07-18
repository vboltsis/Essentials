using Microsoft.Extensions.ObjectPool;

namespace AllDotNetInterfaces.Examples;

public class ResettableExample : IResettable
{
    private static List<int> _emptyList = new();
    public List<int> _list = new();

    public bool TryReset()
    {
        _list.Clear();
        return true;
    }
}

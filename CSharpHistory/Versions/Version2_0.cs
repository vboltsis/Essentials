namespace CSharpHistory;

partial class Version2_0<T> where T : class
{
    public string FirstProperty { get; private set; }
}

partial class Version2_0<T> where T : class
{
    public string SecondProperty { protected get; set; }

    Func<int, int, int> Sum = delegate (int a, int b) { return a + b; };

    public int? Number { get; set; }

    static void CovarianceContravariance()
    {
        // Assignment compatibility.
        string str = "test";
        // An object of a more derived type is assigned to an object of a less derived type.
        object obj = str;

        // Covariance.
        IEnumerable<string> strings = new List<string>();
        // An object that is instantiated with a more derived type argument
        // is assigned to an object instantiated with a less derived type argument.
        // Assignment compatibility is preserved.
        IEnumerable<object> objects = strings;

        // Contravariance.
        // Assume that the following method is in the class:
        static void SetObject(object o) { }
        Action<object> actObject = SetObject;
        // An object that is instantiated with a less derived type argument
        // is assigned to an object instantiated with a more derived type argument.
        // Assignment compatibility is reversed.
        Action<string> actString = actObject;
    }
}

class SomeData
{
    public SomeData() { }

    static IEnumerable<SomeData> CreateSomeDataWithoutYield()
    {
        return new List<SomeData> {
            new SomeData(),
            new SomeData(),
            new SomeData()
        };
    }

    static IEnumerable<SomeData> CreateSomeDataYield()
    {
        yield return new SomeData();
        yield return new SomeData();
        yield return new SomeData();
    }

    /*
     The advantage of using yield is that if the function consuming your data simply needs the first item of the collection,
    the rest of the items won't be created.
    The yield operator allows the creation of items as it is demanded.
     */
}

/*
Generics
Partial types
Anonymous methods
Nullable value types
Iterators
Covariance and contravariance
 */
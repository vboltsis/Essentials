namespace Benchmark;

/*
| Method     | Number | Mean     | Error     | StdDev    | Gen0   | Allocated |
|----------- |------- |---------:|----------:|----------:|-------:|----------:|
| ExistsList | 10     | 1.259 ns | 0.0108 ns | 0.0084 ns |      - |         - |
| AnySet     | 10     | 8.501 ns | 0.1858 ns | 0.1988 ns | 0.0048 |      40 B |
*/

[MemoryDiagnoser] 
[ReturnValueValidator(true)]
public class ExistsVsAny
{
    private List<int> _list;

    [Params(10)]
    public int Number;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _list = new List<int>(Enumerable.Range(1, Number));
    }

    [Benchmark]
    public bool ExistsList()
    {
        return _list.Exists(i => i > 0);
    }

    [Benchmark]
    public bool AnySet()
    {
        return _list.Any(i => i > 0);
    }
}
/*
The casting and subsequent allocation when dealing with IEnumerable or IEnumerable<T> happens
primarily because of how the C# compiler and runtime handle enumerators and
the differences between value types (like structs) and reference types (like classes).

Enumerator Structs in Generic Collections:

Many generic collections in .NET (like List<T>, Dictionary<TKey, TValue>, etc.) implement their GetEnumerator()
method in such a way that it returns a struct.
This is efficient because structs are value types,
meaning they are allocated on the stack (in most cases)
and do not require garbage collection, which reduces memory pressure.

Boxing of Structs:

When you use a foreach loop or manually call GetEnumerator() on a collection,
if that collection directly implements IEnumerable<T> and returns a struct,
the enumerator can be used without causing any heap allocation.
However, when you deal with the collection through an interface like IEnumerable<T>,
the enumerator struct needs to be cast to IEnumerator<T>.
Since IEnumerator<T> is an interface (a reference type),
the struct (a value type) must be boxed to conform to this interface, causing an allocation on the heap.
Boxing is the process of converting a value type (like a struct)
to a reference type (like object or an interface it implements).
This process involves copying the value type data into a new object on the heap,
which incurs an allocation.
Why Casting Causes Allocation:

When you call GetEnumerator() on a type that implements IEnumerable<T> through an interface reference
(like IEnumerable<T> myList = new List<int>();),
the compiler generates code that boxes the enumerator struct, leading to heap allocation.
This happens because the IEnumerable<T> interface requires the enumerator
to be returned as an IEnumerator<T>. Since IEnumerator<T> is a reference type,
if the enumerator is a struct, it must be boxed to fit into the IEnumerator<T> interface.
Implications of Using Interfaces:

If you use the concrete type (like List<T>) directly, the compiler can use the struct enumerator without boxing, avoiding the allocation.
When you use the interface (IEnumerable<T>), the casting and boxing are necessary to maintain type safety and the polymorphic behavior of interfaces.
Summary of Why Casting Causes Allocation
Struct Enumerators: Structs are value types, which are normally allocated on the stack. This is efficient and avoids heap allocations.
Interfaces Require Reference Types: Interfaces like IEnumerator<T> and IEnumerable<T> require reference types because they are designed to be implemented by classes, which are reference types.
Boxing: When a struct (value type) needs to be used as an interface (reference type), the struct is "boxed," meaning it is copied into an object on the heap, causing a memory allocation.
Using Concrete Types Avoids Boxing: When you work directly with concrete collection types like List<T>, you can avoid these allocations because the compiler doesn't need to box the enumerator.
 */
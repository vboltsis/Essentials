using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Frozen;
using System.Collections.ObjectModel;

namespace Types;

public class EnumerableExamples
{
    public static readonly HashSet<int> Set = [1, 2, 3, 4, 5];
    public static readonly FrozenSet<int> FrozenSet = new HashSet<int> { 1, 2, 3, 4, 5 }.ToFrozenSet();

    public static void Test()
    {
        ArrayList arrayList = new ArrayList();
        arrayList.Add(1);
        arrayList.Add("hello world");

        foreach (var item in arrayList)
        {
            Console.WriteLine(item);
        }

        Collection<int> collection = new Collection<int>();

        int[] array = new int[2];
        var humans = new List<IHuman>();
        List<int> list = new List<int>(humans.Count);//set correct initial capacity

        for (int i = 0; i < humans.Count; i++)
        {
            list.Add(i);
        }

        Dictionary<int, string> dictionary = new Dictionary<int, string>(10);
        HashSet<int> hashSet = new HashSet<int>(10);
        Stack<int> stack = new Stack<int>(10);//LIFO
        Queue<int> queue = new Queue<int>(10);//FIFO

        //Thread safe collections
        ConcurrentDictionary<int, string> concurrentDictionary = new ConcurrentDictionary<int, string>();
        ConcurrentQueue<int> concurrentQueue = new ConcurrentQueue<int>();
        ConcurrentStack<int> concurrentStack = new ConcurrentStack<int>();
        ConcurrentBag<int> concurrentBag = new ConcurrentBag<int>();

        queue.Enqueue(1);
        queue.Enqueue(2);

        var result = queue.Dequeue();

        stack.Push(1);
        stack.Push(2);

        var result2 = stack.Pop();

        Console.WriteLine();
    }
}

public class Plumber : Hooman
{
    static List<int> sharedList = [];
    static Random rnd = new();
    public static void WillThrowWhenRun()
    {
        Thread t1 = new Thread(AddItems);
        Thread t2 = new Thread(AddItems);

        t1.Start();
        t2.Start();

        t1.Join();
        t2.Join();
    }

    static void AddItems()
    {
        for (int i = 0; i < 1000; i++)
        {
            int item = rnd.Next(100);
            sharedList.Add(item); // Non-thread-safe operation
        }
    }
}

public class Teacher : Hooman
{

}

public class Hooman : IHuman
{

}

public interface IHuman
{

}

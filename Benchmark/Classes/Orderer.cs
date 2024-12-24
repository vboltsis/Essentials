namespace Benchmark.Classes;

[RankColumn]
[MemoryDiagnoser] 
[ReturnValueValidator(true)]
public class Orderer
{
    private static readonly List<SelectionWithScore> _selections = new List<SelectionWithScore>
    {
        new SelectionWithScore
        {
            Home = 0,
            Away = 0,
            Selection = new Selection
            {
                Name = "0-0",
            }
        },
        new SelectionWithScore
        {
            Home = 1,
            Away = 1,
            Selection = new Selection
            {
                Name = "1-1",
            }
        },
        new SelectionWithScore
        {
            Home = 0,
            Away = 1,
            Selection = new Selection
            {
                Name = "0-1",
            }
        },
        new SelectionWithScore
        {
            Home = 0,
            Away = 2,
            Selection = new Selection
            {
                Name = "0-2",
            }
        },
        new SelectionWithScore
        {
            Home = 1,
            Away = 0,
            Selection = new Selection
            {
                Name = "1-0",
            }
        },
        new SelectionWithScore
        {
            Home = 3,
            Away = 0,
            Selection = new Selection
            {
                Name = "3-0",
            }
        },
        new SelectionWithScore
        {
            Home = 0,
            Away = 3,
            Selection = new Selection
            {
                Name = "0-3",
            }
        },
        new SelectionWithScore
        {
            Home = 1,
            Away = 3,
            Selection = new Selection
            {
                Name = "1-3",
            }
        },
        new SelectionWithScore
        {
            Home = 2,
            Away = 3,
            Selection = new Selection
            {
                Name = "2-3",
            }
        }, 
        new SelectionWithScore
        {
            Home = 3,
            Away = 3,
            Selection = new Selection
            {
                Name = "3-3",
            }
        },
        new SelectionWithScore
        {
            Home = 3,
            Away = 2,
            Selection = new Selection
            {
                Name = "3-2",
            }
        },
        new SelectionWithScore
        {
            Home = 3,
            Away = 1,
            Selection = new Selection
            {
                Name = "3-1",
            }
        },
        new SelectionWithScore
        {
            Home = 2,
            Away = 0,
            Selection = new Selection
            {
                Name = "2-0",
            }
        }
    };

    [Benchmark(Baseline = true)]
    public List<Selection> GetSelectionsWithManyOrder()
    {
        var orderedSelections = new List<Selection>();
        orderedSelections.AddRange(_selections
            .Where(sel => sel.Home > sel.Away)
            .OrderBy(sel => sel.Home)
            .ThenBy(sel => sel.Away)
            .Select(sel => sel.Selection));

        orderedSelections.AddRange(_selections
            .Where(sel => sel.Home == sel.Away)
            .OrderBy(sel => sel.Home)
            .Select(sel => sel.Selection));

        orderedSelections.AddRange(_selections
            .Where(sel => sel.Home < sel.Away)
            .OrderBy(sel => sel.Away)
            .ThenBy(sel => sel.Home)
            .Select(sel => sel.Selection));

        return orderedSelections;
    }

    [Benchmark]
    public List<Selection> GetSelectionsWithOneTupleOrder()
    {
        return _selections
            .Select(s => s.Away < s.Home ? (s.Home * 10 + s.Away, s.Selection)
                : s.Away == s.Home ? (100 + s.Home * 10 + s.Away, s.Selection)
                : (200 + s.Away * 10 + s.Home, s.Selection))
            .OrderBy(s => s.Item1)
            .Select(s => s.Selection)
            .ToList();
    } 
    
    [Benchmark]
    public List<Selection> GetSelectionsWithOneClassOrder()
    {
        return _selections
            .Select(s => s.Away < s.Home ? new SelectionResultClass { Key = s.Home * 10 + s.Away, Selection = s.Selection }
                : s.Away == s.Home ? new SelectionResultClass { Key = 100 + s.Home * 10 + s.Away, Selection = s.Selection }
                : new SelectionResultClass { Key = 200 + s.Away * 10 + s.Home, Selection = s.Selection })
            .OrderBy(s => s.Key)
            .Select(s => s.Selection)
            .ToList();
    } 
    
    [Benchmark]
    public List<Selection> GetSelectionsWithOneStructOrder()
    {
        return _selections
            .Select(s => s.Away < s.Home ? new SelectionResultStruct { Key = s.Home * 10 + s.Away, Selection = s.Selection }
                : s.Away == s.Home ? new SelectionResultStruct { Key = 100 + s.Home * 10 + s.Away, Selection = s.Selection }
                : new SelectionResultStruct { Key = 200 + s.Away * 10 + s.Home, Selection = s.Selection })
            .OrderBy(s => s.Key)
            .Select(s => s.Selection)
            .ToList();
    }
}

public class SelectionWithScore
{
    public Selection Selection { get; set; }
    public int Home { get; set; }
    public int Away { get; set; }
}

public class Selection
{
    public string Name { get; set; }
}

class SelectionResultClass
{
    public int Key { get; set; }
    public Selection Selection { get; set; }
}

struct SelectionResultStruct
{
    public int Key { get; set; }
    public Selection Selection { get; set; }
}
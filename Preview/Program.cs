List<int> numbers = [2, 0, 2, 4];

var fourFind = numbers.Find(i => i == 4);
var fourFirstOrDefault = numbers.FirstOrDefault(i => i == 4);
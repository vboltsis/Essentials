var number = 0;

for (var i = 0; i < 1000; i++)
{
    Task.Run(() =>
    {
        number++;
    });
}

Console.WriteLine(number);
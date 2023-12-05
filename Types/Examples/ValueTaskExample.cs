namespace Types;

public class ValueTaskExample
{
    public async Task<bool> GetSomethingAsync()
    {
        await Task.Delay(1000);

        return true;
    }

    public async ValueTask<bool> GetValueTaskSomethingAsync()
    {
        var random = new Random();

        if (random.Next(0, 2) == 0)
        {
            return false;
        }

        await Task.Delay(1000);

        return true;
    }

    public async ValueTask<bool> Method1()
    {
        return await Method2();
    }

    public async ValueTask<bool> Method2()
    {
        return await Method3();
    }

    public async Task<bool> Method3()
    {
        await Task.Delay(1000);

        return true;
    }
}

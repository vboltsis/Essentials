namespace InterviewExample.Questions;

public class CollatzConjecture
{
    //ODD --> 1,3,5,7,9
    //EVEN --> 2,4,6,8,10
    // IF NUMBER IS ODD -> * 3 + 1
    // IF NUMBER EVEN -> / 2
    public static int GetSteps(long number)
    {
        var steps = 0;

        if (number <= 1)
        {
            return steps;
        }

        while (number != 1)
        {
            steps++;

            if (number % 2 == 0)
            {
                number /= 2;
            }
            else
            {
                number = number * 3 + 1;
            }
        }

        return steps;
    }
}

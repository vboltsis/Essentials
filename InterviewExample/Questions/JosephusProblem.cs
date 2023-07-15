namespace InterviewExample.Questions;

/*
One hundred people are standing in a circle in order from 1 to 100.

No. 1 has a sword. He smites the next person (No. 2) and passes the sword to the next (No. 3).

All people do the same until only one remains.

Which number is the last remaining person?
 */

public class JosephusProblem
{
    public static int GetLastStanding(int numberOfPeople)
    {
        var people = new List<int>(numberOfPeople);
        for (int i = 1; i <= numberOfPeople; i++)
        {
            people.Add(i);
        }

        int index = 0;
        while (people.Count > 1)
        {
            index = (index + 1) % people.Count;
            people.RemoveAt(index);
        }

        return people[0];
    }

    public static int GetLastStandingRecursive(int n)
    {
        if (n == 1)
            return 1;
        else
            // The + 1 and - 1 operations are adjusting the result from 0-based indexing to 1-based indexing.
            return (GetLastStandingRecursive(n - 1) + 1) % n + 1;
    }
}

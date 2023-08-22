//Write a program that, given an array A[] of n numbers and another number x, determines whether or not
//there exist two elements in A[] whose sum is exactly x. 
//Input: arr[] = { 0, -1, 2, -3, 1 }
//        x = -2
//Output: Pair with a given sum -2 is (-3, 1)
//              Valid pair exists
//Explanation:  If we calculate the sum of the output,1 + (-3) = -2
//Input: arr[] = { 1, -2, 1, 0, 5 }
//       x = 0
//Output: No valid pair exists for 0

namespace InterviewExample;

public class CheckIfSumPairExists
{
    //Time Complexity: O(n).
    //As the whole array is needed to be traversed only once.
    public static bool PairExists(int[] intArray, int sum)
    {
        var numberSet = new HashSet<int>(intArray.Length);
        for (int i = 0; i < intArray.Length; ++i)
        {
            int temp = sum - intArray[i];

            if (numberSet.Contains(temp))
            {
                return true;
            }
            numberSet.Add(intArray[i]);
        }

        return false;
    }    
}

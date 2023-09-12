using System;

/*
 * Binary Search *
1. find the midpoint between start(0) and end(leng-1)
2. are any of the three a hit?
3. if not, is the target value located to the left or right of the midpoint?
4. shift starting point, if the target is greater than midpoint value, pick a new midpoint
on the right (or the other way)
5. if we are down to midpoint = start or midpoint = end and target is still not found return -1
6. return -1 when target is out of bounds less than [0] or greater than [^1]
*/


public class Class1
{
	public Class1()
	{

	}

	public static void Main(string[] args)
	{
		//test values
		int[] testValues = { 1, 3, 6, 9, 12 };
		int testTarget = 7;
		int result = BinarySearch(testValues, testTarget);

		if(result != -1)
		{
			Console.WriteLine($"Element {testTarget} found at index {result}");
		}
		else
		{
			Console.WriteLine($"Element {testTarget} not found in the array.");
		}
	}

	public static int BinarySearch(int[] arrValue, int target)
	{
		//initialize starting, end, and midpoint values
		int startIndex = 0;
		int endIndex = arrValue.Length - 1;
		//int midIndex = (endIndex - startIndex) / 2;

		while (startIndex <= endIndex)
		{
			int midIndex = startIndex + (endIndex - startIndex) / 2;

            if (arrValue[midIndex] == target)
            {
				//target found
				return midIndex;
            }
            if (arrValue[midIndex] > target)
			{
				//go left
				endIndex = midIndex - 1;
			}
			else
			{
				//go right
				startIndex = midIndex + 1;
			}
		}
		return -1;
		
	}
}

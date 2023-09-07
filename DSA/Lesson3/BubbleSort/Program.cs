class Program
{
    static void Main(string[] args)
    {
        int[] examScores = { 95, 54, 87, 82, 94, 22, 75 };

        Console.WriteLine("Here are the exam scores before sorting: ");
        PrintArray(examScores);

        Console.WriteLine("Here are the exam scores after the bubble sort");
        BubbleSort(examScores);

        Console.WriteLine("Space complexity is O(1) for the bubble sort");
    }

    static void BubbleSort(int[] arr)
    {
        int n = arr.Length;

        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - 1; j++)
            {
                if (arr[j] > arr[j + 1])
                {
                    //Swap the elements
                    int temp = arr[j];
                    arr[j] = arr[j + 1];
                    arr[j + 1] = temp;
                }
            }
        }
    }

    static void PrintArray(int[] arr)
    {
        foreach (var score in arr)
        {
            Console.Write(score + " ");
        }
        Console.WriteLine();
    }
}


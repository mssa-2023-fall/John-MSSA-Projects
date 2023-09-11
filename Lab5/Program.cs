using System;
using System.Collections.Generic;

// Define a Node Class
public class Node
{
    public int Data { get; set; }
    public Node Next { get; set; }

    public Node(int data)
    {
        Data = data;
        Next = null;
    }
}

// Define a Stack Class
public class Stack
{
    private Node top;

    public bool IsEmpty()
    {
        return top == null;
    }

    public void Push(int data)
    {
        Node newNode = new Node(data);
        newNode.Next = top;
        top = newNode;
    }

    public int Pop()
    {
        if (IsEmpty())
        {
            throw new InvalidOperationException("Stack is empty.");
        }
        int result = top.Data;
        top = top.Next;
        return result;
    }

    public void RemoveEverySecond()
    {
        if (top == null || top.Next == null) return;

        Node current = top;
        while (current != null && current.Next != null)
        {
            current.Next = current.Next.Next;
            if (current.Next != null)
                current = current.Next;
        }
    }

    public void PrintStack()
    {
        Node current = top;
        while (current != null)
        {
            Console.WriteLine(current.Data);
            current = current.Next;
        }
        Console.WriteLine("-------------");
    }
}

public class Program
{
    public static void Main()
    {
        //Lab 1 and 3
        // Stack Demonstration
        Console.WriteLine("Stack Demonstration:");
        // Create a Stack Object
        Stack stack = new Stack();

        // Push Elements
        for (int i = 1; i <= 10; i++)
        {
            stack.Push(i);
        }

        // Print Initial Stack
        Console.WriteLine("Initial Stack:");
        stack.PrintStack();

        // Remove Every Second Element
        stack.RemoveEverySecond();

        // Print Modified Stack
        Console.WriteLine("After Removing Every Second Element:");
        stack.PrintStack();

        // Queue Demonstration
        Console.WriteLine("Queue Demonstration:");
        // Create a Queue Object
        Queue<int> callerIds = new Queue<int>();

        //Lab 2
        // Enqueue Caller IDs
        callerIds.Enqueue(101);
        callerIds.Enqueue(102);
        callerIds.Enqueue(103);
        callerIds.Enqueue(104);
        callerIds.Enqueue(105);

        // Iterate Over the Queue
        Console.WriteLine("List of Caller IDs in the Queue:");
        foreach (int callerId in callerIds)
        {
            Console.WriteLine(callerId);
        }
    }
}

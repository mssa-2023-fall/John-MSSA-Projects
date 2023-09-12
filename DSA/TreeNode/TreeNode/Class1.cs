using System;
using System.Collections.Generic;

namespace TreeNode
{
    public class Class1
    {

    }

    

public class Node
    {
        public int Value { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }

        public Node(int value)
        {
            this.Value = value;
            this.Left = null;
            this.Right = null;
        }
    }

    public class BinaryTree
    {
        public Node Root { get; set; }

        public BinaryTree()
        {
            Root = null;
        }

        public void Insert(int value)
        {
            if (Root == null)
            {
                Root = new Node(value);
                return;
            }

            Queue<Node> nodes = new Queue<Node>();
            nodes.Enqueue(Root);

            while (nodes.Count > 0)
            {
                Node current = nodes.Dequeue();

                if (current.Left == null)
                {
                    current.Left = new Node(value);
                    break;
                }
                else
                {
                    nodes.Enqueue(current.Left);
                }

                if (current.Right == null)
                {
                    current.Right = new Node(value);
                    break;
                }
                else
                {
                    nodes.Enqueue(current.Right);
                }
            }
        }

        public void PrintTree()
        {
            PrintPreOrder(Root);
        }

        private void PrintPreOrder(Node node)
        {
            if (node == null)
                return;

            Console.WriteLine(node.Value);   // Visit the node
            PrintPreOrder(node.Left);       // Visit left subtree
            PrintPreOrder(node.Right);      // Visit right subtree
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            BinaryTree tree = new BinaryTree();

            tree.Insert(1);
            tree.Insert(2);
            tree.Insert(3);
            tree.Insert(4);
            tree.Insert(5);
            tree.Insert(6);
            tree.Insert(7);

            tree.PrintTree(); // This will print: 1 2 4 5 3 6 7
        }
    }
}
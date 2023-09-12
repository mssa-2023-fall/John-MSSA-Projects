using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TreeNode;

namespace BinaryTreeTest
{
    [TestClass]
    public class BinaryTreeTests
    {
        [TestMethod]
        public void InsertingSingleNode_ShouldHaveCorrectValue()
        {
            BinaryTree tree = new BinaryTree();
            tree.Insert(5);
            Assert.AreEqual(5, tree.Root.Value);
        }

        [TestMethod]
        public void InsertingMultipleNodes_ShouldFollowBFSOrder()
        {
            BinaryTree tree = new BinaryTree();

            tree.Insert(1);
            tree.Insert(2);
            tree.Insert(3);
            tree.Insert(4);
            tree.Insert(5);

            Assert.AreEqual(1, tree.Root.Value);
            Assert.AreEqual(2, tree.Root.Left.Value);
            Assert.AreEqual(3, tree.Root.Right.Value);
            Assert.AreEqual(4, tree.Root.Left.Left.Value);
            Assert.AreEqual(5, tree.Root.Left.Right.Value);
        }

        [TestMethod]
        public void PrintTree_ShouldReturnCorrectPreOrderTraversal()
        {
            BinaryTree tree = new BinaryTree();
            tree.Insert(1);
            tree.Insert(2);
            tree.Insert(3);
            tree.Insert(4);
            tree.Insert(5);

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                tree.PrintTree();

                string expected = string.Join(Environment.NewLine, new[] { "1", "2", "4", "5", "3" }) + Environment.NewLine;
                Assert.AreEqual(expected, sw.ToString());
            }
        }
    }
}
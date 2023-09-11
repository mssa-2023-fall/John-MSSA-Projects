namespace Lab5_Test
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void TestStackPushAndPop()
        {
            Stack stack = new Stack();
            stack.Push(1);
            stack.Push(2);
            stack.Push(3);

            Assert.AreEqual(3, stack.Pop());
            Assert.AreEqual(2, stack.Pop());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestStackPopEmpty()
        {
            Stack stack = new Stack();
            stack.Pop();
        }

        [TestMethod]
        public void TestRemoveEverySecondElement()
        {
            Stack stack = new Stack();
            for (int i = 1; i <= 10; i++)
            {
                stack.Push(i);
            }

            stack.RemoveEverySecond();

            Assert.AreEqual(9, stack.Pop());
        }

        [TestMethod]
        public void TestQueueEnqueueAndDequeue()
        {
            Queue<int> callerIds = new Queue<int>();
            callerIds.Enqueue(101);
            callerIds.Enqueue(102);

            Assert.AreEqual(101, callerIds.Dequeue());
            Assert.AreEqual(102, callerIds.Dequeue());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestQueueDequeueEmpty()
        {
            Queue<int> callerIds = new Queue<int>();
            callerIds.Dequeue();
        }
    }
}
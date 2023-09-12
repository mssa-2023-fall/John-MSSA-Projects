namespace SearchProjectTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Test_BinarySearch_FoundElement()
        {
            int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            int target = 7;
            int expectedPosition = 6;

            int result = Searcher.BinarySearch(numbers, target);

            Assert.AreEqual(expectedPosition, result);
        }

        [TestMethod]
        public void Test_BinarySearch_NotFoundElement()
        {
            int[] numbers = { 1, 2, 3,, 4, 5, 6, 7, 8, 9, 10 };
            target = 11;
            int result = Searcher.BinarySearch(numbers, target);

            Assert.AreEqual(-1, result);
        }

        [TestMethod]
        public void Test_BinarySearch_EmptyArray()
        {
            int[] numbers = { };
            int target = 5;

            int result = Searcher.BinarySearch(numbers, target);

            Assert.AreEqual(-1, result);
        }
    }
}
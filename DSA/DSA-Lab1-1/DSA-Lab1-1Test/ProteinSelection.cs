using DSA_Lab1_1;

namespace DSA_Lab1_1Test

{
    [TestClass]
    public class ProteinSelection
    {
        RestaurantMenu menu;
        [TestInitialize]
        public void TestSetup()
        {
            menu = new RestaurantMenu();
        }

        [TestMethod]
        [DataRow("Beef", "hamburger")]
        [DataRow("Pepperoni", "pizza")]
        [DataRow("Tofu", "Tofu Fried Rice")]
        [DataRow("BEEF", "hamburger")]
        [DataRow("PEPPERoni", "pizza")]
        [DataRow("tofu", "Tofu Fried Rice")]
        public void TestWithExpectedProteinTypeShouldReturnCorrespondingDishes(string proteinChoices, string menuItem)
        {
            string dish = menu.GetDishRecommendation(proteinChoices);
            Assert.AreEqual(menuItem, dish, true);
        }

        [TestMethod]
        public void TestWith_UNExpected_ProteinTypeShouldReturnMessageProteinNotAvailable()
        {
            string dish = menu.GetDishRecommendation("fish");
            Assert.AreEqual("That protein is not available.", dish, true);
        }
        public void TestConsoleOutput()
        {
            var p = new Process();
            p.StartInfo = new ProcessStartInfo("C:\\Users\\victor\\source\\repos\\Chinjila\\DSA\\DSA-Lab1-1\\RestaurantApp\\bin\\Debug\\net6.0\\RestaurantApp.exe");
            var inputStream = new StreamWriter(new MemoryStream(new byte[100]));
            p.StartInfo.RedirectStandardInput = true;
            var outputStream = p.StandardOutput;
            var outputLine = outputStream.ReadLine();

            Assert.AreEqual(outputLine, "Hello, World!", true);
        }
    }
}
namespace MortgageCalc2
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var mortgage = new Mortgage
            {
                Duration = Duration.Fifteen,
                InterestRate = 0.035,
                PrincipalAmount = 200000m,
                OriginationDate = DateTime.Today
            };

            // Add some payments
            mortgage.Payments.Add(new Payment { /* initialize payment */ });
            // ...

            var payoffDate = mortgage.GetPayoffDate();
            Assert.AreEqual(DateTime.Today.AddYears(15), payoffDate);
        }
    }
}
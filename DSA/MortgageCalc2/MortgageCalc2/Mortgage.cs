using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MortgageCalc2
{
    public enum Duration
    {
        Ten = 10,
        Fifteen = 15,
        Thirty = 30
    }
    public class Mortgage
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Mortgage Creation:");

            // Get Duration
            Console.WriteLine("Select Duration: 1) 10 years 2) 15 years 3) 30 years");
            int choice = int.Parse(Console.ReadLine());
            Duration duration = choice switch
            {
                1 => Duration.Ten,
                2 => Duration.Fifteen,
                3 => Duration.Thirty,
                _ => throw new InvalidOperationException("Invalid choice")
            };

            // Get Interest Rate
            Console.WriteLine("Enter Interest Rate (e.g., 0.035 for 3.5%):");
            double interestRate = double.Parse(Console.ReadLine());

            // Get Principal Amount
            Console.WriteLine("Enter Principal Amount:");
            decimal principalAmount = decimal.Parse(Console.ReadLine());

            // Get Origination Date
            Console.WriteLine("Enter Origination Date (YYYY-MM-DD):");
            DateTime originationDate = DateTime.Parse(Console.ReadLine());

            // Create Mortgage
            Mortgage mortgage = new Mortgage
            {
                Duration = duration,
                InterestRate = interestRate,
                PrincipalAmount = principalAmount,
                OriginationDate = originationDate
            };

            Console.WriteLine("Mortgage Created!");
            Console.WriteLine($"Payoff Date: {mortgage.GetPayoffDate().ToShortDateString()}");
            Console.WriteLine($"Monthly Payment: {mortgage.MonthlyPayment:C2}");

            // Option to save mortgage object as JSON
            Console.WriteLine("Do you want to save this mortgage to a JSON file? (yes/no)");
            if (Console.ReadLine().Trim().ToLower() == "yes")
            {
                string filePath = "mortgage.json";
                mortgage.SaveToFile(filePath);
                Console.WriteLine($"Mortgage saved to {filePath}");
            }

            // Keep the console open until a key is pressed
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
        public Duration Duration { get; set; }
        public double InterestRate { get; set; }
        public decimal PrincipalAmount { get; set; }
        public DateTime OriginationDate { get; set; }
        public List<Payment> Payments { get; private set; } = new List<Payment>();

        public Mortgage() { }

        public DateTime GetPayoffDate() => OriginationDate.AddYears((int)Duration);

        public decimal MonthlyInterestRate => (decimal)InterestRate / 12;
        public int TotalPayments => (int)Duration * 12;

        public decimal MonthlyPayment =>
            PrincipalAmount * MonthlyInterestRate * (decimal)Math.Pow((1 + (double)MonthlyInterestRate), TotalPayments) /
            ((decimal)Math.Pow((1 + (double)MonthlyInterestRate), TotalPayments) - 1);

        public decimal RemainingPrincipalAtDate(DateTime date)
        {
            int monthsElapsed = (date.Year - OriginationDate.Year) * 12 + date.Month - OriginationDate.Month;
            if (monthsElapsed <= 0) return PrincipalAmount;
            if (monthsElapsed >= TotalPayments) return 0;

            decimal remainingPrincipal = PrincipalAmount;
            for (int i = 0; i < monthsElapsed; i++)
            {
                decimal interestForMonth = remainingPrincipal * MonthlyInterestRate;
                decimal principalForMonth = MonthlyPayment - interestForMonth;
                remainingPrincipal -= principalForMonth;
            }
            return remainingPrincipal;
        }

        public decimal InterestPaidAtDate(DateTime date)
        {
            int monthsElapsed = (date.Year - OriginationDate.Year) * 12 + date.Month - OriginationDate.Month;
            if (monthsElapsed <= 0) return 0;

            decimal totalInterestPaid = 0;
            decimal remainingPrincipal = PrincipalAmount;
            for (int i = 0; i < monthsElapsed; i++)
            {
                decimal interestForMonth = remainingPrincipal * MonthlyInterestRate;
                totalInterestPaid += interestForMonth;
                decimal principalForMonth = MonthlyPayment - interestForMonth;
                remainingPrincipal -= principalForMonth;
            }
            return totalInterestPaid;
        }

        public List<Payment> GetAmortizationSchedule()
        {
            List<Payment> schedule = new List<Payment>();
            decimal remainingPrincipal = PrincipalAmount;
            for (int i = 0; i < TotalPayments; i++)
            {
                decimal interestForMonth = remainingPrincipal * MonthlyInterestRate;
                decimal principalForMonth = MonthlyPayment - interestForMonth;
                remainingPrincipal -= principalForMonth;

                Payment payment = new Payment
                {
                    Date = OriginationDate.AddMonths(i + 1),
                    PrincipalAmount = principalForMonth,
                    InterestAmount = interestForMonth
                };
                schedule.Add(payment);
            }
            return schedule;
        }



        public string SerializeToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        public static Mortgage DeserializeFromJson(string json)
        {
            return JsonSerializer.Deserialize<Mortgage>(json);
        }

        public void SaveToFile(string filePath)
        {
            File.WriteAllText(filePath, SerializeToJson());
        }
    }

    public class Payment
    {
        public DateTime Date { get; set; }
        public decimal PrincipalAmount { get; set; }
        public decimal InterestAmount { get; set; }
        public decimal Total => PrincipalAmount + InterestAmount;
    }
}

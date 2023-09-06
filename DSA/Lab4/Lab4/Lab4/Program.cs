using System;
using System.Text;

public class Program
{
    public static void Main(string[] args)
    {
        // Define Item Prices
        const double applePrice = 1.99;
        const double bananaPrice = 2.49;
        const double orangePrice = 0.99;
        const double peachPrice = 1.29;
        const double watermelonPrice = 3.99;

        // Initialize Total Price
        double totalPrice = 0;

        StringBuilder receipt = new StringBuilder();
        receipt.AppendLine("Receipt:");

        // Create a Loop for Scanning Items
        while (true)
        {
            Console.WriteLine("Enter the name of the scanned item (or 'done' to finish):");
            string item = Console.ReadLine();

            if (item == "done")
                break;

            double itemPrice = 0;
            switch (item.ToLower())
            {
                case "apple":
                    itemPrice = applePrice;
                    receipt.AppendLine($"Apple - ${itemPrice}");
                    break;
                case "banana":
                    itemPrice = bananaPrice;
                    receipt.AppendLine($"Banana - ${itemPrice}");
                    break;
                case "orange":
                    itemPrice = orangePrice;
                    receipt.AppendLine($"Orange - ${itemPrice}");
                    break;
                case "peach":
                    itemPrice = peachPrice;
                    receipt.AppendLine($"Peach - ${itemPrice}");
                    break;
                case "watermelon":
                    itemPrice = watermelonPrice;
                    receipt.AppendLine($"Watermelon - ${itemPrice}");
                    break;
                default:
                    Console.WriteLine("Unknown item. Try again.");
                    continue;
            }

            // Update total price
            totalPrice += itemPrice;
        }

        // Print the Receipt
        receipt.AppendLine("--------------------------");
        receipt.AppendLine($"Total Price: ${totalPrice:F2}");
        Console.WriteLine(receipt);
    }
}
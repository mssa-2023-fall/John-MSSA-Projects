

namespace DSA_Lab1_1
{
    public class RestaurantMenu
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Please enter your choice of protein:");
            string userOrder = Console.ReadLine();
            string rec = GetDishRecommendation(userOrder);
        }
        public string GetDishRecommendation(string proteinChoices)
        {
            switch (proteinChoices)
            {
                case "beef":
                    return "Hamburger";
                case "pepperoni":
                    return "Pizza";
                case "tofu":
                    return "Tofu fried rice";
                default:
                    return "That protein is not available.";
            }
        }
    }
    
}
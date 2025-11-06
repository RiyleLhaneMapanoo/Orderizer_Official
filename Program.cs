using Microsoft.Extensions.Configuration;
using Ord_Business;
using Ord_Common;
using Ord_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;



namespace Orderizer_Official
{
    internal class Program
    {

        static void Main(string[] args)
        {


           
         
         

            var configuration = new ConfigurationBuilder()
      .SetBasePath(Directory.GetCurrentDirectory())
      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
      .Build();

            EmailService emailServ = new EmailService(configuration);
            Ord_BusinessServ businessServ = new Ord_BusinessServ(emailServ);

            while (true)
            {
                Console.WriteLine("\n=== ORDERIZER MENU ===");
                Console.WriteLine("1. Add Item");
                Console.WriteLine("2. View All Items");
                Console.WriteLine("3. Update Item");
                Console.WriteLine("4. Delete Item");
                Console.WriteLine("5. Exit");
                Console.WriteLine("6. Send Best Deals Summary ✉️");
                Console.Write("Choose option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddItem(businessServ);
                        break;
                    case "2":
                        ViewItems(businessServ);
                        break;
                    case "3":
                        UpdateItem(businessServ);
                        break;
                    case "4":
                        DeleteItem(businessServ);
                        break;
                    case "5":
                        return;
                    case "6":
                        SendBestDealsEmail(businessServ,emailServ);
                        break;
                    default:
                        Console.WriteLine("Invalid choice!");
                        break;
                }
            }
        }


        static void SendBestDealsEmail(Ord_BusinessServ businessServ, EmailService emailServ)
        {
            List<items> allItems = businessServ.GetAllItems();
            Console.WriteLine("Sending email...");
            emailServ.SendEmail("receiver@gmail.com","test", allItems);
            Console.WriteLine("✅ Email sent successfully!");
        }



        static void AddItem(Ord_BusinessServ business)
        {
            items newItem = new items();

            Console.Write("Enter item name: ");
            newItem.ItemName = Console.ReadLine();

            Console.Write("Enter item description: ");
            newItem.Description = Console.ReadLine();

            Console.Write("Enter item status (e.g., Out of Stock, Available): ");
            newItem.ItemStatus = Console.ReadLine();

            Console.Write("How many platforms does this item have?");

            int platformCount;
            while (!int.TryParse(Console.ReadLine(), out platformCount) || platformCount <= 0)
            {
                Console.Write("❌ Invalid number. Please enter a valid platform count: ");
            }

            newItem.Platforms = new List<platform>();

            for (int i = 0; i < platformCount; i++)
            {
                Console.WriteLine($"\n--- Platform #{i + 1} ---");

                platform newPlatform = new platform();
                newPlatform.PlatformId = i + 1;

                Console.Write("Platform name: ");
                newPlatform.PlatformName = Console.ReadLine();

                Console.Write("Shop name: ");
                newPlatform.ShopName = Console.ReadLine();

                Console.Write("Platform price:");
                decimal price;
                while (!decimal.TryParse(Console.ReadLine(), out price) || price < 0)
                {
                    Console.Write("❌ Invalid price. Please enter a valid number: ₱");
                }
                newPlatform.Price = price;


                newItem.Platforms.Add(newPlatform);
            }

            business.AddNewItem(newItem);

            Console.WriteLine("\n✅ Item and platform(s) added successfully!");
        }


        static void ViewItems(Ord_BusinessServ business)
        {
            List<items> allItems = business.GetAllItems();

            if (allItems == null || !allItems.Any())
            {
                Console.WriteLine("No items found.");
                return;
            }

            Console.WriteLine("\n=== ALL ITEMS IN ORDERIZER ===");

            foreach (items item in allItems)
            {
                Console.WriteLine($"\n[{item.ItemId}] {item.ItemName}");
                Console.WriteLine($"   Description: {item.Description}");
                Console.WriteLine($"   Status: {item.ItemStatus}");
                Console.WriteLine("   Platforms:");

                if (item.Platforms != null && item.Platforms.Any())
                {
                    foreach (platform p in item.Platforms)
                    {
                        Console.WriteLine($"     • {p.PlatformName,-15} | Shop: {p.ShopName,-15} | Price: ₱{p.Price:N2}");
                    }

                    var bestPrice = item.Platforms.Min(pl => pl.Price);
                    var cheapest = item.Platforms.First(pl => pl.Price == bestPrice);
                    Console.WriteLine($"   👉 Best Deal: {cheapest.PlatformName} ({cheapest.ShopName}) — ₱{cheapest.Price:N2}");
                }
                else
                {
                    Console.WriteLine("     (No platforms listed)");
                }
            }
        }


        static void UpdateItem(Ord_BusinessServ business)
        {
            List<items> allItems = business.GetAllItems();

            if (allItems == null || !allItems.Any())
            {
                Console.WriteLine("No items available to update.");
                return;
            }

            Console.WriteLine("\n=== UPDATE MENU ===");
            Console.WriteLine("1. Update Item Details");
            Console.WriteLine("2. Update Item Platform and Price");
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    UpdateItemDetails(business, allItems);
                    break;

                case "2":
                    UpdateItemPlatformAndPrice(business, allItems);
                    break;

                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }

        static void UpdateItemDetails(Ord_BusinessServ business, List<items> allItems)
        {
            Console.WriteLine("\n=== AVAILABLE ITEMS ===");
            foreach (items item in allItems)
            {
                Console.WriteLine($"{item.ItemId} - {item.ItemName}");
            }

            Console.Write("\nChoose an Item ID to update: ");
            if (!int.TryParse(Console.ReadLine(), out int itemId))
            {
                Console.WriteLine("❌ Invalid input.");
                return;
            }

            items selectedItem = business.GetItemById(itemId);
            if (selectedItem == null)
            {
                Console.WriteLine("❌ Item not found.");
                return;
            }

            Console.WriteLine($"\nEditing Item: {selectedItem.ItemName}");
            Console.WriteLine("Leave blank to keep the current value.");

            Console.Write($"New item name (current: {selectedItem.ItemName}): ");
            string newName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newName))
                selectedItem.ItemName = newName;

            Console.Write($"New description (current: {selectedItem.Description}): ");
            string newDesc = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newDesc))
                selectedItem.Description = newDesc;

            Console.Write($"New status (current: {selectedItem.ItemStatus}): ");
            string newStatus = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newStatus))
                selectedItem.ItemStatus = newStatus;

            business.UpdateItem(selectedItem);
            Console.WriteLine("\n✅ Item details updated successfully!");
        }

        static void UpdateItemPlatformAndPrice(Ord_BusinessServ business, List<items> allItems)
        {
            Console.WriteLine("\n=== AVAILABLE ITEMS ===");
            foreach (items item in allItems)
            {
                Console.WriteLine($"{item.ItemId} - {item.ItemName}");
            }

            Console.Write("\nChoose an Item ID to view its platforms: ");
            if (!int.TryParse(Console.ReadLine(), out int itemId))
            {
                Console.WriteLine("❌ Invalid input.");
                return;
            }

            items selectedItem = business.GetItemById(itemId);
            if (selectedItem == null)
            {
                Console.WriteLine("❌ Item not found.");
                return;
            }

            if (selectedItem.Platforms == null || !selectedItem.Platforms.Any())
            {
                Console.WriteLine("\n⚠️ This item has no platforms to update.");
                return;
            }

            Console.WriteLine($"\n=== PLATFORMS FOR {selectedItem.ItemName} ===");
            for (int i = 0; i < selectedItem.Platforms.Count; i++)
            {
                platform p = selectedItem.Platforms[i];
                Console.WriteLine($"{i + 1}. {p.PlatformName} | Shop: {p.ShopName} | Price: ₱{p.Price:N2}");
            }

            Console.Write("\nChoose a platform number to update: ");
            if (!int.TryParse(Console.ReadLine(), out int platformIndex) ||
                platformIndex < 1 || platformIndex > selectedItem.Platforms.Count)
            {
                Console.WriteLine("❌ Invalid platform selection.");
                return;
            }

            platform selectedPlatform = selectedItem.Platforms[platformIndex - 1];

            Console.WriteLine($"\nEditing Platform: {selectedPlatform.PlatformName}");
            Console.WriteLine("Leave blank to keep current values.");

            Console.Write($"New platform name (current: {selectedPlatform.PlatformName}): ");
            string newPlatName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newPlatName))
                selectedPlatform.PlatformName = newPlatName;

            Console.Write($"New shop name (current: {selectedPlatform.ShopName}): ");
            string newShop = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newShop))
                selectedPlatform.ShopName = newShop;

            Console.Write($"New price (current: ₱{selectedPlatform.Price:N2}): ₱");
            string newPriceInput = Console.ReadLine();
            if (decimal.TryParse(newPriceInput, out decimal newPrice))
                selectedPlatform.Price = newPrice;

            business.UpdateItem(selectedItem);
            Console.WriteLine("\n✅ Platform details updated successfully!");
        }

        static void DeleteItem(Ord_BusinessServ business)
        {
            Console.Write("Enter item ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("❌ Invalid ID.");
                return;
            }

            items item = business.GetItemById(id);
            if (item == null)
            {
                Console.WriteLine("❌ Item not found!");
                return;
            }

            Console.WriteLine($"\nYou are about to delete '{item.ItemName}'. Are you sure? (y/n): ");
            string confirm = Console.ReadLine().ToLower();

            if (confirm == "y")
            {
                business.DeleteItem(id);
                Console.WriteLine("🗑️ Item deleted successfully!");
            }
            else
            {
                Console.WriteLine("❎ Deletion cancelled.");
            }
        }



    }
}

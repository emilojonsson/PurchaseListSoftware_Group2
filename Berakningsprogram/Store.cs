using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Runtime.Serialization;

namespace purchase_list_group2
{
    [DataContract]
    public class Store
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public Guid StoreID { get; set; } = Guid.NewGuid();
        [DataMember]
        public List<StoreItem> Inventory { get; set; } = new List<StoreItem>();
        [DataMember]
        public List<Dictionary<string, object>> Campaigns { get; set; } = new List<Dictionary<string, object>>();
        [DataMember]
        public List<ShoppingListItem> PurchasedItems { get; set; } = new List<ShoppingListItem>();

        public Store(string name)
        {
            Name = name;
        }
        public static void removeStoreAndItems(List<Store> storeList)
        {
            Console.WriteLine("Select the number of the store that you wish to remove:");
            int storeIndex = 1;
            foreach (Store store in storeList)
            {
                Console.WriteLine($"{storeIndex}. {store.Name}");
                storeIndex++;
            }

            try
            {
                int selectInt = int.Parse(Console.ReadLine()) - 1;
                Store storeToRemove = storeList[selectInt];
                Console.WriteLine($"\nNote: Are you sure you want to delete {storeToRemove.Name} and all its items?\n Press * then [Enter] to confirm.\n Press 0 then [Enter] to abort.");
                string confirmation = Console.ReadLine();
                if (confirmation == "*")
                {
                    storeList.Remove(storeToRemove);
                    Console.WriteLine($"{storeToRemove.Name}, and all its items have been removed.");
                }
                else if (confirmation == "0")
                {
                    Console.WriteLine("No store was removed.");
                }
                else
                {
                    Console.WriteLine("Invalid input. No store was removed.");
                }
                Console.WriteLine("Press Enter to continue.");
            }
            catch (FormatException)
            {
                Console.WriteLine("The input was not in correct format, no store was removed.");
                Console.WriteLine("Press Enter to continue.");
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("The input could not relate to any store, no store was removed.");
                Console.WriteLine("Press Enter to continue.");
            }
        }


        public void addStoreItem()
        {
            while (true)
            {
                Console.WriteLine("\nType the item details you are adding:");
                Console.Write("Item name: ");
                string name = Console.ReadLine();
                Console.Write("Category type: ");
                string category = Console.ReadLine();
                Console.Write("Price per item: ");
                double pricePerItem;
                while (!double.TryParse(Console.ReadLine(), out pricePerItem))
                {
                    Console.WriteLine("Invalid input. Enter a valid number.");
                }
                Console.WriteLine("\nSelect the unit of the item:");
                Console.WriteLine($"1. Pcs");
                Console.WriteLine($"2. Litres");
                Console.WriteLine($"3. Gram");
                ShoppingListItem.UnitEnum unit;
                while (!Enum.TryParse(Console.ReadLine(), out unit))
                {
                    Console.WriteLine("Invalid selection. Try again.");
                    Console.WriteLine($"1. Pcs");
                    Console.WriteLine($"2. Litres");
                    Console.WriteLine($"3. Gram");
                }
                int intUnit = (int)unit - 1;
                unit = (ShoppingListItem.UnitEnum)intUnit; 
                StoreItem storeItem = new StoreItem(name, category, pricePerItem, unit);
                Inventory.Add(storeItem);
                Console.WriteLine($"\nSummary details of item added:\nItem name: {storeItem.Name}\nCategory: {storeItem.Category}\nStore Name: {Name}\nUnit type: {storeItem.Unit}");

                Console.WriteLine("\nPress 1 to add another item or [ENTER] to exit.");
                if (Console.ReadLine() != "1")
                {
                    break;
                }
            }
        }

        public void editStoreItem()
        {
            if (viewStoreItems())
            {
                Console.WriteLine("\nWhat item number do you want to edit? [Press 0 to Exit]");
                int selectInt;
                while (!int.TryParse(Console.ReadLine(), out selectInt) || selectInt < 0 || selectInt > Inventory.Count)
                {
                    Console.WriteLine("Invalid item number. Try again.");
                }
                if (selectInt == 0)
                {
                    return;
                }
                StoreItem storeItem = Inventory[selectInt - 1];
                Console.WriteLine($"\nThe current item in the inventory is {storeItem.Name}. Enter the new item details:");
                Console.Write("New name: ");
                string newName = Console.ReadLine();
                Console.Write("New category: ");
                string newCategory = Console.ReadLine();
                Console.Write("New price: ");
                double newPrice = double.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.WriteLine("Select the unit of the item:");
                Console.WriteLine($"1. Pcs");
                Console.WriteLine($"2. Litres");
                Console.WriteLine($"3. Gram");
                ShoppingListItem.UnitEnum newUnit;
                while (!Enum.TryParse(Console.ReadLine(), out newUnit))
                {
                    Console.WriteLine("Invalid selection. Try again.");
                    Console.WriteLine("Select the unit of the item:");
                    Console.WriteLine($"1. Pcs");
                    Console.WriteLine($"2. Litres");
                    Console.WriteLine($"3. Gram");
                }
                int intUnit = (int)newUnit - 1;
                newUnit = (ShoppingListItem.UnitEnum)intUnit;
                storeItem.Name = newName;
                storeItem.Category = newCategory;
                storeItem.Price = newPrice;
                storeItem.Unit = newUnit;
                Console.WriteLine($"\n{storeItem.Name} in {Name} inventory was updated. Press any key to continue...");
            }
                
        }

        public void addCampaign()
        {
            Console.WriteLine("Type the campaign:");
            string name = Console.ReadLine();
            Console.WriteLine("Enter the discount % use a comma if needed e.g.[00,00]:");
            double discount;
            while (!double.TryParse(Console.ReadLine(), out discount))
            {
                Console.WriteLine("Invalid input. Enter a valid number.");
            }
            DateTime startDate;
            do
            {
                Console.WriteLine("Enter the start date (yyyy-mm-dd):");
            } while (!DateTime.TryParse(Console.ReadLine(), out startDate));
            DateTime endDate;
            do
            {
                Console.WriteLine("Enter the end date (yyyy-mm-dd):");
            } while (!DateTime.TryParse(Console.ReadLine(), out endDate) || endDate <= startDate);
            List<StoreItem> sortedInventory = Inventory.OrderBy(item => item.Name).ToList();// Sorts inventory|alphabetically|name
            Console.WriteLine("Select the item numbers to include in the campaign, separated by commas:");// Display inventory items|numbers
            for (int i = 0; i < sortedInventory.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {sortedInventory[i].Name}");
            }
            string input = Console.ReadLine();// Get user's input
            List<int> selectedNumbers = new List<int>();
            while (!string.IsNullOrEmpty(input))
            {
                if (int.TryParse(input.Trim(), out int number) && number >= 1 && number <= sortedInventory.Count)
                {
                    selectedNumbers.Add(number);
                }
                Console.WriteLine($"\n#{input}. was selected. Press [Enter] to continue..");
                input = Console.ReadLine();
            }
            List<Guid> selectedIDs = selectedNumbers.Select(number => sortedInventory[number - 1].ItemID).ToList();// Map the selected numbers to item IDs
            // Shows selected items|confirmation|edit
            Console.WriteLine("You have selected the following items:");
            double totalDiscountedPrice = 0.0;
            foreach (Guid id in selectedIDs)
            {
                StoreItem item = Inventory.Find(i => i.ItemID == id);
                double discountedPrice = item.Price - (item.Price * (discount / 100));
                Console.WriteLine($"{item.Name} ({item.Price:C}) - {discount}% = {discountedPrice:C}");
                totalDiscountedPrice += discountedPrice;
            }
            Console.WriteLine($"New price after discount: {totalDiscountedPrice:C}");
            Console.WriteLine("Press Enter to confirm or space bar to edit");
            ConsoleKeyInfo key = Console.ReadKey();// Allow the user to confirm or edit the selection
            while (key.Key != ConsoleKey.Enter)
            {
                if (key.Key == ConsoleKey.Spacebar)
                {
                    Console.WriteLine();
                    addCampaign();
                    return;
                }
                key = Console.ReadKey();
            }
            Guid campaignID = Guid.NewGuid();
            Dictionary<string, object> campaign = new Dictionary<string, object>
            {
                { "Name", name },
                { "Discount", discount },
                { "StartDate", startDate },
                { "EndDate", endDate },
                { "ItemIDs", selectedIDs },
                { "CampaignID", campaignID }
            };
            Campaigns.Add(campaign);
            Console.WriteLine($"{name} was added to {Name} campaigns.");
        }

        public bool viewStoreItems()
        {
            if (Inventory.Count == 0)
            {
                Console.WriteLine($"{Name} inventory is empty. Press any key to continue...");
                return false;
            }
            else
            {
                Console.WriteLine($"Inventory for {Name}:");
                Console.WriteLine("No.".PadRight(6) + "Item".PadRight(20) + "Price per item".PadRight(15));
                Console.WriteLine("----".PadRight(6) + "--------------------".PadRight(20) + "--------------".PadRight(15));
                for (int i = 0; i < Inventory.Count; i++)
                {
                    StoreItem item = Inventory[i];
                    Console.WriteLine($"{i + 1}".PadRight(6) + $"{item.Name}".PadRight(20) + $"{item.Price}".PadRight(15));
                }
            }
            return true;
        }

        public void viewCampaigns()
        {
            if (Campaigns.Count == 0)
            {
                Console.WriteLine($"{Name} does not have any campaigns. \nPress any key to continue...");
            }
            else
            {
                Console.WriteLine($"Campaigns for {Name}:");
                foreach (Dictionary<string, object> campaign in Campaigns)
                {
                    string campaignName = (string)campaign["Name"];
                    double discount = (double)campaign["Discount"];
                    DateTime startDate = (DateTime)campaign["StartDate"];
                    DateTime endDate = (DateTime)campaign["EndDate"];
                    List<Guid> itemIDs = (List<Guid>)campaign["ItemIDs"];
                    Console.WriteLine($"Campaign name: {campaignName} \nStarts : {startDate.ToShortDateString()}\nEnd date: {endDate.ToShortDateString()}");
                    Console.WriteLine("Selected items:");
                    double totalDiscount = 0;
                    foreach (Guid id in itemIDs)
                    {
                        StoreItem item = Inventory.Find(i => i.ItemID == id);
                        double itemDiscount = item.Price - (item.Price * (discount / 100));
                        Console.WriteLine($"Before discount {item.Name} ({item.Price:C} | You save = {(item.Price - itemDiscount):C})");
                        totalDiscount += itemDiscount;
                    }
                    Console.WriteLine($"Current Price: {totalDiscount:C}.\nPress any key to continue...");
                }
            }
        }

        public void viewHistoricalPurchases(List<Store> stores)
        {
            Console.WriteLine($"\nSales statistics per {Name}:");
            IEnumerable<ShoppingListItem> purchasedItems = PurchasedItems;
            int purchasedItemCountForStore = purchasedItems?.Sum(item => (int)item.Quantity) ?? 0;
            if (purchasedItems != null && purchasedItems.Any())
            {
                Dictionary<string, double> salesByProduct = purchasedItems.GroupBy(item => item.Name).ToDictionary(g => g.Key, g => g.Sum(item => item.Quantity));
                IEnumerable<KeyValuePair<string, double>> topSellingProducts = salesByProduct.OrderByDescending(kvp => kvp.Value).Take(3);
                IEnumerable<KeyValuePair<string, double>> leastSellingProducts = salesByProduct.OrderBy(kvp => kvp.Value).Take(3);
                Console.WriteLine("Top 3 most purchased products:");
                int i = 1;
                foreach (KeyValuePair<string, double> product in topSellingProducts)
                {
                    Console.WriteLine($"{i}. {product.Key} : {product.Value}");
                    i++;
                }
                Console.WriteLine("\n3 least purchased products:");
                i = 1;
                foreach (KeyValuePair<string, double> product in leastSellingProducts)
                {
                    IEnumerable<ShoppingListItem> itemsWithStatus = purchasedItems.Where(item => item.Name == product.Key && item.Status != ShoppingListItem.EnumStatus.Purchased);
                    Console.WriteLine($"{i}. {product.Key}");
                    i++;
                }
            }
            else
            {
                Console.WriteLine($"No purchased items for {Name}.");
            }
        }
    }
}   
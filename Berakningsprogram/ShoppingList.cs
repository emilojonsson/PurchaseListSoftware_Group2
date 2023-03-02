using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace inköpslista_grupp2
{
    public class ShoppingList
    {
        public string Name { get; set; }
        public Guid ShoppingListID { get; set; }
        public double TotalCost { get; set; }
        public List<ShoppingListItem> ItemList { get; set; }
        public User Customer { get; set; }
        public Status StatusShoppinglist { get; set; }
        public Store Store { get; set; }
        public ShoppingList(string name, User customer, Store store) 
        {
            this.Name = name;
            this.ShoppingListID = new Guid();
            this.TotalCost = 0;
            this.ItemList = new List<ShoppingListItem>();
            this.Customer = customer;
            this.StatusShoppinglist = Status.NotPurchased;
            this.Store = store;
        }
        public enum Status
        {
            NotPurchased,
            Purchased
        }
        public void addItem() 
        {
            Store.viewStoreItems();
            Console.WriteLine("Add the item from the list by entering the number related to it:");
            try
            {
                int selectInt = int.Parse(Console.ReadLine()) - 1;
                Console.WriteLine("How many/much of the item do you wish to add:");
                double quantity = double.Parse(Console.ReadLine());
                ItemList.Add(new ShoppingListItem(Store.inventory[selectInt], quantity));
                Console.WriteLine($"{ItemList[ItemList.Count - 1].ToString()}, has been added to the list. Press any key to continue...");
            }
            catch (Exception ex)
            {
                if (ex is FormatException)
                {
                    Console.Write("The input was not in correct format, ");
                }
                else if (ex is IndexOutOfRangeException || ex is ArgumentOutOfRangeException)
                {
                    Console.Write("The input could not relate to any item in the shoppinglist, ");
                }
                Console.WriteLine("no item was added to the list");
            }
            Console.ReadLine();
        }
        public void removeItem()
        {
            if (viewShoppingList() == true)
            {
                Console.WriteLine("Select the number related to the item that you wish to remove from the list:");
                try
                {
                    int selectInt = int.Parse(Console.ReadLine()) - 1;
                    Console.WriteLine($"{ItemList[selectInt].ToString()}, has been removed from the list\nPress [Enter] to go back..");
                    ItemList.RemoveAt(selectInt);
                }
                catch (Exception ex)
                {
                    if (ex is FormatException)
                    {
                        Console.Write("The input was not in correct format, ");
                    }
                    else if (ex is IndexOutOfRangeException || ex is ArgumentOutOfRangeException)
                    {
                        Console.Write("The input could not relate to any item in the shoppinglist, ");
                    }
                    Console.WriteLine("no item was removed from the list");
                }
            }
            else
            {
                Console.WriteLine("No items in the list to remove");
            }
            Console.ReadLine();
        }
        public void editItem()
        {
            if (viewShoppingList() == true)
            {
                Console.WriteLine("Select the number related to the item that you wish to edit in the list:");
                try
                {
                    int selectInt = int.Parse(Console.ReadLine()) - 1;
                    Console.WriteLine($"Item name = {ItemList[selectInt].Name}, change to?");
                    string name = Console.ReadLine();
                    ItemList[selectInt].Name = name;

                    Console.WriteLine($"Item quantity = {ItemList[selectInt].Quantity}, change to?");
                    double quantity = double.Parse(Console.ReadLine());
                    ItemList[selectInt].Quantity = quantity;

                    Console.WriteLine($"Item category = {ItemList[selectInt].Category}, change to?");
                    string category = Console.ReadLine();
                    ItemList[selectInt].Category = category;

                    Console.WriteLine();
                    Console.WriteLine($"Edited to => {ItemList[selectInt].Name}, {ItemList[selectInt].Quantity} [{ItemList[selectInt].Unit}], {ItemList[selectInt].Category}");

                    //todo. possible to implement more features
                }
                catch (Exception ex)
                {
                    if (ex is FormatException)
                    {
                        Console.Write("The input was not in correct format, ");
                    }
                    else if (ex is IndexOutOfRangeException || ex is ArgumentOutOfRangeException)
                    {
                        Console.Write("The input could not relate to any item in the shoppinglist, ");
                    }
                    Console.WriteLine("no item was edited in the list");
                }
                Console.ReadLine();
            }
        }
        public double getTotalCost(ShoppingListItem.EnumStatus status = ShoppingListItem.EnumStatus.NotPicked)
        {
            double sumOfStatus = 0;
            foreach(ShoppingListItem item in ItemList)
            {
                if (item.Status == status)
                {
                    sumOfStatus += item.Price * item.Quantity;
                }
            }
            return sumOfStatus;
        }
        public bool viewShoppingList()
        {
            if (ItemList.Count == 0) 
            {
                Console.WriteLine($"No items in the shopping list");
                return false;
            }
            else
            {
                ItemList = ItemList.OrderBy(o => o.Status).
                    ThenBy(o => o.Category).ThenBy(o => o.Name).ToList();
                
                int shoppingListIndex = 1;
                foreach (ShoppingListItem item in ItemList)
                {
                    Console.WriteLine($"{shoppingListIndex}. {item.ToString()}, category = {item.Category}, price = {item.Price}, status = {item.Status}");
                    shoppingListIndex++;
                }
                Console.WriteLine("Press [Enter] to continue..");
                return true;
            }
        }
        public void checkOutShoppingList(User customer, ShoppingList shoppingList)
        {
            int purchasedItems = 0;
            int purchasedItemsTotal = 0;
            foreach (ShoppingListItem item in ItemList)
            {
                if (item.Status == ShoppingListItem.EnumStatus.Picked)
                {
                    item.Status = ShoppingListItem.EnumStatus.Purchased;
                    customer.PurchasedItems.Add(item);
                    Store.PurchasedItems.Add(item);
                    purchasedItems++;
                }
                if (item.Status == ShoppingListItem.EnumStatus.Purchased)
                {
                    purchasedItemsTotal++;
                }
            }
            Console.WriteLine($"Checkout complete, {purchasedItems} items were purchased, " +
                $"{ItemList.Count - purchasedItemsTotal} items remains in the list");
            if (purchasedItemsTotal == ItemList.Count)
            {
                customer.ShoppingLists.Remove(shoppingList);
                Console.WriteLine("Shopping list has been removed");
            }
        }
        public void changeStatus()
        {
            if (viewShoppingList() == true)
            {
                Console.WriteLine("Select the number related to the item that you wish to pick from the shelf:");
                try
                {
                    int selectInt = int.Parse(Console.ReadLine()) - 1;
                    Console.WriteLine($"{ItemList[selectInt].ToString()}, now has status 'picked'. Press any key to continue...");
                    ItemList[selectInt].Status = ShoppingListItem.EnumStatus.Picked;
                }
                catch (Exception ex)
                {
                    if (ex is FormatException)
                    {
                        Console.Write("The input was not in correct format, ");
                    }
                    else if (ex is IndexOutOfRangeException || ex is ArgumentOutOfRangeException)
                    {
                        Console.Write("The input could not relate to any item in the shoppinglist, ");
                    }
                    Console.WriteLine("no item was picked in the list");
                }
                Console.ReadLine();
            }
        }
        public override string ToString()
        {
            return $"{Name}, totalcost: {getTotalCost().ToString()} kr, in {Store.Name}";
        }
    }
}

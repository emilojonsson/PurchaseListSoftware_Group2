﻿

namespace purchase_list_group2
{
    public partial class Program
    {
        static void SAMenu(Database dataObject)
        {
            Console.Clear();
            Console.WriteLine("1 Add user account");
            Console.WriteLine("2 Add store");
            Console.WriteLine("3 Add store item");
            Console.WriteLine("4 Edit store item");
            Console.WriteLine("5 Add store related campaigns");
            Console.WriteLine("6 Remove store and items");
            Console.WriteLine("7 View store statistics");
            Console.WriteLine();
            Console.WriteLine("0 Go back");

            try
            {
                int menuChoice = int.Parse(Console.ReadLine());
                switch (menuChoice)
                {
                    case 1:
                        createAccount(dataObject);
                        Console.ReadLine();
                        break;
                    case 2:
                        addStore(dataObject);
                        Console.ReadLine();
                        break;
                    case 3:
                        selectStore(dataObject).addStoreItem();
                        break;
                    case 4:
                        selectStore(dataObject).editStoreItem();
                        Console.ReadLine();
                        break;
                    case 5:
                        selectStore(dataObject).addCampaign();
                        Console.ReadLine();
                        break;
                    case 6:
                        Store.removeStoreAndItems(dataObject.StoreObjects);
                        Console.ReadLine(); 
                        break;
                    case 7:
                        selectStore(dataObject).viewHistoricalPurchases(dataObject.StoreObjects);
                        Console.ReadLine();
                        break;
                    default:
                        if (menuChoice == 0)
                        {
                            return; 
                        }
                        break;
                }
            }
            catch (FormatException) { }
            SAMenu(dataObject);
        }
        static void CustomerMenu(Database dataObject, User customer)
        {
            Console.Clear();
            Console.WriteLine("1 Edit customer account");
            Console.WriteLine("2 Remove customer account");
            Console.WriteLine();
            Console.WriteLine("3 View items in store");
            Console.WriteLine("4 View store related campaigns");
            Console.WriteLine();
            Console.WriteLine("5 Add new template");
            Console.WriteLine("6 View saved templates");
            Console.WriteLine();
            Console.WriteLine("7 Add new shopping list related to a store");
            Console.WriteLine("8 Add items to a shopping list");
            Console.WriteLine("9 Remove item from shopping list");
            Console.WriteLine("10 Remove shopping list and items");
            Console.WriteLine("11 Edit item in shopping list");
            Console.WriteLine("12 View shopping lists and their items");
            Console.WriteLine("13 Share shopping list with other accounts");
            Console.WriteLine();
            Console.WriteLine("14 Mark item in shopping list as picked");
            Console.WriteLine("15 Store checkout"); 
            Console.WriteLine();
            Console.WriteLine("16 View account statistics");
            Console.WriteLine();
            Console.WriteLine("0 Go back");
            try
            {
                int menuChoice = int.Parse(Console.ReadLine());
                switch (menuChoice)
                {
                    case 1:
                        customer.editContactInformation();
                        Console.ReadLine();
                        break;
                    case 2:
                        customer.removeUser(dataObject.UserObjects, customer);
                        Console.ReadLine();
                        return;
                    case 3:
                        selectStore(dataObject).viewStoreItems();
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadLine();
                        break;
                    case 4:
                        selectStore(dataObject).viewCampaigns();
                        Console.ReadLine();
                        break;
                    case 5:
                        customer.createNewTemplate(dataObject.StoreObjects);
                        Console.ReadLine();
                        break;
                    case 6:
                        customer.displaySavedTemplates();
                        Console.ReadLine();
                        break;
                    case 7:
                        customer.addNewShoppingList(customer.UserID, dataObject.StoreObjects);
                        break;
                    case 8:
                        selectShoppingList(customer).addItem();
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadLine();
                        break;
                    case 9:
                        selectShoppingList(customer).removeItem();
                        Console.ReadLine();
                        break;
                    case 10:
                        customer.removeShoppingList();
                        Console.ReadLine();
                        break;
                    case 11:
                        selectShoppingList(customer).editItem();
                        Console.ReadLine();
                        break;
                    case 12:
                        customer.viewAllShoppingLists();
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadLine();
                        break;
                    case 13:
                        customer.shareListWithOthers(dataObject.UserObjects);
                        Console.ReadLine();
                        break;
                    case 14:
                        selectShoppingList(customer).changeStatus();
                        Console.ReadLine();
                        break;
                    case 15:
                        ShoppingList shoppingList = selectShoppingList(customer);
                        shoppingList.checkOutShoppingList(customer, shoppingList, dataObject.StoreObjects);
                        Console.ReadLine();
                        break;
                    case 16:
                        customer.viewHistoricalPurchases();
                        Console.ReadLine();
                        break;
                    default:
                        if (menuChoice == 0)
                        {
                            return;
                        }
                        break;
                }
            }
            catch (FormatException) { }
            CustomerMenu(dataObject, customer);
        }
        static void Main(string[] args)
        {
            Database dataObject = new Database();
            dataObject.LoadDataBase();
            bool loop = true;
            while (loop)
            {
                Console.Clear();
                Console.WriteLine("1 Systemadministrator");
                Console.WriteLine("2 Customer");
                Console.WriteLine();
                Console.WriteLine("0 Exit");
                try
                {
                    int menuChoice = int.Parse(Console.ReadLine());
                    switch (menuChoice)
                    {
                        case 1:
                            SAMenu(dataObject);
                            break;
                        case 2:
                            if (dataObject.UserObjects.Count == 0)
                            {
                                Console.WriteLine("You must first create a new user in the administrator menu. Press any key to continue...");
                                Console.ReadLine();
                                break;
                            }
                            User customer = selectUser(dataObject.UserObjects);
                            CustomerMenu(dataObject, customer);
                            break;
                        case 0:
                            loop = false;
                            dataObject.SaveToDataBase();
                            break;
                        default:
                            break;
                    }
                }
                catch (FormatException) { }
            }
        }
    }
}
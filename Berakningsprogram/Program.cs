
using System.Xml.Linq;

namespace purchase_list_group2
{
    public class Program
    {
        static void createAccount(Database dataObject)
        {
            Console.WriteLine("Enter the name:");
            string name = Console.ReadLine();
            Console.WriteLine("Enter the email address:");
            string email = Console.ReadLine();
            dataObject.UserObjects.Add(new User(name, email, false));
            Console.WriteLine("The user was added");
        }
        static User selectUser(List<User> users)
        {
            int intUser = 0;
            int selectUser = 1;
            foreach (User user in users) 
            {
                Console.WriteLine($"{selectUser}, {user.Name}");
                selectUser++;
            }
            do
            {
                Console.WriteLine("Select user:");
                try
                {
                    intUser = int.Parse(Console.ReadLine()) - 1;
                }
                catch (FormatException)
                {
                    Console.WriteLine("The input was not in correct format, try again");
                }
            }
            while (intUser < 0 || intUser >= users.Count);
            return users[intUser];

        }
        static Store selectStore(Database dataObject)
        {
            int intStore = 0;
            int storeIndex = 1;
            foreach (Store store in dataObject.StoreObjects)
            {
                Console.WriteLine($"{storeIndex}. {store.Name}");
                storeIndex++;
            }
            do
            {
                Console.WriteLine("Select store:");
                try
                {
                    intStore = int.Parse(Console.ReadLine()) - 1;
                }
                catch (FormatException)
                {
                    Console.WriteLine("The input was not in correct format, try again");
                }
            }
            while (intStore < 0 || intStore >= dataObject.StoreObjects.Count); 
            return dataObject.StoreObjects[intStore];
        }
        static ShoppingList selectShoppingList(Database dataObject)
        {
            int intShoppingList = 0;
            int shoppingListIndex = 1;
            foreach (ShoppingList shoppingList in dataObject.UserObjects[0].ShoppingLists)
            {
                Console.WriteLine($"{shoppingListIndex}. {shoppingList.Name}");
                shoppingListIndex++;
            }
            while (true)
            {
                Console.WriteLine("Select shoppinglist:");
                if (int.TryParse(Console.ReadLine(), out intShoppingList))
                {
                    intShoppingList--;
                    if (intShoppingList >= 0 && intShoppingList < dataObject.UserObjects[0].ShoppingLists.Count)
                    {
                        return dataObject.UserObjects[0].ShoppingLists[intShoppingList];
                    }
                }
                Console.WriteLine("The input was not in correct format, try again");
            }
        }
        static void SAMenu(Database dataObject)
        {
            Console.Clear();
            Console.WriteLine("1 Add customer account");
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
                        break;
                    case 2:
                        addStore(dataObject);
                        break;
                    case 3:
                        selectStore(dataObject).addStoreItem();
                        break;
                    case 4:
                        selectStore(dataObject).editStoreItem();
                        break;
                    case 5:
                        selectStore(dataObject).addCampaign();
                        break;
                    case 6:
                        Store.removeStoreAndItems(dataObject.StoreObjects);
                        break;
                    case 7:
                        selectStore(dataObject).viewHistoricalPurchases(dataObject.StoreObjects);
                        break;
                    default:
                        if (menuChoice == 0)
                        {
                            return; // exit the SAMenu method if user selects "0 Go back"
                        }
                        else
                        {
                            Console.WriteLine("");
                            Console.WriteLine("\nInvalid input. Select a number from the menu and press [Enter].");
                        }
                        break;
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("\nInvalid input. Select a number from the menu and press [Enter].");
            }
            // return to the SAMenu method to show the menu again
            SAMenu(dataObject);
        }

        private static void addStore(Database dataObject)
        {
            Console.WriteLine("What's the name of the new store:");
            string storeName = Console.ReadLine();
            dataObject.StoreObjects.Add(new Store(storeName));
            Console.WriteLine($"Store '{storeName}' has been added.");
            Console.WriteLine("Press Enter to continue..");
            Console.ReadLine();
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
            Console.WriteLine();
            Console.WriteLine("6 Add new shopping list related to a store");
            Console.WriteLine("7 Add items to shopping list");
            Console.WriteLine("8 Remove item from shopping list");
            Console.WriteLine("9 Remove shopping list and items");
            Console.WriteLine("10 Edit item in shopping list");
            Console.WriteLine("11 View shopping lists and their items");
            Console.WriteLine("12 Share shopping list with other accounts");
            Console.WriteLine();
            Console.WriteLine("13 Mark item in shopping list as picked");
            Console.WriteLine("14 Store checkout"); 
            Console.WriteLine();
            Console.WriteLine("15 View account statistics");
            Console.WriteLine();
            Console.WriteLine("0 Go back");

            int menuChoice = int.Parse(Console.ReadLine());
            switch (menuChoice)
            {
                case 1:
                    customer.editContactInformation();
                    break;
                case 2:
                    customer.removeUser(dataObject.UserObjects, customer);
                    break;
                case 3:
                    selectStore(dataObject).viewStoreItems();
                    break;
                case 4:
                    selectStore(dataObject).viewCampaigns();
                    break;
                case 5:
                    customer.addNewTemplates(dataObject.StoreObjects);
                    break;
                case 6:
                    customer.addNewShoppingList(customer.UserID, dataObject.StoreObjects);
                    break;
                case 7:
                    selectShoppingList(dataObject).addItem();
                    break;
                case 8:
                    selectShoppingList(dataObject).removeItem();
                    break;
                case 9:
                    customer.removeShoppingList();
                    break;
                case 10:
                    selectShoppingList(dataObject).editItem();
                    break;
                case 11:
                    customer.viewAllShoppingLists();
                    break;
                case 12:
                    //customer.shareListWithOthers(users);
                    break;
                case 13:
                    selectShoppingList(dataObject).changeStatus();
                    break;
                case 14:
                    ShoppingList shoppingList = selectShoppingList(dataObject);
                    shoppingList.checkOutShoppingList(customer, shoppingList);
                    break;
                case 15:
                    customer.viewHistoricalPurchases();
                    break;
                default:
                    break;
            }
        }
        static void Main(string[] args)
        {
            //Here we can create objects initialy when we don´t have the database

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
                    Console.Clear();
                    switch (menuChoice)
                    {
                        case 1:
                            SAMenu(dataObject);
                            break;
                        case 2:
                            User customer = selectUser(dataObject.UserObjects);
                            CustomerMenu(dataObject, customer);
                            break;
                        case 0:
                            loop = false;
                            dataObject.SaveToDataBase();
                            break;
                        default:
                            Console.WriteLine("Invalid input. Please enter a valid menu choice.");
                            break;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input. Please enter a valid menu choice.");
                }
            }
        }
    }
}
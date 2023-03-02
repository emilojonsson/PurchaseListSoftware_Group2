namespace inköpslista_grupp2
{
    public class Program
    {
        static Store selectStore(List<Store> storeList)
        {
            int intStore = 0;
            int storeIndex = 1;
            foreach (Store store in storeList)
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
            while (intStore < 0 || intStore >= storeList.Count); 
            return storeList[intStore];
        }
        static ShoppingList selectShoppingList(List<ShoppingList> shoppingLists)
        {
            int intShoppingList = 0;
            int shoppingListIndex = 1;
            foreach (ShoppingList shoppingList in shoppingLists)
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
                    if (intShoppingList >= 0 && intShoppingList < shoppingLists.Count)
                    {
                        return shoppingLists[intShoppingList];
                    }
                }
                Console.WriteLine("The input was not in correct format, try again");
            }
        }

        static void SAMenu(User systemAdministrator, List<Store> storeList)
        {
            Console.WriteLine();
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
                        //User newUser = new User();
                        //newUser.createAccount();
                        break;
                    case 2:
                        addStore(storeList);
                        break;
                    case 3:
                        selectStore(storeList).addStoreItem();
                        break;
                    case 4:
                        selectStore(storeList).editStoreItem();
                        break;
                    case 5:
                        selectStore(storeList).addCampaign();
                        break;
                    case 6:
                        Store.removeStoreAndItems(storeList);
                        break;
                    case 7:
                        selectStore(storeList).viewHistoricalPurchases(storeList);
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
            SAMenu(systemAdministrator, storeList);
        }

        private static void addStore(List<Store> storeList)
        {
            Console.WriteLine("What's the name of the new store:");
            string storeName = Console.ReadLine();
            storeList.Add(new Store(storeName));
            Console.WriteLine($"Store '{storeName}' has been added.");
            Console.WriteLine("Press Enter to continue..");
            Console.ReadLine();
        }

        static void CustomerMenu(User customer, List<Store> storeList, List<User> users)
        {
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
                    //customer.removeUser();
                    break;
                case 3:
                    selectStore(storeList).viewStoreItems();
                    break;
                case 4:
                    selectStore(storeList).viewCampaigns();
                    break;
                case 5:
                    customer.addNewTemplates(storeList);
                    break;
                case 6:
                    customer.addNewShoppingList(customer, storeList);
                    break;
                case 7:
                    selectShoppingList(customer.ShoppingLists).addItem();
                    break;
                case 8:
                    selectShoppingList(customer.ShoppingLists).removeItem();
                    break;
                case 9:
                    customer.removeShoppingList();
                    break;
                case 10:
                    selectShoppingList(customer.ShoppingLists).editItem();
                    break;
                case 11:
                    customer.viewAllShoppingLists();
                    break;
                case 12:
                    //customer.shareListWithOthers(users);
                    break;
                case 13:
                    selectShoppingList(customer.ShoppingLists).changeStatus();
                    break;
                case 14:
                    ShoppingList shoppingList = selectShoppingList(customer.ShoppingLists);
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
            //SomethingDatabaseContext DB = new SomethingDatabaseContext(); // Any use of the Database will need this.

            User systemAdmin = new User("systemadmin", "system@gmail.com", true);
            User customer1 = new User("svenne", "svenne@gmail.com", false);
            User customer2 = new User("benne", "benne@gmail.com", false);
            List<User> users = new List<User>();
            User user = new User("Carl", "carl.carnmo@gmail.com", false);
            Guid userID = new Guid("00000000000000000000000000000000");
            Console.WriteLine(user.UserID.ToString());
            //DB.RemoveUser(userID);
            users.Add(systemAdmin);
            users.Add(customer1);
            users.Add(customer2);
            List<Store> storeList = new List<Store>();
            Store store1 = new Store("Jula");
            storeList.Add(store1);
            ShoppingList shoppingList1 = new ShoppingList("List to Jula", customer1, store1);

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
                            SAMenu(systemAdmin, storeList);
                            break;
                        case 2:
                            CustomerMenu(customer1, storeList, users);
                            break;
                        case 0:
                            loop = false;
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
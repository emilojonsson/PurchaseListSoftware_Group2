
namespace purchase_list_group2
{
    public partial class Program
    {
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
        static User selectUser(List<User> users)
        {
            Console.Clear();
            int intUser = 0;
            int selectUser = 1;
            foreach (User user in users)
            {
                Console.WriteLine($"{selectUser}. {user.Name}");
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
        static void addStore(Database dataObject)
        {
            Console.WriteLine("What's the name of the new store:");
            string storeName = Console.ReadLine();
            dataObject.StoreObjects.Add(new Store(storeName));
            Console.WriteLine($"Store '{storeName}' has been added. Press any key to continue...");
        }

        static void createAccount(Database dataObject)
        {
            Console.WriteLine("Enter the name:");
            string name = Console.ReadLine();
            Console.WriteLine("Enter the email address:");
            string email = Console.ReadLine();
            dataObject.UserObjects.Add(new User(name, email, false));
            Console.WriteLine($"The user {name} was added. Press any key to continue...");
        }

    }
}

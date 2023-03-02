using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Formats.Asn1.AsnWriter;

namespace inköpslista_grupp2
{
    public class User : ICSVable
    {
        private bool emailEdited = false; // new private field for editContactInformation() 
        public string? Name { get; set; }
        public Guid UserID { get; set; }
        public string? Email { get; set; }
        public bool SystemAdminstrator { get; set; }
        public List<ShoppingList> ShoppingLists { get; set; } = new List<ShoppingList>();
        public List<ShoppingList> Template { get; set; } = new List<ShoppingList>();
        public List<ShoppingListItem> PurchasedItems { get; set; } = new List<ShoppingListItem>();
        public User(string name, string email, bool systemAdminstrator)
        {
            Name = name;
            UserID = new Guid();
            Email = email;
            SystemAdminstrator = systemAdminstrator;
        }

        public string CSVify()
        {
            return $"{nameof(Name)}:{Name},{nameof(UserID)}:{UserID},{nameof(Email)}:{Email},{nameof(SystemAdminstrator)}:{SystemAdminstrator},{nameof(ShoppingLists)}:{string.Join(";", ShoppingLists)}, {nameof(Template)}:{string.Join(";", Template)}, {nameof(PurchasedItems)}:{string.Join(";", PurchasedItems)}";
        }

        //maybe we should consider moving this method to outside the class. Move it wherever you want.
        //public User addUser(SomethingDatabaseContext DB) // Needs DB to store a user.
        //{
        //    bool SystemAdministrator = false;
        //    Console.WriteLine("Enter your name: ");
        //    string Name = Console.ReadLine();
        //    Console.WriteLine("Enter your mail: ");
        //    string Email = Console.ReadLine();
        //    Console.WriteLine("admin(true/false): ");
        //    //bool SystemAdminstrator = Console.ReadLine();
        //    //Console.WriteLine("");
        //    Guid UserID = Guid.NewGuid();
        //    User user = new User(Name, UserID, Email, false);
        //    DB.Create<User>(user as User);
        //    return user;
        //}

        public void createAccount()//"1 Add customer account"); SA MENU
        {
            //Console.WriteLine("Enter the name:");
            //Name = Console.ReadLine();

            //Console.WriteLine("Enter the email address:");
            //Email = Console.ReadLine();

            //Console.WriteLine("Enter the user ID (press Enter to skip):");
            //string userIdInput = Console.ReadLine();

            //if (Guid.TryParse(userIdInput, out Guid result))
            //{
            //    UserID = result;
            //}
            //else
            //{
            //    Console.WriteLine("Warning: User ID not provided. Account will be locked until a valid User ID is provided.");
            //    Console.WriteLine("Press [Enter] to continue..");
            //    Console.ReadLine();
            //}
        }

        public void addNewShoppingList(User customer, List<Store> storeList)
        {
            Console.WriteLine("Do you wish to create the shoppinglist from a template [yes], if no just press any key...");
            if (Console.ReadLine()?.ToLower() == "yes")
            {
                addTemplate();
            }
            else
            {
                Console.WriteLine("Select from which store you wish the shopping list to be based upon");
                int storeIndex = 1;
                foreach (Store store in storeList)
                {
                    Console.WriteLine($"{storeIndex}. {store.Name}");
                    storeIndex++;
                }
                try
                {
                    int selectInt = int.Parse(Console.ReadLine()) - 1;

                    Console.WriteLine("What do you want to call the shoppinglist?");
                    string shoppingListName = Console.ReadLine();
                    ShoppingLists.Add(new ShoppingList(shoppingListName, customer, storeList[selectInt]));
                }
                catch (Exception ex)
                {
                    if (ex is FormatException)
                    {
                        Console.Write("The input was not in correct format, ");
                    }
                    else if (ex is IndexOutOfRangeException || ex is ArgumentOutOfRangeException)
                    {
                        Console.Write("The input could not relate to any store, ");
                    }
                    Console.WriteLine("no shopping list was created");
                }
            }

            bool loop = true;
            while (loop)
            {
                Console.WriteLine("Work finished with this shopping list [yes], or continue add more items (just press any key)...");
                if (Console.ReadLine()?.ToLower() != "yes")
                {
                    ShoppingLists[ShoppingLists.Count - 1].addItem();
                }
                else
                {
                    loop = false;
                }
            }
        }
        public void removeShoppingList()
        {
            viewAllShoppingLists();
            if (ShoppingLists.Count > 0)
            {
                int intShoppingListIndex = 1;
                foreach (ShoppingList list in ShoppingLists)
                {
                    Console.WriteLine($"{intShoppingListIndex}: {list.Name}");
                    intShoppingListIndex++;
                }
                Console.WriteLine("Which shopping list do you want to remove?");
                try
                {
                    int intRemoveList = int.Parse(Console.ReadLine()) - 1;
                    Console.WriteLine($"You removed list: {ShoppingLists[intRemoveList].Name}\nPress [Enter] to continue..]");
                    ShoppingLists.RemoveAt(intRemoveList);
                }
                catch (Exception ex)
                {
                    if (ex is FormatException)
                    {
                        Console.Write("The input was not in correct format, ");
                    }
                    else if (ex is IndexOutOfRangeException || ex is ArgumentOutOfRangeException)
                    {
                        Console.Write("The input could not relate to any shopping list, ");
                    }
                    Console.WriteLine("no shopping list was removed");
                }
                Console.ReadLine();
            }
        }
        public void shareListWithOthers(List<User> users)
        {
            throw new NotImplementedException();
        }
       
        public void addNewTemplates(List<Store> storeList)
        {
            Console.WriteLine("Choose a store for the new template:");
            int storeIndex = 1;
            foreach (Store store in storeList)
            {
                Console.WriteLine($"{storeIndex}. {store.Name}");
                storeIndex++;
            }
            int selectInt = int.Parse(Console.ReadLine()) - 1;

            Console.WriteLine("Enter a name for the new template:");
            string templateName = Console.ReadLine();
            var newTemplate = new ShoppingList(templateName, null, storeList[selectInt]);

            Console.WriteLine($"Adding items to template {templateName} at store {storeList[selectInt].Name}");
            while (true)
            {
                storeList[selectInt].viewStoreItems();
                Console.WriteLine("Enter the number of the item to add or 'exit' to stop adding items:");
                string input = Console.ReadLine();
                if (input == "exit")
                    return;
                try
                {
                    int selectItem = int.Parse(input);
                    Console.WriteLine("Enter the quantity of the item to add:");
                    if (double.TryParse(Console.ReadLine(), out double quantity))
                    {
                        var itemToAdd = new ShoppingListItem(storeList[selectInt].inventory[selectItem - 1], quantity);
                        newTemplate.ItemList.Add(itemToAdd);
                        Console.WriteLine($"Added {itemToAdd.Quantity} {itemToAdd.Unit} to the template");
                    }
                    else
                    {
                        Console.WriteLine("Invalid quantity entered, item not added");
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input format, item not added");
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("Invalid item number entered, item not added");
                }
            }
            addNewTemplates(storeList);
        }

        public void editContactInformation()
        {
            if (emailEdited) // check if email has already been edited
            {
                Console.WriteLine("Email has already been edited and cannot be updated again.\nPress [Enter] to exit.");
                Console.ReadLine();
                return;
            }
            Console.WriteLine($"Old email:{this.Email}");
            Console.Write($"New email:");
            string newemail = Console.ReadLine();
            this.Email = newemail;
            Console.WriteLine($"You successfully changed your email to {this.Email}\nPress [Enter] to exit.");
            Console.ReadLine();
            emailEdited = true; // set emailEdited to true
        }



        public void removeUser()
        {
            /*foreach (User user in database)//database?
            {
                if(this.userID == user.userID)
                {
                        user.delete? 
                }
            }
            */
        }
        public void viewAllShoppingLists()
        {
            if (ShoppingLists.Count == 0) 
            {
                Console.WriteLine("No shopping lists to view");
                Console.WriteLine("Press [Enter] to go back");
            }
            else
            {
                foreach (ShoppingList list in ShoppingLists)
                {
                    Console.WriteLine($"__{list.ToString().ToUpper()}__");
                    list.viewShoppingList();
                }
            }
            Console.ReadLine();
        }
        public void addTemplate()
        {
            if (ShoppingLists == null)
            {
                ShoppingLists = new List<ShoppingList>();
            }
            Console.WriteLine("Create a new shopping list: ");
            Console.WriteLine("1. From scratch");
            Console.WriteLine("2. From template");
            int choice = int.Parse(Console.ReadLine());
            if (choice == 1){ addNewTemplates(new List<Store>()); }

            else if (choice == 2){
                Console.WriteLine("Choose a template from these options:");
                for (int i = 0; i < Template.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {Template[i].Name}");
                }
                int templateChoice = int.Parse(Console.ReadLine()) - 1;
                ShoppingList template = Template[templateChoice];
                ShoppingList newShoppingList = new ShoppingList(template.Name, this, new Store("MyStore"));
                ShoppingLists.Add(newShoppingList);
                Console.WriteLine($"New shopping list '{newShoppingList.Name}' created from template '{template.Name}':");
                Console.WriteLine("Press Enter to continue.");
                Console.ReadLine();
            }
        }
        public void viewHistoricalPurchases()
        {
            Console.WriteLine("All your historical purchases:");
            foreach (ShoppingListItem item in PurchasedItems)
            {
                Console.WriteLine($"Purchase date: {item.PurchaseDate.ToString("yyyy-MM-dd")}");
                Console.WriteLine($"Item ID: {item.ItemID}");
                Console.WriteLine($"Quantities: {item.Quantity} {item.Unit}");
                Console.WriteLine($"Cost: {item.Quantity * item.Price}");
                Console.WriteLine();
            }
            Console.ReadLine();
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Formats.Asn1.AsnWriter;

namespace purchase_list_group2
{
    [DataContract]
    public class User
    {
        [DataMember]
        public bool EmailEdited { get; set; } = false; 
        [DataMember]
        public string? Name { get; set; }
        [DataMember]
        public Guid UserID { get; set; }
        [DataMember]
        public string? Email { get; set; }
        [DataMember]
        public bool SystemAdminstrator { get; set; }
        [DataMember]
        public List<ShoppingList> ShoppingLists { get; set; } = new List<ShoppingList>();
        [DataMember]
        public List<ShoppingList> Template { get; set; } = new List<ShoppingList>();
        [DataMember]
        public List<ShoppingListItem> PurchasedItems { get; set; } = new List<ShoppingListItem>();
        public User(string name, string email, bool systemAdminstrator)
        {
            Name = name;
            UserID = Guid.NewGuid();
            Email = email;
            SystemAdminstrator = systemAdminstrator;
        }
        public void addNewShoppingList(Guid UserID, List<Store> storeList)
        {
            Console.WriteLine("Do you wish to create the shoppinglist from a template [yes], if no just press any key...");
            if (Console.ReadLine()?.ToLower() == "yes")
            {
                addTemplateToShoppingList();
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
                    ShoppingLists.Add(new ShoppingList(shoppingListName, UserID, storeList[selectInt]));
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
                Console.WriteLine("Finished [yes], or continue add items (just press any key)...");
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
                    Console.WriteLine($"You removed list: {ShoppingLists[intRemoveList].Name}\nPress any key to continue...");
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
            }
        }
        public void shareListWithOthers(List<User> users)
        {
            throw new NotImplementedException();
        }
        public void createNewTemplate(List<Store> storeList)
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
            var newTemplate = new ShoppingList(templateName, UserID, storeList[selectInt]);

            Console.WriteLine($"Adding items to template {templateName} at store {storeList[selectInt].Name}");
            while (true)
            {
                storeList[selectInt].viewStoreItems();
                Console.WriteLine("Enter the number of the item to add or 'exit' to stop adding items:");
                string input = Console.ReadLine();
                if (input == "exit")
                    break;
                try
                {
                    int selectItem = int.Parse(input);
                    Console.WriteLine("Enter the quantity of the item to add:");
                    if (double.TryParse(Console.ReadLine(), out double quantity))
                    {
                        var itemToAdd = new ShoppingListItem(storeList[selectInt].Inventory[selectItem - 1], quantity, UserID);
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
            Template.Add(newTemplate);
        }
        public void displaySavedTemplates()
        {
            if (Template.Count == 0)
            {
                Console.WriteLine("No saved templates.");
            }
            else
            {
                Console.WriteLine($"Saved templates for {Name}:");
                foreach (ShoppingList template in Template)
                {
                    Console.WriteLine($"{template.Name} in {template.Store.Name}");
                    template.viewShoppingList();

                }
            }
            Console.WriteLine("Press any key to continue...");
        }
        public void addTemplateToShoppingList()
        {
            Console.WriteLine("Choose a template from these options:");
            for (int i = 0; i < Template.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {Template[i].Name}");
            }
            int templateChoice = int.Parse(Console.ReadLine()) - 1;
            ShoppingList template = Template[templateChoice];
            ShoppingList newShoppingList = new ShoppingList(template.Name, UserID, template.Store);
            ShoppingLists.Add(newShoppingList);
            Console.WriteLine($"New shopping list '{newShoppingList.Name}' created from template '{template.Name}'.");
        }
        public void editContactInformation()
        {
            if (EmailEdited)
            {
                Console.WriteLine("Email has already been edited and cannot be updated again.\nPress any key to continue...");
                return;
            }
            Console.WriteLine($"Old email:{this.Email}");
            Console.Write($"New email:");
            string newemail = Console.ReadLine();
            this.Email = newemail;
            Console.WriteLine($"You successfully changed your email to {this.Email}\nPress any key to continue...");
            EmailEdited = true; 
        }
        public void removeUser(List<User> users, User customer)
        {
            int removeUser = 0;
            foreach (User user in users)
            {
                if (customer.UserID == user.UserID)
                {
                    break;
                }
                removeUser++;
            }
            users.Remove(users[removeUser]);
            Console.WriteLine("User removed!\nPress any key to continue...");
        }
        public void viewAllShoppingLists()
        {
            if (ShoppingLists.Count == 0) 
            {
                Console.WriteLine("No shopping lists to view.");
            }
            else
            {
                foreach (ShoppingList list in ShoppingLists)
                {
                    Console.WriteLine($"__{list.ToString().ToUpper()}__");
                    list.viewShoppingList();
                }
            }
        }
        public void viewHistoricalPurchases()
        {
            Console.WriteLine("All your historical purchases:");
            foreach (ShoppingListItem item in PurchasedItems)
            {
                Console.WriteLine($"Purchase date: {item.PurchaseDate.ToString("yyyy-MM-dd")}");
                Console.WriteLine($"Item ID: {item.Name}");
                Console.WriteLine($"Quantities: {item.Quantity} {item.Unit}");
                Console.WriteLine($"Cost: {item.Quantity * item.Price}");
                Console.WriteLine();
            }
            Console.WriteLine("Press any key to continue...");
        }
    }
}
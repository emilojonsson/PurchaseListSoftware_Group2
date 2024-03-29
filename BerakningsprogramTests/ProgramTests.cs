﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Berakningsprogram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using purchase_list_group2;

namespace Berakningsprogram.Tests
{
    [TestClass()]
    public class ProgramTests
    {
        [TestMethod()]
        public void addItemTest()
        {
            int expectedItemsInList = 1;
            string expectedName = "Article 1";
            string expectedCategory = "Furniture";
            double expectedPrice = 100;
            Item.UnitEnum expectedEnum = Item.UnitEnum.Pcs;
            double expectedQuantity = 10;

            Store store1 = new Store("Ikea");
            StoreItem storeItem = new StoreItem(expectedName, expectedCategory, expectedPrice, expectedEnum);
            store1.Inventory.Add(storeItem);
            User customer = new User("Pelle", "p@gmail.com", false);
            ShoppingList shoppingList = new ShoppingList("the shopping list", customer.UserID, store1);

            string simulateUserInMethod = "1\n" + "10\n" + "readLine\n";
            StringReader srObject = new StringReader(simulateUserInMethod);
            Console.SetIn(srObject);

            shoppingList.addItem();

            Assert.AreEqual(expectedItemsInList, shoppingList.ItemList.Count);
            Assert.AreEqual(expectedName, shoppingList.ItemList[0].Name);
            Assert.AreEqual(expectedCategory, shoppingList.ItemList[0].Category);
            Assert.AreEqual(expectedPrice, shoppingList.ItemList[0].Price);
            Assert.AreEqual(expectedEnum, shoppingList.ItemList[0].Unit);
            Assert.AreEqual(expectedQuantity, shoppingList.ItemList[0].Quantity);
        }
        [TestMethod()]
        public void removeItemTest()
        {
            int expectedItemsInList = 1;
            StoreItem expectedToRemoved = new StoreItem("Article 1", "AAA", 50, Item.UnitEnum.Litres);
            string expectedToRemainName = "Article 2";
            StoreItem expectedToRemain = new StoreItem(expectedToRemainName, "BBB", 100, Item.UnitEnum.Pcs);

            Store store1 = new Store("Ikea");
            store1.Inventory.Add(expectedToRemoved);
            store1.Inventory.Add(expectedToRemain);
            User customer = new User("Pelle", "p@gmail.com", false);
            ShoppingList shoppingList = new ShoppingList("the shopping list", customer.UserID, store1);
            shoppingList.ItemList.Add(new ShoppingListItem(expectedToRemoved, 5, customer.UserID));
            shoppingList.ItemList.Add(new ShoppingListItem(expectedToRemain, 10, customer.UserID));

            string simulateUserInMethod = "1\n" + "readLine\n";
            StringReader srObject = new StringReader(simulateUserInMethod);
            Console.SetIn(srObject);

            shoppingList.removeItem();

            Assert.AreEqual(expectedItemsInList, shoppingList.ItemList.Count);
            Assert.AreEqual(expectedToRemainName, shoppingList.ItemList[0].Name);
        }
        [TestMethod()]
        public void editItem()
        {
            string expectedNewName = "New name";
            double expectedNewQuantity = 10;
            StoreItem expectedToEdit = new StoreItem("Old name", "AAA", 50, Item.UnitEnum.Litres);
            StoreItem expectedNotEdit = new StoreItem("Article 1", "BBB", 100, Item.UnitEnum.Pcs);

            Store store1 = new Store("Ikea");
            store1.Inventory.Add(expectedToEdit);
            store1.Inventory.Add(expectedNotEdit);
            User customer = new User("Pelle", "p@gmail.com", false);
            ShoppingList shoppingList = new ShoppingList("the shopping list", customer.UserID, store1);
            shoppingList.ItemList.Add(new ShoppingListItem(expectedToEdit, 5, customer.UserID));
            shoppingList.ItemList.Add(new ShoppingListItem(expectedNotEdit, 5, customer.UserID));

            string simulateUserInMethod = "1\n" + "New name\n" + "10\n" + "readLine\n";
            StringReader srObject = new StringReader(simulateUserInMethod);
            Console.SetIn(srObject);

            shoppingList.editItem();

            Assert.AreEqual(expectedNewName, shoppingList.ItemList[0].Name);
            Assert.AreEqual(expectedNewQuantity, shoppingList.ItemList[0].Quantity);
        }
        [TestMethod()]
        public void getTotalCost()
        {
            double expectedCost1 = 1; //when no argument given, and when NotPicked as argument
            double expectedCost2 = 10; //Picked as argument
            double expectedCost3 = 100; //Purchased as argument

            StoreItem storeItem1 = new StoreItem("Article 1", "AAA", 1, Item.UnitEnum.Pcs);
            StoreItem storeItem2 = new StoreItem("Article 2", "BBB", 10, Item.UnitEnum.Pcs);
            StoreItem storeItem3 = new StoreItem("Article 3", "CCC", 100, Item.UnitEnum.Pcs);
            User customer = new User("Pelle", "p@gmail.com", false);
            ShoppingList shoppingList = new ShoppingList("the shopping list", customer.UserID, new Store("Ikea"));
            shoppingList.ItemList.Add(new ShoppingListItem(storeItem1, 1, customer.UserID));
            shoppingList.ItemList.Add(new ShoppingListItem(storeItem2, 1, customer.UserID));
            shoppingList.ItemList.Add(new ShoppingListItem(storeItem3, 1, customer.UserID));
            shoppingList.ItemList[0].Status = ShoppingListItem.EnumStatus.NotPicked;
            shoppingList.ItemList[1].Status = ShoppingListItem.EnumStatus.Purchased;
            shoppingList.ItemList[2].Status = ShoppingListItem.EnumStatus.Purchased;

            double actualCost1 = shoppingList.getTotalCost();
            Assert.AreEqual(expectedCost1, actualCost1);

            shoppingList.ItemList[0].Status = ShoppingListItem.EnumStatus.Purchased;
            shoppingList.ItemList[1].Status = ShoppingListItem.EnumStatus.NotPicked;
            shoppingList.ItemList[2].Status = ShoppingListItem.EnumStatus.Purchased;
            double actualCost2 = shoppingList.getTotalCost();
            Assert.AreEqual(expectedCost2, actualCost2);

            shoppingList.ItemList[0].Status = ShoppingListItem.EnumStatus.Purchased;
            shoppingList.ItemList[1].Status = ShoppingListItem.EnumStatus.Purchased;
            shoppingList.ItemList[2].Status = ShoppingListItem.EnumStatus.Picked;

            double actualCost3 = shoppingList.getTotalCost();
            Assert.AreEqual(expectedCost3, actualCost3);
        }
        [TestMethod()]
        public void viewShoppingList()
        {
            bool expectedEmptyShoppingList = false;
            bool expectedNotEmptyShoppingList = true;
            //Sorting ascending first on status, then category, lastly name
            string expectedSorted1 = "Article F";
            string expectedSorted2 = "Article E";
            string expectedSorted3 = "Article C";
            string expectedSorted4 = "Article D";
            string expectedSorted5 = "Article B";
            string expectedSorted6 = "Article A";

            StoreItem storeItem6 = new StoreItem(expectedSorted6, "BBB", 1, Item.UnitEnum.Pcs);
            StoreItem storeItem5 = new StoreItem(expectedSorted5, "AAA", 1, Item.UnitEnum.Pcs);
            StoreItem storeItem4 = new StoreItem(expectedSorted4, "AAA", 1, Item.UnitEnum.Pcs);
            StoreItem storeItem3 = new StoreItem(expectedSorted3, "AAA", 1, Item.UnitEnum.Pcs);
            StoreItem storeItem1 = new StoreItem(expectedSorted1, "AAA", 1, Item.UnitEnum.Pcs);
            StoreItem storeItem2 = new StoreItem(expectedSorted2, "BBB", 1, Item.UnitEnum.Pcs);
            User customer = new User("Pelle", "p@gmail.com", false);
            ShoppingList emptyShoppingList = new ShoppingList("the shopping list", customer.UserID, new Store("Ikea"));
            ShoppingList sortedShoppingList = new ShoppingList("the shopping list", customer.UserID, new Store("Ikea"));
            sortedShoppingList.ItemList.Add(new ShoppingListItem(storeItem6, 1, customer.UserID));
            sortedShoppingList.ItemList.Add(new ShoppingListItem(storeItem5, 1, customer.UserID));
            sortedShoppingList.ItemList.Add(new ShoppingListItem(storeItem4, 1, customer.UserID));
            sortedShoppingList.ItemList.Add(new ShoppingListItem(storeItem3, 1, customer.UserID));
            sortedShoppingList.ItemList.Add(new ShoppingListItem(storeItem1, 1, customer.UserID));
            sortedShoppingList.ItemList.Add(new ShoppingListItem(storeItem2, 1, customer.UserID));
            sortedShoppingList.ItemList[0].Status = ShoppingListItem.EnumStatus.Purchased;
            sortedShoppingList.ItemList[1].Status = ShoppingListItem.EnumStatus.Purchased;
            sortedShoppingList.ItemList[2].Status = ShoppingListItem.EnumStatus.Picked;
            sortedShoppingList.ItemList[3].Status = ShoppingListItem.EnumStatus.Picked;
            sortedShoppingList.ItemList[4].Status = ShoppingListItem.EnumStatus.NotPicked;
            sortedShoppingList.ItemList[5].Status = ShoppingListItem.EnumStatus.NotPicked;

            bool actualEmptyShoppingList = emptyShoppingList.viewShoppingList();
            bool actualNotEmptyShoppingList = sortedShoppingList.viewShoppingList();

            Assert.AreEqual(expectedEmptyShoppingList, actualEmptyShoppingList);
            Assert.AreEqual(expectedNotEmptyShoppingList, actualNotEmptyShoppingList);
            Assert.AreEqual(expectedSorted1, sortedShoppingList.ItemList[0].Name);
            Assert.AreEqual(expectedSorted2, sortedShoppingList.ItemList[1].Name);
            Assert.AreEqual(expectedSorted3, sortedShoppingList.ItemList[2].Name);
            Assert.AreEqual(expectedSorted4, sortedShoppingList.ItemList[3].Name);
            Assert.AreEqual(expectedSorted5, sortedShoppingList.ItemList[4].Name);
            Assert.AreEqual(expectedSorted6, sortedShoppingList.ItemList[5].Name);
        }
        [TestMethod()]
        public void checkOutShoppingList()
        {
            ShoppingListItem.EnumStatus expectedStatus1 = ShoppingListItem.EnumStatus.Purchased;
            ShoppingListItem.EnumStatus expectedStatus2 = ShoppingListItem.EnumStatus.NotPicked;
            int expectedItemsSavedToLog = 2;
            int expectedListsLeft = 1;

            User customer = new User("Pelle", "p@gmail.com", false);
            StoreItem storeItem1 = new StoreItem("Article 1", "AAA", 1, Item.UnitEnum.Pcs);
            StoreItem storeItem2 = new StoreItem("Article 2", "AAA", 1, Item.UnitEnum.Pcs);
            Store Jula = new Store("Jula");
            Store Ikea = new Store("Ikea");
            ShoppingList listToRemain = new ShoppingList("Shopping list A", customer.UserID, Jula);
            ShoppingList listToRemove = new ShoppingList("Shopping list B", customer.UserID, Ikea);
            Store willys = new Store("Willys");
            List<Store> Storelist = new List<Store>();
            Storelist.Add(Ikea);
            Storelist.Add(Jula);
            Storelist.Add(willys);
            customer.ShoppingLists.Add(listToRemain);
            listToRemain.ItemList.Add(new ShoppingListItem(storeItem1, 1, customer.UserID));
            listToRemain.ItemList.Add(new ShoppingListItem(storeItem2, 1, customer.UserID));
            listToRemain.ItemList[0].Status = ShoppingListItem.EnumStatus.Picked;
            listToRemain.ItemList[1].Status = ShoppingListItem.EnumStatus.NotPicked;
            customer.ShoppingLists.Add(listToRemove);
            listToRemove.ItemList.Add(new ShoppingListItem(storeItem1, 1, customer.UserID));
            listToRemove.ItemList[0].Status = ShoppingListItem.EnumStatus.Picked;

            listToRemain.checkOutShoppingList(customer, listToRemain,Storelist);
            ShoppingListItem.EnumStatus actualStatus1 = listToRemain.ItemList[0].Status;
            ShoppingListItem.EnumStatus actualStatus2 = listToRemain.ItemList[1].Status;
            listToRemove.checkOutShoppingList(customer, listToRemove,Storelist);

            Assert.AreEqual(expectedStatus1, actualStatus1);
            Assert.AreEqual(expectedStatus2, actualStatus2);
            Assert.AreEqual(expectedItemsSavedToLog, customer.PurchasedItems.Count());
            Assert.AreEqual(expectedListsLeft, customer.ShoppingLists.Count());
        }
        [TestMethod()]
        public void changeStatus()
        {
            ShoppingListItem.EnumStatus expectedStatus = ShoppingListItem.EnumStatus.Picked;

            StoreItem storeItem1 = new StoreItem("Article 1", "AAA", 1, Item.UnitEnum.Pcs);
            User customer = new User("Pelle", "p@gmail.com", false);
            ShoppingList shoppingList = new ShoppingList("Shopping list A", customer.UserID, new Store("Ikea"));
            shoppingList.ItemList.Add(new ShoppingListItem(storeItem1, 1, customer.UserID));
            shoppingList.ItemList[0].Status = ShoppingListItem.EnumStatus.NotPicked;

            string simulateUserInMethod = "1\n" + "readLine\n";
            StringReader srObject = new StringReader(simulateUserInMethod);
            Console.SetIn(srObject);

            shoppingList.changeStatus();

            Assert.AreEqual(expectedStatus, shoppingList.ItemList[0].Status);
        }

    }
}
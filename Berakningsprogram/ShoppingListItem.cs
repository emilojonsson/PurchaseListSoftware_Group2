using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inköpslista_grupp2
{
    public class ShoppingListItem : Item
    {
        public double Quantity { get; set; }
        public EnumStatus Status { get; set; }
        public DateTime PurchaseDate { get; set; }//Customer Statistics in viewHistoricalPurchases()
        public ShoppingListItem(StoreItem item, double quantity)
        {
            this.Quantity = quantity;
            this.Name = item.Name;
            this.Category = item.Category;
            this.Price = item.Price;
            this.ItemID = new Guid();
            this.Unit = item.Unit;
            this.Status = EnumStatus.NotPicked;
        }

        public enum EnumStatus
        {
            NotPicked, Picked, Purchased
        }
        public override string ToString()
        {
            return $"{Name}, {Quantity} [{Unit}]";
        }
    }
}
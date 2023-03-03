using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace purchase_list_group2
{
    [DataContract]
    public class ShoppingListItem : Item
    {
        [DataMember]
        public double Quantity { get; set; }
        [DataMember]
        public EnumStatus Status { get; set; }
        [DataMember]
        public DateTime PurchaseDate { get; set; }
        [DataMember]
        public Guid CustomerID { get; set; }
        public ShoppingListItem(StoreItem item, double quantity, Guid customerID)
        {
            this.Quantity = quantity;
            this.Name = item.Name;
            this.Category = item.Category;
            this.Price = item.Price;
            this.ItemID = Guid.NewGuid();
            this.Unit = item.Unit;
            this.Status = EnumStatus.NotPicked;
            this.CustomerID = customerID;
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
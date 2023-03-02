
using System.Runtime.Serialization;

namespace purchase_list_group2
{
    [DataContract]
    abstract public class Item
    {
        [DataMember]
        public string? Name { get; set; }
        [DataMember]
        public string? Category { get; set; }
        [DataMember]
        public UnitEnum Unit { get; set; }
        [DataMember]
        public double Price { get; set; }
        [DataMember]
        public Guid ItemID { get; set; }
        public enum UnitEnum
        {
            Pcs = 0,
            Litres,
            Gram
        }
        public abstract string ToString();
    }
}
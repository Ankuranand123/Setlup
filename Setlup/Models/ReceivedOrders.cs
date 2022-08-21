namespace Setlup.Models
{
    public class ReceivedOrders
    {
        public DateTime CreatedDate { get; set; }
        public String OrderID { get; set; }
        public int Amount { get; set; }
        public int TotalItems { get; set; }
        public int OrderStatus { get; set; }
        public int PaymentMode { get; set; }
    }
}

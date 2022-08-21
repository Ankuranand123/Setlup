namespace Setlup.Models
{
    public class Customer_SuppliersList
    {
        public List<Customers> CustomersList { get; set; }

        public List<Suppliers> SuppliersList { get; set; }
    }

    public class Customers
    {
        public string ObjUniqueId { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public int Status { get; set; } = 1;  // To differniate b/w C/S
        public int AccountStatus { get; set; } // Whether user has setlup account or not

        public int Amount { get; set; }

        public int DueorAdvance { get; set; }  // Due 1 Advance 2 NoBalanace 3

    }

    public class Suppliers
    {
        public string ObjUniqueId { get; set; } = string.Empty;
        public string SupplierName { get; set; } = string.Empty;
        public int Status { get; set; } = 2;
        public int AccountStatus { get; set; }

        public int Amount { get; set; }

        public int DueorAdvance { get; set; }

    }

}

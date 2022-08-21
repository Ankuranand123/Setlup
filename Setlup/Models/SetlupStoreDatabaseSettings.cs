namespace Setlup.Models
{
    public class SetlupStoreDatabaseSettings :ISetlupStoreDatabaseSettings
    {
        public string StudentCoursesCollectionName { get; set; } = String.Empty;
        public string userDetailsCollectionName { get; set; } = String.Empty;

       public string UserCustomerSuppliersCollectionName { get; set; } = String.Empty;

      public string MessageTextCollectionName { get; set; } = String.Empty;
        public string ConnectionString { get; set; } = String.Empty;
        public string DatabaseName { get; set; } = String.Empty;

        public string OrdersCollectionName { get; set; } = String.Empty;

        public string InventoryCollectionName { get; set; } = String.Empty;

        public string BusinessMasterCollectionName { get; set; } = String.Empty;
        public string StatesCollectionName { get; set; } = String.Empty;

        public string InvoiceCollectionName { get; set; } = String.Empty;

    }
}

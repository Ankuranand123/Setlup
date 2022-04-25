namespace Setlup.Models
{
    public interface ISetlupStoreDatabaseSettings
    {
        string StudentCoursesCollectionName { get; set; }
        string userDetailsCollectionName { get; set; }

        string UserCustomerSuppliersCollectionName { get; set; }

        string OrdersCollectionName { get; set; }

        string  InventoryCollectionName { get; set; }   

        string  MessageTextCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }

        string BusinessMasterCollectionName { get; set; }

    }
}

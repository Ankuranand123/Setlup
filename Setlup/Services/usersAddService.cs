using MongoDB.Bson;
using MongoDB.Driver;
using Setlup.Models;
using Setlup.Utilities;


namespace Setlup.Services
{
    public class usersAddService : IusersAddService
    {

        private readonly IMongoCollection<userMobileDetails> _userMobileDetails;
        private readonly IMongoCollection<userDetails> _userDetails;
        private readonly IMongoCollection<Users_CustomerSuppliers> _userCustomerSuppliers;
        private readonly IMongoCollection<BusinessDetailsList> _businessType;
        //private readonly IMongoDatabase _database;

        public usersAddService(ISetlupStoreDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _userMobileDetails = database.GetCollection<userMobileDetails>(settings.StudentCoursesCollectionName);
            _userDetails = database.GetCollection<userDetails>(settings.userDetailsCollectionName);
            _userCustomerSuppliers = database.GetCollection<Users_CustomerSuppliers>(settings.UserCustomerSuppliersCollectionName);
            _businessType = database.GetCollection<BusinessDetailsList>(settings.BusinessMasterCollectionName);
        }

        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

        public string InsertUser(userMobileDetails mobileDetails)
        {
            try
            {
                //Check if the user which is signing in already exists in database
                FilterDefinition<userMobileDetails> filter = Builders<userMobileDetails>.Filter.Where(x => x.mobileNumber == mobileDetails.mobileNumber);
                var li = _userMobileDetails.Find(filter).ToList();
                if (li.Count > 0)
                {
                    //user already exists
                    //Check status of the user whether it is 1 or 0 .
                    if (li[0].status == 0)
                    {
                        // Some other user has added this user as customer or suppliers but this user has not registered himself in Setlup yet so Status = 0
                        var update = Builders<userMobileDetails>.Update.Set(p => p.status, 1);
                        var options = new UpdateOptions { IsUpsert = true };
                        _userMobileDetails.UpdateOne(filter, update, options);

                        FilterDefinition<Users_CustomerSuppliers> filter1 = Builders<Users_CustomerSuppliers>.Filter.Where(x => x.Customer_SupplierId == li[0].userId);
                        var update1 = Builders<Users_CustomerSuppliers>.Update.Set(p => p.Status, 1);
                        var options1 = new UpdateOptions { IsUpsert = true };
                        _userCustomerSuppliers.UpdateOne(filter1, update1, options1);
                        string s1 = cryptingData.Encrypt(li[0].userId);
                        return s1;


                    }
                    else
                    {
                        //Status = 1
                        string s1 = cryptingData.Encrypt(li[0].userId);
                        return s1;
                    }

                }
                else
                {
                    //User is a new one
                    mobileDetails.status = 1;
                    mobileDetails.CreatedDate = DateTime.Now;
                    // mobileDetails.CreatedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);


                    _userMobileDetails.InsertOne(mobileDetails);
                    var id = mobileDetails.userId;
                    string s1 = cryptingData.Encrypt(id);
                    return s1;
                }
            }
            catch(Exception ex)
            {
                String str = "Exception";
                return str;
            }
        }

        public string InsertUserBusinessTypeDetails(string userId,userDetails userDetails)
        {
            try
            {
                var uid = cryptingData.Decrypt(userId);

                // check if that particular user exists in table
                FilterDefinition<userDetails> filter = Builders<userDetails>.Filter.Where(x => x.userId == uid);
                var li = _userDetails.Find(filter).ToList();
                string ret = "";
                if (li.Count == 0)
                {
                    //insert  

                    userDetails.userId = uid;
                    userDetails.CreatedDate = DateTime.Now;
                    _userDetails.InsertOne(userDetails);

                    ret = userDetails.businessId;
                    return ret;

                }
                else
                {
                    //update
                    FilterDefinition<userDetails> updatefilter = Builders<userDetails>.Filter.Eq(x => x.userId, uid);
                    var update = Builders<userDetails>.Update.Set(p => p.businessId, userDetails.businessId);
                    var options = new UpdateOptions { IsUpsert = true };
                    _userDetails.UpdateOne(updatefilter, update, options);
                    ret = userDetails.businessId;
                    return ret;
                }
            }catch(Exception ex)
            {
                string str = "Exception";
                return str;
            }

           
        }

        public string InserUserDetails(string userId, userDetails userDetails)
        {

            try
            {
                var uid = cryptingData.Decrypt(userId);

                // check if that particular user exists in table
                FilterDefinition<userDetails> filter = Builders<userDetails>.Filter.Where(x => x.userId == uid);
                var li = _userDetails.Find(filter).ToList();
                string ret = "";

                if (li.Count == 0)
                {
                    //insert  

                    userDetails.userId = uid;
                    userDetails.CreatedDate = DateTime.Now;
                    _userDetails.InsertOne(userDetails);
                    ret = userDetails.name;
                    return ret;
                }
                else
                {
                    //update
                    FilterDefinition<userDetails> updatefilter = Builders<userDetails>.Filter.Eq(x => x.userId, uid);
                    var update = Builders<userDetails>.Update
                        .Set(p => p.address, userDetails.address).Set(p => p.name, userDetails.name).Set(p => p.email, userDetails.email)
                        .Set(p => p.designation, userDetails.designation).Set(p => p.panNumber, userDetails.panNumber).Set(p => p.brandDeals, userDetails.brandDeals);
                    var options = new UpdateOptions { IsUpsert = true };
                    _userDetails.UpdateOne(updatefilter, update, options);

                    ret = userDetails.name;
                    return ret;
                }
            }catch (Exception ex)
            {
                string str = "Exception";
                return str;
            }
        }
        public string GetDetails(string id)
        {
            FilterDefinition<userMobileDetails> filter = Builders<userMobileDetails>.Filter.Where(x => x.userId == id);
          var s1 =   _userMobileDetails.Find(filter).FirstOrDefault();
            return s1.mobileNumber;

        }

        public string InsertCustomerSupplier(string userId, Users_CustomerSuppliers objCustomerSuppliers)
        {

            try
            {


                //Decrypting to get user ID
                var uid = cryptingData.Decrypt(userId);
                FilterDefinition<userDetails> filterUserDetails = Builders<userDetails>.Filter.Where(x => x.userId == uid);
                var liUserDetails = _userDetails.Find(filterUserDetails).ToList();
                if (liUserDetails.Count == 0)
                {
                    //user has not provided his basic details.User info does not exist in database
                    return "Please provide your details";
                }
                else
                {
                    //User has provided info. User details is present in database...need to check whether he has provided both business role type and his details like name, pan etc
                    if (string.IsNullOrEmpty(liUserDetails[0].name) || string.IsNullOrEmpty(liUserDetails[0].businessId))
                    {
                        //either name or businessID is not provided..will not allow user to add C/S
                        return "Please provide your details";
                    }
                    else
                    {
                        //all info exists...in this case only user can add C/S

                        FilterDefinition<userMobileDetails> filter = Builders<userMobileDetails>.Filter.Eq(x => x.mobileNumber, objCustomerSuppliers.AddedUserPh);
                        var li = _userMobileDetails.Find(filter).ToList();
                        if (li.Count > 0)
                        {
                            //if user which is getting added is registerd in Setlup  mobile no. is there in database and status can be anything
                            objCustomerSuppliers.Customer_SupplierId = li[0].userId;
                            objCustomerSuppliers.Status = li[0].status;
                            objCustomerSuppliers.UserId = uid;
                            //Unique combination of userId and Customer/Supplier should be there
                            FilterDefinition<Users_CustomerSuppliers> CSfilter_UserId = Builders<Users_CustomerSuppliers>.Filter.Eq(x => x.UserId, uid);
                            FilterDefinition<Users_CustomerSuppliers> CSfilter_CS_Id = Builders<Users_CustomerSuppliers>.Filter.Eq(x => x.Customer_SupplierId, objCustomerSuppliers.Customer_SupplierId);
                            var record = _userCustomerSuppliers.Find(CSfilter_UserId & CSfilter_CS_Id).ToList();
                            if (record.Count > 0)
                            {
                                // if the combination already exists , no need to insert
                                return "User already exists";
                            }
                            else
                            {
                                // if such combination does not exist
                                _userCustomerSuppliers.InsertOne(objCustomerSuppliers);
                                return "Inserted";

                            }

                        }
                        else
                        {
                            // if user which is getting added is not registered in Setlup ...mobile no. is not in database
                            userMobileDetails objmobiledetails = new userMobileDetails();
                            objmobiledetails.mobileNumber = objCustomerSuppliers.AddedUserPh;
                            objmobiledetails.status = 0;
                            objmobiledetails.CreatedDate = DateTime.Now;
                            _userMobileDetails.InsertOne(objmobiledetails);
                            objCustomerSuppliers.Customer_SupplierId = objmobiledetails.userId;
                            objCustomerSuppliers.Status = objmobiledetails.status;
                            objCustomerSuppliers.UserId = uid;
                            _userCustomerSuppliers.InsertOne(objCustomerSuppliers);
                            return "Inserted";
                        }



                    }


                }
            }
            catch (Exception ex)
            {
                string ret = "Exception";
                    return ret;
            }




        }

        public Customer_SuppliersList GetCustomerSuppliers(string userId)
        {
            var uid = cryptingData.Decrypt(userId);
            Customer_SuppliersList CSList = new Customer_SuppliersList();

            //Find all the records in Customer Suppliers collection where uid is present either in UserId or Customer_SupplierId
            var filter = Builders<Users_CustomerSuppliers>.Filter.Where(x=>x.UserId == uid || x.Customer_SupplierId == uid);

            var allRecords = _userCustomerSuppliers.Find(filter).ToList();

            //where uid is present in UserId column--->uid has customers / Suppliers
           var CustomersList_1 =  allRecords.Where(x=>x.UserId == uid && x.Customer_or_Supplier == 1); // to get customers  CustomerID

            var SuppliersList_1 = allRecords.Where(x => x.UserId == uid && x.Customer_or_Supplier == 2); // to get Suppliers SupplierID

            //where uid is present in CustomerSupplierId  column ----> some other user has added uid either as Customer/Supplier

            var SuppliersList_2 =    allRecords.Where(x => x.Customer_SupplierId == uid && x.Customer_or_Supplier == 1); // to get Suppliers CustomerID

            var CustomersList_2 = allRecords.Where(x => x.Customer_SupplierId == uid && x.Customer_or_Supplier == 2); // to get Customers SupplierID

            List<Customers> CustomersList = new List<Customers>();

            foreach(var customer in CustomersList_1)
            {
                Customers c1 = new Customers();
                c1.ObjUniqueId = customer.CustomerSuppliersId;
                c1.CustomerName = customer.AddedUserName;
                c1.Status = 1;
                c1.AccountStatus = customer.Status;
                CustomersList.Add(c1);
             
            }
            foreach(var customer in CustomersList_2)
            {

                Customers c2 = new Customers();
                c2.ObjUniqueId = customer.CustomerSuppliersId;
                c2.Status = 1;
                c2.AccountStatus = customer.Status;

                var filter1 = Builders<userDetails>.Filter.Where(x => x.userId == customer.UserId);
                c2.CustomerName = _userDetails.Find(filter1).FirstOrDefault().name;
                CustomersList.Add(c2);


            }

            List<Suppliers> SuppliersList = new List<Suppliers>();

            foreach(var supplier in SuppliersList_1)
            {
                Suppliers s1 = new Suppliers();
                s1.ObjUniqueId = supplier.CustomerSuppliersId;
                s1.SupplierName = supplier.AddedUserName;
                s1.Status = 2;
                s1.AccountStatus = supplier.Status;
                SuppliersList.Add(s1);

            }

            foreach(var supplier in SuppliersList_2)
            {
                Suppliers s2 = new Suppliers();
                s2.ObjUniqueId = supplier.CustomerSuppliersId;
                s2.Status = 2;
                s2.AccountStatus = supplier.Status;
                var filter1 = Builders<userDetails>.Filter.Where(x => x.userId == supplier.UserId);
                s2.SupplierName = _userDetails.Find(filter1).FirstOrDefault().name;
                SuppliersList.Add(s2);


            }

            CSList.CustomersList = CustomersList;
            CSList.SuppliersList = SuppliersList;



            return CSList;

        }

        public userDetails GetUserDetails(string Userid)
        {
            var uid = cryptingData.Decrypt(Userid);
            FilterDefinition<userDetails> filter = Builders<userDetails>.Filter.Where(x => x.userId == uid);
          userDetails UserDetails =   _userDetails.Find(filter).FirstOrDefault();
            return UserDetails;

        }

        public BusinessTypeDetails GetBusinessType()
        {
            BusinessTypeDetails Objbd = new BusinessTypeDetails();
            try
            {

                var li = _businessType.Find(_ => true).ToList();

                Objbd.BusinessDetailsList = li;
                return Objbd;
            }catch (Exception ex)
            {
                Objbd = null;
                return Objbd;
            }


        }
    }
}

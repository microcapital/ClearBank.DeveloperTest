using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace ClearBank.DeveloperTest.Services
{
    public  class AccountDataStoreFactory : IAccountDataStoreFactory
    {
        public IAccountDataStore Create()
        {

            var dataStoreType = ConfigurationManager.AppSettings["DataStoreType"];

            if (dataStoreType == "Backup")
                return new BackupAccountDataStore();
            return new AccountDataStore();
        }
    }
}

using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace StoresAndBrands
{
  public class StoreTest : IDisposable
  {
    public StoreTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=store_brands_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void test_coursesEmptyAtFirst()
    {
      int result = Store.GetAll().Count;

      Assert.Equal(0, result);
    }

    public void Dispose()
    {
      Store.DeleteAll();
    }
  }
}

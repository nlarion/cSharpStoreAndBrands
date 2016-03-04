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
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=shoe_stores_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void test_coursesEmptyAtFirst()
    {
      int result = Store.GetAll().Count;

      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueForSameName()
    {
      Store firstStore = new Store("Payless");
      Store secondStore = new Store("Payless");

      Assert.Equal(firstStore, secondStore);
    }

    [Fact]
    public void Test_Save_SavesStoreToDatabase()
    {
      Store testStore = new Store("DSW");
      testStore.Save();

      List<Store> result = Store.GetAll();
      List<Store> testList = new List<Store>{testStore};
      
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Save_AssignsIdToStoreObject()
    {
      Store testStore = new Store("DSW");
      testStore.Save();

      Store savedStore = Store.GetAll()[0];

      int result = savedStore.GetId();
      int testId = testStore.GetId();

      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_Find_FindsStoreInDatabase()
    {
      //Arrange
      Store testStore = new Store("FootLocker");
      testStore.Save();

      //Act
      Store foundStore = Store.Find(testStore.GetId());

      //Assert
      Assert.Equal(testStore, foundStore);
    }

    [Fact]
    public void Test_Delete_DeletesStoreFromDatabase()
    {
    //Arrange
    Store testStore1 = new Store("Nordstrom Rack");
    testStore1.Save();

    Store testStore2 = new Store("Ladies FootLocker");
    testStore2.Save();

    //Act
    testStore1.Delete();
    List<Store> resultcourses = Store.GetAll();
    List<Store> testStoreList = new List<Store> {testStore2};

    //Assert
    Assert.Equal(testStoreList, resultcourses);
     }

    public void Dispose()
    {
      Store.DeleteAll();
    }
  }
}

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
    public void Test_Update_UpdatesStoreObject()
    {
      Store testStore = new Store("FootLocker");
      testStore.Save();

      testStore.Update("Payless");

      Store afterStore = Store.Find(testStore.GetId());

      Assert.Equal(afterStore.GetName(), "Payless");
    }

    [Fact]
    public void Test_Update_UpdatesStoreObjectInDB()
    {
      Store testStore = new Store("FootLocker");
      testStore.Save();

      testStore.Update("Payless");

      Store afterStore = Store.Find(testStore.GetId());

      Store testStore2 = new Store("Payless", testStore.GetId());
      // Console.WriteLine(afterStore.GetId());
      // Console.WriteLine(testStore2.GetId());
      Assert.Equal(afterStore, testStore2);
    }

    [Fact]
    public void Test_GetBrands_RetrievesAllBrandsWithStore()
    {
      Store testStore = new Store("FootLocker");
      testStore.Save();

      Brand firstBrand = new Brand("Asic");
      firstBrand.Save();
      Brand secondBrand = new Brand("Timberland");
      secondBrand.Save();

      testStore.AddBrand(firstBrand);
      List<Brand> testBrandList = new List<Brand> {firstBrand};
      List<Brand> resultBrandList = testStore.GetBrands();

      Assert.Equal(testBrandList, resultBrandList);
    }

    [Fact]
    public void Test_AddBrand_AddsBrandToStore()
    {
      //Arrange
      Store testStore = new Store("Payless");
      testStore.Save();

      Brand testBrand = new Brand("Nike");
      testBrand.Save();

      Brand testBrand2 = new Brand("Adidas");
      testBrand2.Save();

      //Act
      testStore.AddBrand(testBrand);
      testStore.AddBrand(testBrand2);

      List<Brand> result = testStore.GetBrands();
      List<Brand> testList = new List<Brand>{testBrand, testBrand2};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Delete_DeletesStoreAssociationsFromDatabase()
    {
      Brand testBrand = new Brand("Nike");
      testBrand.Save();

      Store testStore = new Store("Nike Outlet");
      testStore.Save();

      testStore.AddBrand(testBrand);
      testStore.Delete();

      List<Store> resultBrandcourses = testBrand.GetStore();
      List<Store> testBrandcourses = new List<Store> {};

      Assert.Equal(testBrandcourses, resultBrandcourses);
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

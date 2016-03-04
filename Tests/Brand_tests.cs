using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace StoresAndBrands
{
  public class BrandTest : IDisposable
  {
    public BrandTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=shoe_stores_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void test_BrandsEmptyAtFirst()
    {
      int result = Brand.GetAll().Count;

      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueForSameName()
    {
      Brand firstBrand = new Brand("Nike");
      Brand secondBrand = new Brand("Nike");

      Assert.Equal(firstBrand, secondBrand);
    }

    [Fact]
    public void Test_Save_SavesBrandToDatabase()
    {
      Brand testBrand = new Brand("Adidas");
      testBrand.Save();

      List<Brand> result = Brand.GetAll();
      List<Brand> testList = new List<Brand>{testBrand};
      
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Save_AssignsIdToBrandObject()
    {
      Brand testBrand = new Brand("Adidas");
      testBrand.Save();

      Brand savedBrand = Brand.GetAll()[0];

      int result = savedBrand.GetId();
      int testId = testBrand.GetId();

      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_Find_FindsBrandInDatabase()
    {
      //Arrange
      Brand testBrand = new Brand("Asic");
      testBrand.Save();

      //Act
      Brand foundBrand = Brand.Find(testBrand.GetId());

      //Assert
      Assert.Equal(testBrand, foundBrand);
    }

    [Fact]
    public void Test_Update_UpdatesBrandObject()
    {
      Brand testBrand = new Brand("Asic");
      testBrand.Save();

      testBrand.Update("Nike");

      Brand afterBrand = Brand.Find(testBrand.GetId());

      Assert.Equal(afterBrand.GetName(), "Nike");
    }

    [Fact]
    public void Test_Update_UpdatesBrandObjectInDB()
    {
      Brand testBrand = new Brand("Asic");
      testBrand.Save();

      testBrand.Update("Nike");

      Brand afterBrand = Brand.Find(testBrand.GetId());

      Brand testBrand2 = new Brand("Nike", testBrand.GetId());
      // Console.WriteLine(afterBrand.GetId());
      // Console.WriteLine(testBrand2.GetId());
      Assert.Equal(afterBrand, testBrand2);
    }

    [Fact]
    public void Test_GetStores_RetrievesAllStoresWithBrand()
    {
      Brand testBrand = new Brand("Asic");
      testBrand.Save();

      Store firstStore = new Store("Payless");
      firstStore.Save();
      Store secondStore = new Store("FootLocker");
      secondStore.Save();

      testBrand.AddStore(firstStore);
      List<Store> testStoreList = new List<Store> {firstStore};
      List<Store> resultStoreList = testBrand.GetStores();

      Assert.Equal(testStoreList, resultStoreList);
    }

    [Fact]
    public void Test_AddStore_AddsStoreToBrand()
    {
      //Arrange
      Brand testBrand = new Brand("Nike");
      testBrand.Save();

      Store testStore = new Store("Payless");
      testStore.Save();

      Store testStore2 = new Store("DSW");
      testStore2.Save();

      //Act
      testBrand.AddStore(testStore);
      testBrand.AddStore(testStore2);

      List<Store> result = testBrand.GetStores();
      List<Store> testList = new List<Store>{testStore, testStore2};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Delete_DeletesBrandAssociationsFromDatabase()
    {
      Brand testBrand = new Brand("Nike");
      testBrand.Save();

      Store testStore = new Store("Nike Outlet");
      testStore.Save();

      testBrand.AddStore(testStore);
      testBrand.Delete();

      List<Brand> resultStoreBrands = testStore.GetBrands();
      List<Brand> testStoreBrands = new List<Brand> {};

      Assert.Equal(testStoreBrands, resultStoreBrands);
    }

    [Fact]
    public void Test_Delete_DeletesBrandFromDatabase()
    {
      //Arrange
      Brand testBrand1 = new Brand("Nike");
      testBrand1.Save();

      Brand testBrand2 = new Brand("Asic");
      testBrand2.Save();

      //Act
      testBrand1.Delete();
      List<Brand> resultcourses = Brand.GetAll();
      List<Brand> testBrandList = new List<Brand> {testBrand2};

      //Assert
      Assert.Equal(testBrandList, resultcourses);
    }

    public void Dispose()
    {
      Brand.DeleteAll();
    }
  }
}

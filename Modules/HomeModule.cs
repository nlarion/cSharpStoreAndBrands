using System;
using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;

namespace StoresAndBrands
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        return View["index.cshtml"];
      };
      Get["/Stores"]= _ =>{
        Dictionary<string, object> returnDictionary = new Dictionary<string, object> ();
        List<Brand> brandList = Brand.GetAll();
        List<Store> storeList = Store.GetAll();
        returnDictionary.Add("brandList", brandList);
        returnDictionary.Add("storeList", storeList);
        return View["stores.cshtml", returnDictionary];
      };
      Post["/Stores"]= _ =>{
        Dictionary<string, object> returnDictionary = new Dictionary<string, object> ();
        Store newStore = new Store(Request.Form["store-name"]);
        newStore.Save();
        if(Request.Form["brand-name"] != null)
        {
          Brand newBrand = Brand.Find(Request.Form["brand-name"]);
          newBrand.AddStore(newStore);
        }
        List<Brand> brandList = Brand.GetAll();
        List<Store> storeList = Store.GetAll();
        returnDictionary.Add("brandList", brandList);
        returnDictionary.Add("storeList", storeList);
        return View["stores.cshtml", returnDictionary];
      };
      Get["/Stores/{id}"]= parameters =>{
        Dictionary<string, object> returnDictionary = new Dictionary<string, object> ();
        List<Brand> allBrands = Brand.GetAll();
        Store foundStore = Store.Find(parameters.id);
        List<Brand> foundBrands = foundStore.GetBrands();
        returnDictionary.Add("foundStore", foundStore);
        returnDictionary.Add("foundBrands", foundBrands);
        returnDictionary.Add("allBrands", allBrands);
        return View["storesDetail.cshtml", returnDictionary];
      };
      Post["/Stores/{id}"]= parameters =>{
        Store foundStore = Store.Find(parameters.id);
        foundStore.Update(Request.Form["store-name"]);
        Dictionary<string, object> returnDictionary = new Dictionary<string, object> ();
        List<Brand> allBrands = Brand.GetAll();
        List<Brand> foundBrands = foundStore.GetBrands();
        returnDictionary.Add("foundStore", foundStore);
        returnDictionary.Add("foundBrands", foundBrands);
        returnDictionary.Add("allBrands", allBrands);
        return View["storesDetail.cshtml", returnDictionary];
      };
      Post["/Stores/AddBrand/{id}"]= parameters =>{
        Store foundStore = Store.Find(parameters.id);
        if(Request.Form["brand-name"] != null)
        {
          Brand newBrand = Brand.Find(Request.Form["brand-name"]);
          foundStore.AddBrand(newBrand);
        }
        Dictionary<string, object> returnDictionary = new Dictionary<string, object> ();
        List<Brand> allBrands = Brand.GetAll();
        List<Brand> foundBrands = foundStore.GetBrands();
        returnDictionary.Add("foundStore", foundStore);
        returnDictionary.Add("foundBrands", foundBrands);
        returnDictionary.Add("allBrands", allBrands);
        return View["storesDetail.cshtml", returnDictionary];
      };
      Get["/Stores/Delete/{id}"]= parameters =>{
        Store foundStore = Store.Find(parameters.id);
        foundStore.Delete();
        Dictionary<string, object> returnDictionary = new Dictionary<string, object> ();
        List<Brand> brandList = Brand.GetAll();
        List<Store> storeList = Store.GetAll();
        returnDictionary.Add("brandList", brandList);
        returnDictionary.Add("storeList", storeList);
        return View["stores.cshtml", returnDictionary];
      };
      Get["/Brands"]= _ =>{
        Dictionary<string, object> returnDictionary = new Dictionary<string, object> ();
        List<Brand> brandList = Brand.GetAll();
        List<Store> storeList = Store.GetAll();
        returnDictionary.Add("brandList", brandList);
        returnDictionary.Add("storeList", storeList);
        return View["brands.cshtml", returnDictionary];
      };
      Post["/Brands"]= _ =>{
        Dictionary<string, object> returnDictionary = new Dictionary<string, object> ();
        Brand newBrands = new Brand(Request.Form["brand-name"]);
        newBrands.Save();
        if(Request.Form["store-name"] != null)
        {
          Store newStore = Store.Find(Request.Form["store-name"]);
          newStore.AddBrand(newBrands);
        }
        List<Brand> brandList = Brand.GetAll();
        List<Store> storeList = Store.GetAll();
        returnDictionary.Add("brandList", brandList);
        returnDictionary.Add("storeList", storeList);
        return View["brands.cshtml", returnDictionary];
      };
      Post["/Brands/{id}"]= parameters =>{
        Brand foundBrand = Brand.Find(parameters.id);
        foundBrand.Update(Request.Form["brand-name"]);
        Dictionary<string, object> returnDictionary = new Dictionary<string, object> ();
        List<Store> foundStores = foundBrand.GetStores();
        List<Store> allStores = Store.GetAll();
        returnDictionary.Add("foundStores", foundStores);
        returnDictionary.Add("foundBrand", foundBrand);
        returnDictionary.Add("allStores", allStores);
        return View["brandsDetail.cshtml", returnDictionary];
      };      
      Post["/Brands/AddStore/{id}"]= parameters =>{
        Brand foundBrand = Brand.Find(parameters.id);
        if(Request.Form["store-name"] != null)
        {
          Store newStore = Store.Find(Request.Form["store-name"]);
          foundBrand.AddStore(newStore);
        }
        Dictionary<string, object> returnDictionary = new Dictionary<string, object> ();
        List<Store> foundStores = foundBrand.GetStores();
        List<Store> allStores = Store.GetAll();
        returnDictionary.Add("foundStores", foundStores);
        returnDictionary.Add("foundBrand", foundBrand);
        returnDictionary.Add("allStores", allStores);
        return View["brandsDetail.cshtml", returnDictionary];
      };
      Get["/Brands/{id}"]= parameters =>{
        Dictionary<string, object> returnDictionary = new Dictionary<string, object> ();
        Brand foundBrand = Brand.Find(parameters.id);
        List<Store> foundStores = foundBrand.GetStores();
        List<Store> allStores = Store.GetAll();
        returnDictionary.Add("foundStores", foundStores);
        returnDictionary.Add("foundBrand", foundBrand);
        returnDictionary.Add("allStores", allStores);
        return View["brandsDetail.cshtml", returnDictionary];
      };
      Get["/Brands/Delete/{id}"]= parameters =>{
        Brand foundBrands = Brand.Find(parameters.id);
        foundBrands.Delete();
        Dictionary<string, object> returnDictionary = new Dictionary<string, object> ();
        List<Brand> brandList = Brand.GetAll();
        List<Store> storeList = Store.GetAll();
        returnDictionary.Add("brandList", brandList);
        returnDictionary.Add("storeList", storeList);
        return View["brands.cshtml", returnDictionary];
      };
    }
  }
}
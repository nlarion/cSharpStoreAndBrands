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
        if(Request.Form["brand-name"] != null)
        {
          Brand newBrand = Brand.Find(Request.Form["brand-name"]);
          newBrand.AddStore(newStore);
        }
        newStore.Save();
        List<Brand> brandList = Brand.GetAll();
        List<Store> storeList = Store.GetAll();
        returnDictionary.Add("brandList", brandList);
        returnDictionary.Add("storeList", storeList);
        return View["stores.cshtml", returnDictionary];
      };
      Get["/Stores/{id}"]= parameters =>{
        Dictionary<string, object> returnDictionary = new Dictionary<string, object> ();
        Store foundStore = Store.Find(parameters.id);
        List<Brand> storesBrands = foundStore.GetBrands();
        returnDictionary.Add("foundStore", foundStore);
        returnDictionary.Add("storesBrands", storesBrands);
        return View["storesDetail.cshtml", returnDictionary];
      };
      Post["/Stores/{id}"]= parameters =>{
        Store foundStore = Store.Find(parameters.id);
        foundStore.Update(Request.Form["store-name"]);
        Dictionary<string, object> returnDictionary = new Dictionary<string, object> ();
        List<Brand> storesBrands = foundStore.GetBrands();
        returnDictionary.Add("foundStore", foundStore);
        returnDictionary.Add("storesBrands", storesBrands);
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
    }
  }
}
using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace StoresAndBrands
{
  public class Store
  {
    private int _id;
    private string _name;

    public Store(string name, int id = 0)
    {
      _id = id;
      _name = name;
    }

    public override bool Equals(System.Object otherStore)
    {
      if(!(otherStore is Store))
      {
        return false;
      }
      else
      {
        Store newStore = (Store) otherStore;
        bool idEquality = this.GetId() == newStore.GetId();
        bool nameEquality = this.GetName() == newStore.GetName();
        return (idEquality && nameEquality);
      }
    }
    public int GetId()
    {
      return _id;
    }
    public string GetName()
    {
      return _name;
    }
    public void SetName(string newName)
    {
      _name = newName;
    }
    public static List<Store> GetAll()
    {
      List<Store> allStores = new List<Store>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM stores;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int storeId = rdr.GetInt32(0);
        string storeName = rdr.GetString(1);
        Store newStore = new Store(storeName, storeId);
        allStores.Add(newStore);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }

      return allStores;
    }
    public static Store Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM stores WHERE id = @StoreId;", conn);
      SqlParameter storeIdParameter = new SqlParameter();
      storeIdParameter.ParameterName = "@StoreId";
      storeIdParameter.Value = id.ToString();
      cmd.Parameters.Add(storeIdParameter);
      rdr = cmd.ExecuteReader();

      int foundStoreId = 0;
      string foundStoreName = null;

      while(rdr.Read())
      {
        foundStoreId = rdr.GetInt32(0);
        foundStoreName = rdr.GetString(1);
      }
      Store foundStore = new Store(foundStoreName, foundStoreId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundStore;
    }
    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO stores (name) OUTPUT INSERTED.id VALUES (@StoreName);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@StoreName";
      nameParameter.Value = this.GetName();
      cmd.Parameters.Add(nameParameter);

      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
    }
    public void Update(string storeName)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("update stores set name=@NewStore where id = @StoreId;", conn);

      SqlParameter newStoreParameter = new SqlParameter();
      newStoreParameter.ParameterName = "@NewStore";
      newStoreParameter.Value = storeName;
      cmd.Parameters.Add(newStoreParameter);

      SqlParameter storeIdParameter = new SqlParameter();
      storeIdParameter.ParameterName = "@StoreId";
      storeIdParameter.Value = this._id;
      cmd.Parameters.Add(storeIdParameter);

      cmd.ExecuteNonQuery();

      this._name = storeName;

      if (conn != null)
      {
        conn.Close();
      }
    }
    public List<Brand> GetBrands()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();
      List<Brand> brands = new List<Brand>{};
      SqlCommand cmd = new SqlCommand("SELECT b.id, b.name from brands b join brands_stores sb on (sb.brands_id = b.id) join stores s on (s.id = sb.stores_id) WHERE s.id = @StoreId", conn);

      SqlParameter storeIdParameter = new SqlParameter();
      storeIdParameter.ParameterName = "@StoreId";
      storeIdParameter.Value = this.GetId();
      cmd.Parameters.Add(storeIdParameter);
      rdr = cmd.ExecuteReader();
      while (rdr.Read())
      {
        int storeId = rdr.GetInt32(0);
        string studentName = rdr.GetString(1);
        Brand newBrand= new Brand(studentName, storeId);
        brands.Add(newBrand);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return brands;
    }
    public void AddBrand(Brand newBrand)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("INSERT INTO brands_stores (stores_id, brands_id) VALUES (@StoreId, @BrandId)", conn);
      SqlParameter storeIdParameter = new SqlParameter();
      storeIdParameter.ParameterName = "@StoreId";
      storeIdParameter.Value = this.GetId();
      cmd.Parameters.Add(storeIdParameter);

      SqlParameter brandIdParameter = new SqlParameter();
      brandIdParameter.ParameterName = "@BrandId";
      brandIdParameter.Value = newBrand.GetId();
      cmd.Parameters.Add(brandIdParameter);

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }
    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM stores WHERE id = @StoreId; DELETE FROM brands_stores WHERE stores_id = @StoreId;", conn);

      SqlParameter storeIdParameter = new SqlParameter();
      storeIdParameter.ParameterName = "@StoreId";
      storeIdParameter.Value = this.GetId();
      cmd.Parameters.Add(storeIdParameter);

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }
    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM stores;", conn);
      cmd.ExecuteNonQuery();
    }
  }
}
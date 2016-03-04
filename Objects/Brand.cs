using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace StoresAndBrands
{
  public class Brand
  {
    private int _id;
    private string _name;

    public Brand(string name, int id = 0)
    {
      _id = id;
      _name = name;
    }

    public override bool Equals(System.Object otherBrand)
    {
      if(!(otherBrand is Brand))
      {
        return false;
      }
      else
      {
        Brand newBrand = (Brand) otherBrand;
        bool idEquality = this.GetId() == newBrand.GetId();
        bool nameEquality = this.GetName() == newBrand.GetName();
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
    public static List<Brand> GetAll()
    {
      List<Brand> allBrands = new List<Brand>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM brands;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int brandId = rdr.GetInt32(0);
        string brandName = rdr.GetString(1);
        Brand newBrand = new Brand(brandName, brandId);
        allBrands.Add(newBrand);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }

      return allBrands;
    }
    public static Brand Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM brands WHERE id = @BrandId;", conn);
      SqlParameter brandIdParameter = new SqlParameter();
      brandIdParameter.ParameterName = "@BrandId";
      brandIdParameter.Value = id.ToString();
      cmd.Parameters.Add(brandIdParameter);
      rdr = cmd.ExecuteReader();

      int foundBrandId = 0;
      string foundBrandName = null;

      while(rdr.Read())
      {
        foundBrandId = rdr.GetInt32(0);
        foundBrandName = rdr.GetString(1);
      }
      Brand foundBrand = new Brand(foundBrandName, foundBrandId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundBrand;
    }
    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO brands (name) OUTPUT INSERTED.id VALUES (@BrandName);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@BrandName";
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
    public void Update(string brandName)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("update brands set name=@NewBrand where id = @BrandId;", conn);

      SqlParameter newBrandParameter = new SqlParameter();
      newBrandParameter.ParameterName = "@NewBrand";
      newBrandParameter.Value = brandName;
      cmd.Parameters.Add(newBrandParameter);

      SqlParameter brandIdParameter = new SqlParameter();
      brandIdParameter.ParameterName = "@BrandId";
      brandIdParameter.Value = this._id;
      cmd.Parameters.Add(brandIdParameter);

      cmd.ExecuteNonQuery();

      this._name = brandName;

      if (conn != null)
      {
        conn.Close();
      }
    }
    public List<Store> GetStores()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();
      List<Store> stores = new List<Store>{};
      SqlCommand cmd = new SqlCommand("SELECT s.id, s.name from stores s join stores_brands sb on (sb.stores_id = s.id) join brands b on (b.id = sb.brands_id) WHERE b.id = @BrandId", conn);

      SqlParameter brandIdParameter = new SqlParameter();
      brandIdParameter.ParameterName = "@BrandId";
      brandIdParameter.Value = this.GetId();
      cmd.Parameters.Add(brandIdParameter);
      rdr = cmd.ExecuteReader();
      while (rdr.Read())
      {
        int brandId = rdr.GetInt32(0);
        string studentName = rdr.GetString(1);
        Store newStore = new Store(studentName, brandId);
        stores.Add(newStore);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return stores;
    }
    public void AddStore(Store newStore)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("INSERT INTO stores_brands (stores_id, brands_id) VALUES (@StoreId, @BrandId)", conn);
      SqlParameter brandIdParameter = new SqlParameter();
      brandIdParameter.ParameterName = "@BrandId";
      brandIdParameter.Value = this.GetId();
      cmd.Parameters.Add(brandIdParameter);

      SqlParameter storeIdParameter = new SqlParameter();
      storeIdParameter.ParameterName = "@StoreId";
      storeIdParameter.Value = newStore.GetId();
      cmd.Parameters.Add(storeIdParameter);

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

      SqlCommand cmd = new SqlCommand("DELETE FROM brands WHERE id = @BrandId; DELETE FROM stores_brands WHERE brands_id = @BrandId;", conn);

      SqlParameter brandIdParameter = new SqlParameter();
      brandIdParameter.ParameterName = "@BrandId";
      brandIdParameter.Value = this.GetId();
      cmd.Parameters.Add(brandIdParameter);

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
      SqlCommand cmd = new SqlCommand("DELETE FROM brands;", conn);
      cmd.ExecuteNonQuery();
    }
  }
}
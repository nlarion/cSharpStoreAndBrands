using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace StoresAndBrands
{
  public class Store
  {
    private int _id;
    private string _name;
    private string _storeNumber;

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
    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM stores WHERE id = @StoreId; DELETE FROM stores_brands WHERE store_id = @StoreId;", conn);

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
  }
}
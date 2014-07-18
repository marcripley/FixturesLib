using System;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using AjaxControlToolkit;
using System.Collections.Generic;


/// <summary>
/// Summary description for ServiceCS
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class ServiceCS : System.Web.Services.WebService {

    public ServiceCS () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }


    [WebMethod]
    public CascadingDropDownNameValue[] GetCategories(string knownCategoryValues)
    {
        string query = "SELECT 'ALL' AS CategoryName, 1 AS CategoryID UNION SELECT CategoryName, CategoryID FROM flCategories WHERE ParentID IS NULL";
        List<CascadingDropDownNameValue> categories = GetData(query);
        return categories.ToArray();
    }

    [WebMethod]
    public CascadingDropDownNameValue[] GetSubCategories(string knownCategoryValues)
    {
        string query;

        string categories = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)["CategoryID"];
        if (categories == "1")
        {
            query = string.Format("SELECT 'Select SubCategory' AS CategoryName, 0 AS CategoryID UNION SELECT 'ALL' AS CategoryName, 1 AS CategoryID UNION SELECT CategoryName, CategoryID FROM flCategories WHERE ParentID IS NOT NULL", categories);
        }
        else if (categories == "0")
        {
            query = string.Format("SELECT CategoryName, CategoryID FROM flCategories WHERE ParentID = {0}", categories);
        }
        else
        {
            query = string.Format("SELECT 'Select SubCategory' AS CategoryName, 0 AS CategoryID UNION SELECT 'ALL' AS CategoryName, 1 AS CategoryID UNION SELECT CategoryName, CategoryID FROM flCategories WHERE ParentID = {0}", categories);
        }
        List<CascadingDropDownNameValue> subcategories = GetData(query);
        return subcategories.ToArray();
    }


    private List<CascadingDropDownNameValue> GetData(string query)
    {
        string conString = ConfigurationManager.ConnectionStrings["MBData2005"].ConnectionString;
        SqlCommand cmd = new SqlCommand(query);
        List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
        using (SqlConnection con = new SqlConnection(conString))
        {
            con.Open();
            cmd.Connection = con;
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    values.Add(new CascadingDropDownNameValue
                    {
                        name = reader[0].ToString(),
                        value = reader[1].ToString()
                    });
                }
                reader.Close();
                con.Close();
                return values;
            }
        }
    }




   

}

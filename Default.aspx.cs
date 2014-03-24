using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class _Default : System.Web.UI.Page
{
        string MBData2005_DEV =
            ConfigurationManager.ConnectionStrings["MBData2005_DEV"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable table = new DataTable();
        
            var autoID = 1;
            // get the connection

            using (SqlConnection conn = new SqlConnection(MBData2005_DEV))
            {
                // write the sql statement to execute
                string sql = "SELECT TOP 200 Address FROM OLContacts2";
                // instantiate the command object to fire
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    // attach the parameter to pass if no parameter is in the sql no need to attach
                    SqlParameter prm = new SqlParameter("@autoId", autoID);
                    cmd.Parameters.Add(prm);
                    // get the adapter object and attach the command object to it
                    using (SqlDataAdapter ad = new SqlDataAdapter(cmd))
                    {
                        // fire Fill method to fetch the data and fill into DataTable
                        ad.Fill(table);
       
                    }
                    // DataAdapter doesn't need open connection, it takes care of opening and closing the database connection
                }
            }

            // loop through the rows of the table
            foreach (DataRow row in table.Rows)
            {
                //Response.Write("<p>"  + row["Address"] + "</p>");
            }
        }
}

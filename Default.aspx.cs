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
    string MBIntranet_DEV =
            ConfigurationManager.ConnectionStrings["MBIntranet_DEV"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ExecuteQuery(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            DataTable table = new DataTable();

            var autoID = 1;
            // get the connection

            using (SqlConnection conn = new SqlConnection(MBIntranet_DEV))
            {
                // write the sql statement to execute
                string sql = "SELECT TOP 5 jobNumber FROM flPosts";
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
            switch (categoryList.Text)
            {
                case "":
                    break;
                case " ":
                    break;
                default:
                    break;
            }
            if (categoryList.Text == "")
            {

            }
            // loop through the rows of the table
            foreach (DataRow row in table.Rows)
            {
                Response.Write("<p>" + row["jobNumber"] + "</p>");
            }
            
        }
        protected void subcategoryList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void jobNumberText_TextChanged(object sender, EventArgs e)
        {

        }
}

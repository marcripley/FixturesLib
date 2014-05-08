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
        if (!IsPostBack)
        {
            DataTable table = new DataTable();

            var autoID = 1;
            // get the connection

            categoryList0.Items.Clear();

            using (SqlConnection conn = new SqlConnection(MBIntranet_DEV))
            {
                // write the sql statement to execute
                string sql = "SELECT ID, parent, name FROM flCategories";
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
            // loop through the rows of the table and add the categories
            foreach (DataRow row in table.Rows)
            {
                if (string.IsNullOrEmpty(row["parent"].ToString()))
                {
                    categoryList0.Items.Add(row["name"].ToString());
                }
            }

            categoryList0.Items.Insert(0, "Select");
            categoryList0.Items.Insert(1, "All");
        }

    }

    protected void subcategoryList_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable table = new DataTable();
        var autoID = 1;
        // get the connection

        // fill table with categories to find out if the category is a child of the category selected to
        // display it as a subcategory in the subcategory down drop list
        using (SqlConnection conn = new SqlConnection(MBIntranet_DEV))
        {
            // write the sql statement to execute
            string sql = "SELECT jobNumber, tasks, name, category, subCategory, img1, img2, allImages, tags, comments, createdDate FROM flPosts";
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

        DataTable tablec = new DataTable();
        string selectedInd = "";
        //clear subcategory list before adding to it

        var autoIDc = 1;
        // get the connection

        // fill table with categories to find out if the category is a child of the category selected to
        // display it as a subcategory in the subcategory down drop list
        using (SqlConnection conn = new SqlConnection(MBIntranet_DEV))
        {
            // write the sql statement to execute
            string sql = "SELECT ID, parent, name FROM flCategories";
            // instantiate the command object to fire
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                // attach the parameter to pass if no parameter is in the sql no need to attach
                SqlParameter prm = new SqlParameter("@autoId", autoIDc);
                cmd.Parameters.Add(prm);
                // get the adapter object and attach the command object to it
                using (SqlDataAdapter ad = new SqlDataAdapter(cmd))
                {
                    // fire Fill method to fetch the data and fill into DataTable
                    ad.Fill(tablec);
                }
                // DataAdapter doesn't need open connection, it takes care of opening and closing the database connection
            }
        }

        // find the ID of the currently selected Category and send to selectedInd
        foreach (DataRow row in tablec.Rows)
        {
            if (subcategoryList0.SelectedItem.Text == row["name"].ToString())
            {
                selectedInd = row["ID"].ToString();
            }
        }
        // display the post based on the subcategory
        foreach (DataRow row in table.Rows)
        {
            if (selectedInd == row["subcategory"].ToString())
            {
                bottomOfPage.Text = "<br>" + "Name:" + row["name"] + "Job Number:" + row["jobNumber"] + "Tasks:" + row["tasks"] + "Category:" + row["category"] + "Sub-category" + row["subcategory"]
                  + "<a href='single.php'><div class='fixturesProj'><img src='' data-other-src='' alt='Fixtures Library Project' width='540' height='450' class='flFeaturedImage' title='C3 Energy | Other, Planter, Shingles, Round' /><span class='fixturesProjTitle'><p></p></span></div></a>";
                bottomOfPage.Visible = true;
            }
        }
    }

    protected void jobNumberText_TextChanged(object sender, EventArgs e)
    {

    }

    protected void categoryList_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable table = new DataTable();

        string selectedInd = "";
        //clear subcategory list before adding to it
        subcategoryList0.Items.Clear();

        var autoID = 1;
        // get the connection

        // fill table with categories to find out if the category is a child of the category selected to
        // display it as a subcategory in the subcategory down drop list
        using (SqlConnection conn = new SqlConnection(MBIntranet_DEV))
        {
            // write the sql statement to execute
            string sql = "SELECT ID, parent, name FROM flCategories";
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
        // find the ID of the currently selected Category and send to selectedInd
        foreach (DataRow row in table.Rows)
        {
            if (categoryList0.SelectedItem.Text == row["name"].ToString())
            {
                selectedInd = row["ID"].ToString();
            }
        }

        // load table for flPosts
        DataTable tablec = new DataTable();

        var autoIDc = 1;
        // get the connection

        // fill table with the data from the flPosts table
        using (SqlConnection conn = new SqlConnection(MBIntranet_DEV))
        {
            // write the sql statement to execute
            string sql = "SELECT jobNumber, tasks, name, category, subCategory, img1, img2, allImages, tags, comments, createdDate FROM flPosts";
            // instantiate the command object to fire
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                // attach the parameter to pass if no parameter is in the sql no need to attach
                SqlParameter prm = new SqlParameter("@autoId", autoIDc);
                cmd.Parameters.Add(prm);
                // get the adapter object and attach the command object to it
                using (SqlDataAdapter ad = new SqlDataAdapter(cmd))
                {
                    // fire Fill method to fetch the data and fill into DataTable
                    ad.Fill(tablec);
                }
                // DataAdapter doesn't need open connection, it takes care of opening and closing the database connection
            }
        }
        // save the posts' attributes
        string jobNumber, tasks, name, category, subcategory, img1, img2, allImages, tags, comments, createdDate, titles;

        // display all of the posts
        if (categoryList0.SelectedItem.Text == "All")
        {
            bottomOfPage.Text = "<br>";
            foreach (DataRow row in tablec.Rows)
            {
                jobNumber=row["jobNumber"].ToString(); tasks=row["tasks"].ToString(); name=row["name"].ToString(); category=row["category"].ToString(); 
                subcategory=row["subcategory"].ToString(); img1=row["img1"].ToString(); img2=row["img2"].ToString(); allImages=row["allImages"].ToString();
                tags = row["tags"].ToString(); comments = row["comments"].ToString(); createdDate = row["createdDate"].ToString(); titles = name + " | " + tags;

                bottomOfPage.Text += "Name:" + row["name"] + "Job Number:" + row["jobNumber"] + "Tasks:" + row["tasks"] + "Category:" + row["category"] + "Sub-category" + row["subcategory"]
                  + "<a href='single.php'><div class='fixturesProj'><img src=img1 data-other-src=img2 alt=name width=540 height=450 class='flFeaturedImage' title='C3 Energy | Other, Planter, Shingles, Round' style='display: inline;'> /><span class='fixturesProjTitle'><p></p></span></div></a>";

                bottomOfPage.Visible = true;
            }
        }

        category = "";

        // display only a particular category
        // find the ID for the currently selected category
        foreach (DataRow row in table.Rows)
        {
            if (categoryList0.SelectedItem.Text == row["name"].ToString())
            {
                category = row["ID"].ToString();
            }
        }

        // display only a particular category
        foreach (DataRow row in tablec.Rows)
        {
            if (category == row["category"].ToString())
            {
                jobNumber = row["jobNumber"].ToString(); tasks = row["tasks"].ToString(); name = row["name"].ToString(); category = row["category"].ToString();
                subcategory = row["subcategory"].ToString(); img1 = row["img1"].ToString(); img2 = row["img2"].ToString(); allImages = row["allImages"].ToString();
                tags = row["tags"].ToString(); comments = row["comments"].ToString(); createdDate = row["createdDate"].ToString(); titles = name + " | " + tags;

                bottomOfPage.Text = "<br>" + "Name:" + row["name"] + "Job Number:" + row["jobNumber"] + "Tasks:" + row["tasks"] + "Category:" + categoryList0.SelectedItem.Text + "Sub-category" + row["subcategory"]
                  + "<a href='single.php'><div class='fixturesProj'><img src='' data-other-src='' alt='Fixtures Library Project' width='540' height='450' class='flFeaturedImage' title='C3 Energy | Other, Planter, Shingles, Round' /><span class='fixturesProjTitle'><p></p></span></div></a>";
                bottomOfPage.Visible = true;
            }
        }
        // populate the subcategory list based on the category selected
        if (!(categoryList0.SelectedItem.Text == "All"))
        {
            foreach (DataRow row in table.Rows)
            {
                if (selectedInd == row["parent"].ToString())
                {
                    subcategoryList0.Items.Add(row["name"].ToString());
                }
            }
        }
        else
        {
             foreach (DataRow row in table.Rows)
             {
                 if (categoryList0.SelectedItem.Text == "All" && !(string.IsNullOrEmpty(row["parent"].ToString())))
                 {
                      subcategoryList0.Items.Add(row["name"].ToString());
                 }
             }
        }

        subcategoryList0.Items.Insert(0, "Select");
        subcategoryList0.Items.Insert(1, "All");

    }

    protected void autoComplete(object sender, EventArgs e)
    {
        DataTable tablet = new DataTable();

        var autoIDt = 1;
        // get the connection

        // fill table with the data from the flPosts table
        using (SqlConnection conn = new SqlConnection(MBIntranet_DEV))
        {
            // write the sql statement to execute
            string sql = "SELECT jobNumber, tasks, name, category, subCategory, img1, img2, allImages, tags, comments, createdDate FROM flPosts";
            // instantiate the command object to fire
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                // attach the parameter to pass if no parameter is in the sql no need to attach
                SqlParameter prm = new SqlParameter("@autoId", autoIDt);
                cmd.Parameters.Add(prm);
                // get the adapter object and attach the command object to it
                using (SqlDataAdapter ad = new SqlDataAdapter(cmd))
                {
                    // fire Fill method to fetch the data and fill into DataTable
                    ad.Fill(tablet);
                }
                // DataAdapter doesn't need open connection, it takes care of opening and closing the database connection
            }
        }

        IList<string> tagging = new List<string>();

        foreach (DataRow row in tablet.Rows)
        {
            tagging.Add(row["tags"].ToString());
            tagging = row["tags"].ToString().Split(',').Select(sValue => sValue.Trim()).ToArray();
        }

    }

    //code for auto completing tags
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {
        // Create list of tags  
        string[] tags = { "Star Wars", "Star Trek", "Superman", "Memento", "Shrek", "Shrek II" };

        // Return matching movies  
        return (from m in tags where m.StartsWith(prefixText, StringComparison.CurrentCultureIgnoreCase) select m).Take(count).ToArray();
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string GetDynamicContent(string contextKey)
    {
        return default(string);
    }

}

﻿using System;
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

    public static List<string> GetCompletionList(string prefixText)
    {
        DataTable dt = new DataTable();
        string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        SqlConnection con = new SqlConnection(constr);
        con.Open();
        SqlCommand cmd = new SqlCommand("select * from flPosts where tags like @Tag+'%'", con);
        cmd.Parameters.AddWithValue("@Tag", prefixText);
        SqlDataAdapter adp = new SqlDataAdapter(cmd);
        adp.Fill(dt);
        List<string> tags = new List<string>();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            tags.Add(dt.Rows[i][1].ToString());
        }
        return tags;
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
        // display the post based on the subcategory
        foreach (DataRow row in table.Rows)
        {
            if (subcategoryList0.SelectedItem.Text == row["subcategory"].ToString())
            {
                bottomOfPage.Text = "Job Number:" + row["jobNumber"] + "Tasks:" + row["tasks"] + "Name:" + row["name"] + "Category:" + row["category"] + "Sub-category" + row["subcategory"]
                  + "<a href='single.php'><div class='fixturesProj'><img src='http://missionbell.com/projects/c3-energy/images/c3-energy7-940x450.jpg' data-other-src='http://missionbell.com/projects/c3-energy/images/c3energySD.jpg' alt='Fixtures Library Project' width='940' height='450' class='flFeaturedImage' title='C3 Energy | Other, Planter, Shingles, Round' /><span class='fixturesProjTitle'><p></p></span></div></a>";
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
        bottomOfPage.Text = "Hello";
        bottomOfPage.Visible = true;
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

        foreach (DataRow row in table.Rows)
        {
            if (selectedInd == row["parent"].ToString())
            {
                subcategoryList0.Items.Add(row["name"].ToString());
            }
            else if (categoryList0.SelectedItem.Text == "All" && !(string.IsNullOrEmpty(row["parent"].ToString())))
            {
                subcategoryList0.Items.Add(row["name"].ToString());
            }
        }
        subcategoryList0.Items.Insert(0, "Select");
        subcategoryList0.Items.Insert(1, "All");

        // load table for flPosts
        DataTable tablec = new DataTable();

        var autoIDc = 1;
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

        // display all of the posts
        if (categoryList0.SelectedItem.Text == "All")
        {
            foreach (DataRow row in tablec.Rows)
            {
                bottomOfPage.Text = "Job Number:" + row["jobNumber"] + "Tasks:" + row["tasks"] + "Name:" + row["name"] + "Category:" + row["category"] + "Sub-category" + row["subcategory"]
                  + "<a href='single.php'><div class='fixturesProj'><img src='http://missionbell.com/projects/c3-energy/images/c3-energy7-940x450.jpg' data-other-src='http://missionbell.com/projects/c3-energy/images/c3energySD.jpg' alt='Fixtures Library Project' width='940' height='450' class='flFeaturedImage' title='C3 Energy | Other, Planter, Shingles, Round' /><span class='fixturesProjTitle'><p></p></span></div></a>";
                bottomOfPage.Visible = true;
            }
        }

        string category = "";

        // display only a particular category
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
                topOfPage.Text = "Job Number:" + row["jobNumber"] + "Tasks:" + row["tasks"] + "Name:" + row["name"] + "Category:" + row["category"] + "Sub-category" + row["subcategory"]
                  + "<a href='single.php'><div class='fixturesProj'><img src='http://missionbell.com/projects/c3-energy/images/c3-energy7-940x450.jpg' data-other-src='http://missionbell.com/projects/c3-energy/images/c3energySD.jpg' alt='Fixtures Library Project' width='940' height='450' class='flFeaturedImage' title='C3 Energy | Other, Planter, Shingles, Round' /><span class='fixturesProjTitle'><p></p></span></div></a>";
                topOfPage.Visible = true;
            }
        }
    }

    //code for auto completing tags
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {
        // Create array of movies  
        string[] movies = { "Star Wars", "Star Trek", "Superman", "Memento", "Shrek", "Shrek II" };

        // Return matching movies  
        return (from m in movies where m.StartsWith(prefixText, StringComparison.CurrentCultureIgnoreCase) select m).Take(count).ToArray();

    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string GetDynamicContent(string contextKey)
    {
        return default(string);
    }

}

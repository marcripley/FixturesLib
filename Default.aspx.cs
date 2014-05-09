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
    //Jenise Marcus - 5/8/14 - Updated Connection string name - need to re-update later
    string MBIntranet_DEV = ConfigurationManager.ConnectionStrings["MBData2005_DEV"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //jenise marcus 5/8/14 - Add Tags to dropdown from flTags table.
            AddItems();

            DataTable table = new DataTable();

            var autoID = 1;
            // get the connection

            categoryList0.Items.Clear();

            using (SqlConnection conn = new SqlConnection(MBIntranet_DEV))
            { 
                //Jenise Marcus - 5/8/14 - updated field names
                // write the sql statement to execute
                string sql = "SELECT CategoryID, ParentID, CategoryName FROM flCategories";
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
                //Jenise Marcus - 5/8/14 - updated field names
                if (string.IsNullOrEmpty(row["ParentID"].ToString()))
                {
                    categoryList0.Items.Add(row["CategoryName"].ToString());
                }
            }

            categoryList0.Items.Insert(0, "Select");
            categoryList0.Items.Insert(1, "All");
        }
    }



    //public static List<string> GetCompletionList(string prefixText)
    //{
    //    DataTable dt = new DataTable();
    //    string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
    //    SqlConnection con = new SqlConnection(constr);
    //    con.Open();
    //    SqlCommand cmd = new SqlCommand("select * from flPosts where tags like @Tag+'%'", con);
    //    cmd.Parameters.AddWithValue("@Tag", prefixText);
    //    SqlDataAdapter adp = new SqlDataAdapter(cmd);
    //    adp.Fill(dt);
    //    List<string> tags = new List<string>();
    //    for (int i = 0; i < dt.Rows.Count; i++)
    //    {
    //        tags.Add(dt.Rows[i][1].ToString());
   //     }
    //    return tags;
    //}



    protected void subcategoryList_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Jenise Marcus - 5/9/14 - commented out - don't think we need since we have the search button

       // DataTable table = new DataTable();

       // var autoID = 1;
        // get the connection

        // fill table with categories to find out if the category is a child of the category selected to
        // display it as a subcategory in the subcategory down drop list
        //using (SqlConnection conn = new SqlConnection(MBIntranet_DEV))
        //{
            //jenise marcus - 5/8/14 - updated sql statement field names
            // write the sql statement to execute
        //    string sql = "SELECT lJobNumber, lTaskNumber, name, category, subCategory, img1, img2, allImages, tags, comments, createdDate FROM flPosts";
            // instantiate the command object to fire
        //    using (SqlCommand cmd = new SqlCommand(sql, conn))
        //    {
                // attach the parameter to pass if no parameter is in the sql no need to attach
         //       SqlParameter prm = new SqlParameter("@autoId", autoID);
         //       cmd.Parameters.Add(prm);
                // get the adapter object and attach the command object to it
         //       using (SqlDataAdapter ad = new SqlDataAdapter(cmd))
         //       {
                    // fire Fill method to fetch the data and fill into DataTable
         //           ad.Fill(table);
         //       }
                // DataAdapter doesn't need open connection, it takes care of opening and closing the database connection
          //  }
       // }



        //jenise marcus - 5/8/14 - removed - used for testing by Tim
        // display the post based on the subcategory
      //  foreach (DataRow row in table.Rows)
      //  {
      //      if (subcategoryList0.SelectedItem.Text == row["subcategory"].ToString())
      //      {
     //           bottomOfPage.Text = "Job Number:" + row["jobNumber"] + "Tasks:" + row["tasks"] + "Name:" + row["name"] + "Category:" + row["category"] + "Sub-category" + row["subcategory"]
     //             + "<a href='single.php'><div class='fixturesProj'><img src='http://missionbell.com/projects/c3-energy/images/c3-energy7-940x450.jpg' data-other-src='http://missionbell.com/projects/c3-energy/images/c3energySD.jpg' alt='Fixtures Library Project' width='940' height='450' class='flFeaturedImage' title='C3 Energy | Other, Planter, Shingles, Round' /><span class='fixturesProjTitle'><p></p></span></div></a>";
     //           bottomOfPage.Visible = true;
     //       }
     //   }
    }


    //Jenise Marcus - 5/9/14 - may not need since we have a search button.
    protected void jobNumberText_TextChanged(object sender, EventArgs e)
    {

    }



    protected void categoryList_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable table = new DataTable();

        string selectedInd = "";

        //Jenise Marcus = 5/9/14 - commented out - used for Tim's Testing
        //bottomOfPage.Text = "Hello";
        //bottomOfPage.Visible = true;

        //clear subcategory list before adding to it
        subcategoryList0.Items.Clear();

        var autoID = 1;
        // get the connection

        // fill table with categories to find out if the category is a child of the category selected to
        // display it as a subcategory in the subcategory down drop list
        using (SqlConnection conn = new SqlConnection(MBIntranet_DEV))
        {
            //Jenise Marcus - 5/8/14 - Updated field names
            // write the sql statement to execute
            string sql = "SELECT CategoryID, ParentID, CategoryName FROM flCategories";
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



        //jenise marcus - 5/9/14 - need to invistiate - should be able to retrieve value instead
        //of looping
        //selectedInd = categoryList0.SelectedValue;

        //Jenise M. - Updated field names
        // loop through the rows of the table
        // find the ID of the currently selected Category and send to selectedInd
        foreach (DataRow row in table.Rows)
        {
            if (categoryList0.SelectedItem.Text == row["CategoryName"].ToString())
            {
                selectedInd = row["CategoryID"].ToString();
            }
        }

        //Comment by Jenise - 5/9/14 - this portion of code looks like it has been taken out but is
        //working code?? ask Tim
        foreach (DataRow row in table.Rows)
        {
            if (selectedInd == row["ParentId"].ToString())
            {
                subcategoryList0.Items.Add(row["CategoryName"].ToString());
            }
            else if (categoryList0.SelectedItem.Text == "All" && !(string.IsNullOrEmpty(row["ParentID"].ToString())))
            {
                subcategoryList0.Items.Add(row["CategoryName"].ToString());
            }
        }
        subcategoryList0.Items.Insert(0, "Select");
        subcategoryList0.Items.Insert(1, "All");


        // load table for flPosts
        DataTable tablec = new DataTable();

        var autoIDc = 1;
        // get the connection

        // fill table with the data from the flPosts table
        using (SqlConnection conn = new SqlConnection(MBIntranet_DEV))
        {
            //jenise marcus - 5/8/14 - updated statement to *, update later
            // write the sql statement to execute
            string sql = "SELECT * FROM flPosts";
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

        string jobNumber, tasks, name, category, subcategory, img1, img2, allImages, tags, comments, createdDate, titles;

        //jenise marcus - 5/8/14 - removed for now - Tim's Testing
        // display all of the posts
        //if (categoryList0.SelectedItem.Text == "All")
        //{
        //    foreach (DataRow row in tablec.Rows)
        //    {
        //        jobNumber=row["jobNumber"].ToString(); tasks=row["tasks"].ToString(); name=row["name"].ToString(); category=row["category"].ToString(); 
        //        subcategory=row["subcategory"].ToString(); img1=row["img1"].ToString(); img2=row["img2"].ToString(); allImages=row["allImages"].ToString();
       //         tags = row["tags"].ToString(); comments = row["comments"].ToString(); createdDate = row["createdDate"].ToString(); titles = name + " | " + tags;

        //        bottomOfPage.Text += "Job Number:" + row["jobNumber"] + "Tasks:" + row["tasks"] + "Name:" + row["name"] + "Category:" + row["category"] + "Sub-category" + row["subcategory"]
       //           + "<a href='single.php'><div class='fixturesProj'><img src='http://missionbell.com/projects/c3-energy/images/c3-energy7-940x450.jpg' data-other-src='http://missionbell.com/FixturesLib/images/c3energySD.jpg' alt='Fixtures Library Project' width=940 height=450 class='flFeaturedImage' title='C3 Energy | Other, Planter, Shingles, Round' style='display: inline;'> /><span class='fixturesProjTitle'><p></p></span></div></a>";
       //         bottomOfPage.Visible = true;
       //     }
       // }

        category = "";

        // display only a particular category
        foreach (DataRow row in table.Rows)
        {
            if (categoryList0.SelectedItem.Text == row["CategoryName"].ToString())
            {
                category = row["CategoryID"].ToString();
            }
        }

        //jenise marcus - 5/8/14 - removed for now - Looks like Testing code for Tim??
        // display only a particular category
       // foreach (DataRow row in tablec.Rows)
       // {
       //     if (category == row["category"].ToString())
       //     {
       //         topOfPage.Text = "Job Number:" + row["jobNumber"] + "Tasks:" + row["tasks"] + "Name:" + row["name"] + "Category:" + row["category"] + "Sub-category" + row["subcategory"]
      //            + "<a href='single.php'><div class='fixturesProj'><img src='http://missionbell.com/projects/c3-energy/images/c3-energy7-940x450.jpg' data-other-src='http://missionbell.com/projects/c3-energy/images/c3energySD.jpg' alt='Fixtures Library Project' width='940' height='450' class='flFeaturedImage' title='C3 Energy | Other, Planter, Shingles, Round' /><span class='fixturesProjTitle'><p></p></span></div></a>";
      //          topOfPage.Visible = true;
       //     }
       // }



        //Comment by Jenise Marcus - 5/9/14 - Code above does similar thing - ask Tim which is the most current??
        // populate the subcategory list based on the category selected
        //if (!(categoryList0.SelectedItem.Text == "All"))
        //{
        //    foreach (DataRow row in table.Rows)
        //    {
        //        if (selectedInd == row["parent"].ToString())
        //        {
        //            subcategoryList0.Items.Add(row["name"].ToString());
        //        }
        //    }
        //}
        //else
        //{
        //     foreach (DataRow row in table.Rows)
        //     {
        //         if (categoryList0.SelectedItem.Text == "All" && !(string.IsNullOrEmpty(row["parent"].ToString())))
        //         {
        //              subcategoryList0.Items.Add(row["name"].ToString());
        //         }
        //     }
        //}

       // subcategoryList0.Items.Insert(0, "Select");
        //subcategoryList0.Items.Insert(1, "All");
    }


    //comment by Jenise Marcus - 5/9/14 ???
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string GetDynamicContent(string contextKey)
    {
        return default(string);
    }




    //Remaining code below added by Jenise Marcus 5/8/14 for Checkboxlist drop down

    /// Set the Width of the Combo
    public int Width
    {
        set { tags0.Width = value; }
        get { return (Int32)tags0.Width.Value; }
    }
    public bool Enabled
    {
        set { tags0.Enabled = value; }
    }
    /// Set the CheckBoxList font Size
    public FontUnit fontSizeCheckBoxList
    {
        set { chkList.Font.Size = value; }
        get { return chkList.Font.Size; }
    }
    /// Set the ComboBox font Size
    public FontUnit fontSizeTextBox
    {
        set { tags0.Font.Size = value; }
    }

    /// Add Items to the CheckBoxList from sql server tables
    public void AddItems()
    {
        //clear checklist
        chkList.Items.Clear();

        DataTable table = new DataTable();

        using (SqlConnection conn = new SqlConnection(MBIntranet_DEV))
        {
            // write the sql statement to execute
            string sql = "SELECT TagID, Tags FROM flTags";
            // instantiate the command object to fire
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                //conn.Open();
                using (SqlDataAdapter ad = new SqlDataAdapter(cmd))
                {
                    // fire Fill method to fetch the data and fill into DataTable
                    ad.Fill(table);
                }
                //SqlDataReader reader = cmd.ExecuteReader();
                ListItem li = new ListItem();
                foreach (DataRow row in table.Rows)
                //while (reader.Read())
                {
                    li = new ListItem(row["Tags"].ToString(), Convert.ToInt32(row["TagID"]).ToString());
                    //li = new ListItem(reader["Tags"].ToString(), Convert.ToInt32(reader["TagID"]).ToString());
                    //if (row["Tags"].ToString() != "N/A")
                    //if (reader["Tags"].ToString() != "NA")
                    //{
                        chkList.Items.Add(li);
                    //}
                    //else
                    //{
                    //    chkList.SelectedValue = Convert.ToInt32(reader["TagID"]).ToString();
                    //}
                }
                //reader.Close();
                //conn.Close();
                //conn.Dispose();
            }
        }
    }


    /// Get or Set the Text shown in the Combo
    public string Text
    {
        get { return hidVal.Value; }
        set { tags0.Text = value; }
    }



    protected void btnSubmit_OnClick(object sender, EventArgs e)
    {
        //string strquery2 = "SELECT dbo.flPosts.lTaskNumber, dbo.flPosts.lJobNumber, dbo.flPosts.Img1, dbo.flPosts.Img2 FROM  dbo.flPosts INNER JOIN dbo.flTagLookup ON dbo.flPosts.PostID = dbo.flTagLookup.PostID WHERE (dbo.flPosts.SubCategoryID = 5)";
        string strquery = string.Empty;
        //string test;

        for (int i = 0; i < chkList.Items.Count; i++)
        {
           // test = chkList.Items[i].Text;
            if (chkList.Items[i].Selected)
            {
                if (string.IsNullOrEmpty(strquery))
                {
                    strquery = " AND dbo.flTagLookup.TagID = " + chkList.Items[i].Value;
                }
                else
                {
                    strquery += " OR dbo.flTagLookup.TagID = " + chkList.Items[i].Value;
                }
            }
        }

        //submit selected values to stored procedure and retrieve results
        //temp populated into gridveiw
        using (SqlConnection conn = new SqlConnection(MBIntranet_DEV))
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "flPostSearch";
                cmd.Parameters.Add("@SubCategoryID", SqlDbType.Int).Value = subcategoryList0.SelectedValue;
                cmd.Parameters.Add("@TagQuery", SqlDbType.VarChar).Value = strquery;
                cmd.Parameters.Add("@JobNumber", SqlDbType.Int).Value = jobNumber0.Text.Trim();
                cmd.Connection = conn;

                try
                {
                    SqlDataAdapter myadapter = new SqlDataAdapter();
                    myadapter.SelectCommand = cmd;
                    DataSet myDataSet = new DataSet();
                    myadapter.Fill(myDataSet);
      
                    DataView myDataView = new DataView();
                    myDataView = myDataSet.Tables[0].DefaultView;   

                    gv.DataSource = myDataView;
                    gv.DataBind();
                    gv.Visible = true;
                }
                catch (Exception ex)
                {
                    topOfPage.Visible = true;
                    topOfPage.Text = "Error Msg: " + ex.Message;
                }
            }
        }

       topOfPage.Visible = true;
       topOfPage.Text = strquery;  
    }




}

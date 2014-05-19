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

           subcategoryList0.Items.Clear();
        }
        else
        {
            //lbltest.Text = "postback";
        }
    }



    protected void subcategoryList_SelectedIndexChanged(object sender, EventArgs e)
    {
       
    }



    //Jenise Marcus - 5/9/14 - may not need since we have a search button.
    protected void jobNumberText_TextChanged(object sender, EventArgs e)
    {

    }



    protected void categoryList_SelectedIndexChanged(object sender, EventArgs e)
    {
        //clear subcategory list before adding to it
        subcategoryList0.Items.Clear();

        dsSubCategories.SelectCommand = "SELECT CategoryID, CategoryName FROM flCategories WHERE ParentID = " + categoryList0.SelectedValue;
        subcategoryList0.Items.Insert(0, "Select");
        subcategoryList0.Items.Insert(1, "All");
        subcategoryList0.DataBind();
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
                using (SqlDataAdapter ad = new SqlDataAdapter(cmd))
                {
                    // fire Fill method to fetch the data and fill into DataTable
                    ad.Fill(table);
                }
                //SqlDataReader reader = cmd.ExecuteReader();
                ListItem li = new ListItem();
                foreach (DataRow row in table.Rows)
                {
                    li = new ListItem(row["Tags"].ToString(), Convert.ToInt32(row["TagID"]).ToString());
                    chkList.Items.Add(li);
                }
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

        for (int i = 0; i < chkList.Items.Count; i++)
        {
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
                if (string.IsNullOrEmpty(strquery))
                {
                    cmd.Parameters.Add("@TagQuery", SqlDbType.VarChar).Value = "N/A";
                }
                else
                {
                    cmd.Parameters.Add("@TagQuery", SqlDbType.VarChar).Value = strquery;
                }
                if (string.IsNullOrEmpty(jobNumber0.Text))
                {
                    cmd.Parameters.Add("@JobNumber", SqlDbType.VarChar).Value = "N/A";
                }
                else
                {
                    cmd.Parameters.Add("@JobNumber", SqlDbType.VarChar).Value = jobNumber0.Text.Trim();
                }
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


                    //Display No records message if no data found.
                    if (gv.Rows.Count == 0)
                    {
                        gv.Visible = false;
                        topOfPage.Visible = true;
                        topOfPage.Text = "No Requests have been Submitted.";
                    }
                    else
                    {
                        gv.Visible = true;
                        topOfPage.Text = "record count:" + gv.Rows.Count;
                    }
                    myadapter.Dispose();
                }
                catch (Exception ex)
                {
                    topOfPage.Visible = true;
                    topOfPage.Text = "Error Msg: " + ex.Message;
                }
                finally
                {
                    cmd.Dispose();
                    conn.Close();
                    conn.Dispose();
                }
            }
        }
    }




}

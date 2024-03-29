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
using System.DirectoryServices.AccountManagement;
using System.Web.Services;


public partial class _Default : System.Web.UI.Page
{
    //Public Declarations
    //public string strUsername;
    string MBIntranet_DEV = ConfigurationManager.ConnectionStrings["MBData2005"].ConnectionString;

    public string strPrimImageLoc;
    //public string strSecImageLoc;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //Initial Page Open

            //Add List of Tags to checklist drop down
            AddItems();

            //Clear Subcategory list
            subcategoryList0.Items.Clear();

            //Populate user message
            lblMessage.Visible = true;
            lblMessage.Text = "To begin your search, please select a Category, Tag or enter a Job Number";
            

            //Check for Pre-populated tags
            //Tags can be pre-populated if the user has selected the tags from the Project details screen
            //Tags will then automatically filter the selection and pre-populate the "Tags" text box if previously selected
            if (!String.IsNullOrEmpty(Request.QueryString["TagID"]))
            {
                //Populate the Tags Text box from the querystring. Querystring will be populated with the tag if user 
                //selected it from previous project details screen
                tags0.Text = Request.QueryString["Tag"];

                //Obtain TagId from querystring and check Tag checkbox based upon TagID
                ListItem currentCheckBox = chkList.Items.FindByValue(Request.QueryString["TagID"]);
                if (currentCheckBox != null)
                {
                    currentCheckBox.Selected = true;
                }

                //Call GetPics method to retrieve Images from Database based upon TagID
                GetPosts();
            }
        }
    }



    protected void categoryList_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Call GetTags Method - Keeps Populate Tag textbox populated with selected items after postback
        GetTags();

        //clear subcategory list before adding to it
        subcategoryList0.Items.Clear();

        //If user selects "All" from subcategory drop down, provide all otherwise filter by selected category
        if (categoryList0.SelectedValue == "0")
        {
            subcategoryList0.Items.Clear();
        }
        else
        {
            //Add Select and All items to the Subcategory drop down list
            subcategoryList0.Items.Insert(0, new ListItem("Select", "0"));
            subcategoryList0.Items.Insert(1, new ListItem("All", "1"));

            if (categoryList0.SelectedValue == "1")
            {
                dsSubCategories.SelectCommand = "SELECT CategoryID, CategoryName FROM flCategories WHERE ParentID IS NOT NULL";
            }
            else
            {
                dsSubCategories.SelectCommand = "SELECT CategoryID, CategoryName FROM flCategories WHERE ParentID = " + categoryList0.SelectedValue;
            }
        }
        subcategoryList0.DataBind();
    }



    protected void subcategoryList_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Call GetPics method to retrieve Images from Database based upon SubCategoryID Selected
        GetPosts();
    }



    protected void GetPosts()
    {
        //Clear string used for populating query criteria
        string strquery = string.Empty;

        //Loop through list of items checked within Tags checkbox list
        for (int i = 0; i < chkList.Items.Count; i++)
        {
            if (chkList.Items[i].Selected)
            {
                if (string.IsNullOrEmpty(strquery))
                {
                    strquery = " AND (dbo.flTagLookup.TagID = " + chkList.Items[i].Value;
                }
                else
                {
                    strquery += " OR dbo.flTagLookup.TagID = " + chkList.Items[i].Value;
                }
            }
        }


        //submit selected Tags to stored procedure and retrieve results in gridview
        using (SqlConnection conn = new SqlConnection(MBIntranet_DEV))
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "flPostSearch";

                //need to check for empty values within all search criteria boxes before passing values to database
                if (string.IsNullOrEmpty(categoryList0.SelectedValue))
                {
                    cmd.Parameters.Add("@intCategoryID", SqlDbType.Int).Value = "0";
                }
                else
                {
                    cmd.Parameters.Add("@intCategoryID", SqlDbType.Int).Value = categoryList0.SelectedValue;
                }
                if (string.IsNullOrEmpty(subcategoryList0.SelectedValue))
                {
                    cmd.Parameters.Add("@intSubCategoryID", SqlDbType.Int).Value = "0";
                }
                else
                {
                    cmd.Parameters.Add("@intSubCategoryID", SqlDbType.Int).Value = subcategoryList0.SelectedValue;
                }
                if (string.IsNullOrEmpty(strquery))
                {
                    cmd.Parameters.Add("@strTagQuery", SqlDbType.VarChar).Value = "N/A";
                }
                else
                {
                    cmd.Parameters.Add("@strTagQuery", SqlDbType.VarChar).Value = strquery;
                }
                if (string.IsNullOrEmpty(jobNumber0.Text))
                {
                    cmd.Parameters.Add("@strJobNumber", SqlDbType.VarChar).Value = "N/A";
                }
                else
                {
                    //Adding "J" prefix to JobNumber if not supplied to ensure consistancy with MB Database textJobNumber values
                    string strJobNoPrefix;
                    string strJobNumber;

                    strJobNoPrefix = jobNumber0.Text.Substring(0, 1);
                    if (strJobNoPrefix != "J")
                    {
                        strJobNumber = "J" + jobNumber0.Text;
                    }
                    else
                    {
                        strJobNumber = jobNumber0.Text;
                    }
                    cmd.Parameters.Add("@strJobNumber", SqlDbType.VarChar).Value = strJobNumber;
                }
                cmd.Connection = conn;

                try
                {
                    //Open connection and bind datasource to gridview
                    conn.Open();
                    gvPosts.DataSource = cmd.ExecuteReader();
                    gvPosts.DataBind();

                    //Display No records message if no data found.
                    if (gvPosts.Rows.Count == 0)
                    {  
                        gvPosts.Visible = false;
                        lblMessage.Text = "There are no Records that match your criteria.";
                        trlblMessage.Visible = true;
                    }
                    else
                    {
                        gvPosts.Visible = true;
                        lblMessage.Text = string.Empty;
                        trlblMessage.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    //Error handeling
                    lblMessage.Text = "Error Msg: " + ex.Message + " was received while retrieving FL Images. Please contact <u><a href=mailto:support@missionbell.com?subject=FL:RetreiveImages;Default.aspx>Support</a></u> with a screenshot of the page. Thank You.";
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



    protected void gvPosts_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        //Append Caption for each Image
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblimg1 = (Label)e.Row.FindControl("lblimage1");
            strPrimImageLoc = lblimg1.Text;
            Image img = (Image)e.Row.FindControl("imgOriginal");
            img.ImageUrl = strPrimImageLoc;
            img.Attributes.Add("onmouseout", "this.src='" + strPrimImageLoc + "'");
            Label lblOverlay = (Label)e.Row.FindControl("lblOverlayDesc");
        }
    }



    protected void GetTags()
    {
        //Keeps Populate Tag textbox populated with selected items after postback

        //Clear string used for populating query criteria
        string strTags = string.Empty;

        //Loop through the list Tag checkboxes to find items checked.
        //append each checked tag to string and assign to Checkboxlist
        for (int i = 0; i < chkList.Items.Count; i++)
        {
            if (chkList.Items[i].Selected)
            {
                if (string.IsNullOrEmpty(strTags))
                {
                    strTags = chkList.Items[i].Text;
                }
                else
                {
                    strTags += ", " + chkList.Items[i].Text; ;
                }
            }
        }
        //assign Textbox
        tags0.Text = strTags;
    }



    protected void btnSubmit_OnClick(object sender, EventArgs e)
    {
        //Keeps Populate Tag textbox populated with selected items after postback
        GetTags();

        //Get Images based upon Criteria search provided in text/drop down boxes
        GetPosts();
    }




    protected void btnClear_OnClick(object sender, EventArgs e)
    {
        //Clear search criteria boxes
        categoryList0.SelectedValue = "0";
        subcategoryList0.Items.Clear();
        tags0.Text = string.Empty;
        chkList.ClearSelection();
        jobNumber0.Text = string.Empty;
    }




    //Add Items to the CheckBoxList from sql server tables
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
                
                ListItem li = new ListItem();
                //loop through list in select statement and attach to checklist
                foreach (DataRow row in table.Rows)
                {
                    li = new ListItem(row["Tags"].ToString(), Convert.ToInt32(row["TagID"]).ToString());
                    chkList.Items.Add(li);
                }
            }
        }
    }


    //Remaining code below Checkboxlist drop down
    // Get or Set the Text shown in the Combo
    public string Text
    {
        get { return hidVal.Value; }
        set { tags0.Text = value; }
    }
    // Set the Width of the Combo
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



}

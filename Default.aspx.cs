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
using System.DirectoryServices.AccountManagement;
using System.Web.Services;
using AjaxControlToolkit;

public partial class _Default : System.Web.UI.Page
{
    //Public Declarations
    public string strUsername;
    string MBIntranet_DEV = ConfigurationManager.ConnectionStrings["MBData2005"].ConnectionString;

    public string strPrimImageLoc;
    public string strSecImageLoc;

    public Int32 ProjSum = 0;
    public Int32 ActualSum = 0;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {           
            //Initial Page Open
            ViewState["CurrPostID"] = 0;

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

        ScriptManager1.RegisterAsyncPostBackControl(gvPosts);

       // this.RegisterPostBackControl();
    }



    //private void RegisterPostBackControl()
    //{
    //    foreach (GridViewRow row in gvPosts.Rows)
    //    {
            
    //        LinkButton lnkFull = row.FindControl("lbImageDetails") as LinkButton;
    //        string templb = Convert.ToString(lnkFull);
    //        if (!string.IsNullOrEmpty(templb))
    //        {
    //            ScriptManager.GetCurrent(this).RegisterPostBackControl(lnkFull);
    //        }
    //    }
   // }



    protected void lbImageDetails_OnClick(object sender, EventArgs e)
    {
        //LinkButton btn = sender as LinkButton;
        //GridViewRow gvr = btn.NamingContainer as GridViewRow;
        //string key = gvPosts.DataKeys[gvr.RowIndex].Value.ToString();

        //ViewState["CurrPostID"] = gvPosts.SelectedValue;

        //Int32 ipostid = Convert.ToInt32(ViewState["CurrPostID"]);

        tblOrganizationInfo.Visible = true;
        lblLaborDetailsHeader.Visible = true;
        tblProjDetails.Visible = true;
        lblCommentslbl.Visible = true;
        upLaborDetails.Visible = true;

        BindJobDetails();
        BindLaborDetails();
        GetCurrentTags();

       //updatepanel2.Update();
    }



    protected void gvPosts_Onchangeing(object sender, GridViewSelectEventArgs e)
    {
       // gvPosts.SelectedIndex = Convert.ToInt32(e.NewSelectedIndex);
       //Int32 intkey = Convert.ToInt32(gvPosts.SelectedDataKey.Value);
       //ViewState["CurrPostID"] = Convert.ToInt32(gvPosts.SelectedValue).ToString();
       //int itest = Convert.ToInt32(ViewState["CurrPostID"]);

      // updatepanel2.Update();

      // tblOrganizationInfo.Visible = true;
      // lblLaborDetailsHeader.Visible = true;
      // tblProjDetails.Visible = true;
      // lblCommentslbl.Visible = true;
      // upLaborDetails.Visible = true;

      // BindJobDetails();
     //  BindLaborDetails();
      // GetCurrentTags();
       
    }



    protected void subcategoryList_SelectedIndexChanged(object sender, EventArgs e)
    {
       // ViewState["CurrPostID"] = 0;
        tblOrganizationInfo.Visible = false;
        lblLaborDetailsHeader.Visible = false;
        tblProjDetails.Visible = false;
        lblCommentslbl.Visible = false;
        upLaborDetails.Visible = false;


        //Call GetPics method to retrieve Images from Database based upon SubCategoryID Selected
        GetPosts();
    }

  


    protected void GetTags()
    {
        //Clear string used for populating query criteria
        string strTags = string.Empty;

        //Loop through list of items checked within Tags checkbox list
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
        tags0.Text = strTags;
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

                    if (gvPosts.Rows.Count == 0)
                    {
                        //Display No records message if no data found.
                        gvPosts.Visible = false;
                        lblMessage.Text = "There are no Records that match your criteria.";
                        trlblMessage.Visible = true;
                    }
                    else
                    {
                        gvPosts.Visible = true;
                        lblMessage.Text = string.Empty;
                        //trBlank.Visible = true;
                        trlblMessage.Visible = true;
                    }                    
                }
                //Error handeling
                catch (Exception ex)
                {
                    lblMessage.Text = "Error Msg: " + ex.Message + " was received while trying to retrieve the Fixtures Library posts. Please contact support@missionbell.com with a screenshot of the page";
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



    protected void gvPosts_OnSelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        ViewState["CurrPostID"] = gvPosts.SelectedValue;
        Int32 ipostid = Convert.ToInt32(ViewState["CurrPostID"]);
    }




    protected void gvPosts_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        //Formatting for Images
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            using (SqlConnection conn = new SqlConnection(MBIntranet_DEV))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    ListView lvPics = ((ListView)e.Row.FindControl("lvPics"));
                    int ipostId = int.Parse((gvPosts.DataKeys[e.Row.RowIndex].Value.ToString()));
                    
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "flGetImages";
                    cmd.Parameters.Add("@PostId", SqlDbType.Int).Value = ipostId;
                    cmd.Connection = conn;

                    try
                    {
                        SqlDataAdapter daPics = new SqlDataAdapter();
                        daPics.SelectCommand = cmd;
                        DataSet dsPics = new DataSet();
                        daPics.Fill(dsPics);

                        lvPics.DataSource = dsPics;
                        lvPics.DataBind();

                        daPics.Dispose();
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "Error Msg: " + ex.Message + " was received while trying to retrieve the Fixtures Library images. Please contact support@missionbell.com with a screenshot of the page";
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




    protected void btnSubmit_OnClick(object sender, EventArgs e)
    {
        //Populate Tag textbox
        GetTags();

        //Get Images based upon Criteria search provided in text/drop down boxes
        GetPosts();
    }




    private void BindJobDetails()
    {
        //submit selected values to stored procedure and retrieve results
        //temp populated into gridveiw
        using (SqlConnection conn = new SqlConnection(MBIntranet_DEV))
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                //Int32 itestid = 2;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "flGetJobDetails";
                cmd.Parameters.Add("@PostID", SqlDbType.Int).Value = 1;
                cmd.Connection = conn;

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    //get code for no records found
                    if (reader.HasRows)
                    {
                        reader.Read();
                        lblProjectName.Text = reader["txtJobName"].ToString();
                        lblJobNum.Text = reader["txtJobNumber"].ToString();
                        lblGC.Text = reader["txtCustomerName"].ToString();
                        lblArhitect.Text = reader["txtArchitectName"].ToString();
                        lblJobCity.Text = reader["txtJobCity"].ToString();
                        lblCategory.Text = reader["CategoryName"].ToString();
                        lblSubCat.Text = reader["SubCategoryName"].ToString();
                        lblComments.Text = reader["Comments"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error Msg: " + ex.Message + " was received while trying to retrieve the Fixtures Library images. Please contact support@missionbell.com with a screenshot of the page";
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



    protected void GetCurrentTags()
    {
        //submit selected values to stored procedure and retrieve results
        //temp populated into gridveiw
        using (SqlConnection conn = new SqlConnection(MBIntranet_DEV))
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "flGetTags";
                cmd.Parameters.Add("@PostID", SqlDbType.Int).Value = 1;
                cmd.Connection = conn;

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    //get code for no records found
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            HyperLink hp = new HyperLink();
                            hp.Text = reader["Tags"].ToString();
                            hp.NavigateUrl = "~/Default.aspx?TagID=" + reader["TagID"] + "&Tag=" + hp.Text;
                            hp.CssClass = "post-tag";
                            tcTags.Controls.Add(hp);
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error Msg: " + ex.Message + " was received while trying to retrieve the Fixtures Library images. Please contact support@missionbell.com with a screenshot of the page";
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



    private void BindLaborDetails()
    {
        //submit selected values to stored procedure and retrieve results
        //temp populated into gridveiw
        using (SqlConnection conn = new SqlConnection(MBIntranet_DEV))
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "flGetLaborDetails";
                cmd.Parameters.Add("@PostID", SqlDbType.Int).Value = 1;
                cmd.Connection = conn;

                try
                {
                    SqlDataAdapter myadapter = new SqlDataAdapter();
                    myadapter.SelectCommand = cmd;
                    DataSet myDataSet = new DataSet();
                    myadapter.Fill(myDataSet);

                    DataView myDataView = new DataView();
                    myDataView = myDataSet.Tables[0].DefaultView;

                    gvLaborDetails.DataSource = myDataView;
                    gvLaborDetails.DataBind();

                    //Display No records message if no data found.
                    if (gvLaborDetails.Rows.Count == 0)
                    {
                        gvLaborDetails.Visible = false;
                    }
                    else
                    {
                        gvLaborDetails.Visible = true;
                    }
                    myadapter.Dispose();
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error Msg: " + ex.Message + " was received while trying to retrieve the Fixtures Library images. Please contact support@missionbell.com with a screenshot of the page";
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



    protected void gvLaborDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ProjSum = ProjSum + Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "sWorkOrderHoursBudget"));
            ActualSum = ActualSum + Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "sWorkOrderHoursActual"));
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label obj = (Label)e.Row.FindControl("ProjSum");
            obj.Text = Convert.ToString(ProjSum);

            Label obj2 = (Label)e.Row.FindControl("ActualSum");
            obj2.Text = Convert.ToString(ActualSum);
        }
    }



    protected void btnClear_OnClick(object sender, EventArgs e)
    {
        //****RE-test clearing drop downs

        //Clear search criteria boxes
        cddCategories.SelectedValue = "0";
        //cddSubCategories.SelectedValue = "0";
        subcategoryList0.Items.Clear();
        tags0.Text = string.Empty;
        chkList.ClearSelection();
        jobNumber0.Text = string.Empty;
    }



    //Remaining code below Checkboxlist drop down
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


}

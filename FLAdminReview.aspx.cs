using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.DirectoryServices.AccountManagement;
using AjaxControlToolkit;


public partial class FLAdminReview : System.Web.UI.Page
{
    //Public Declarations   
    public string strErrMsg;

    string MBIntranet_DEV = ConfigurationManager.ConnectionStrings["MBData2005"].ConnectionString;

    public Int32 ProjSum = 0;
    public Int32 ActualSum = 0;
    public Int32 intStatus;

    //Declare Class - VerifyAccess.cs file in App_Code folder
     VerifyAccess vaClass = new VerifyAccess();
     public string strAccessType;

    //public Int32 intPostID;


     protected void Page_Load(object sender, EventArgs e)
     {

         //Response.Redirect("ReDirect.aspx");

         if (!Page.IsPostBack)
         {
             //strAccessType = vaClass.VerifyflAdminAccess();
             strAccessType = Session["flgroup"].ToString();
             //strAccessType = "Both";
             //For users with no access - display the following message
             if (strAccessType == "None")
             {
                 lblMessage.Visible = true;
                 lblMessage.Text = "You do not have access to this page.  If you feel you've reached this message in error, please contact <u><a href=mailto:jenisem@missionbell.com?subject=FLAdmin>Support</a></u>.";
                 lblMainTitle.Text = "Fixtures Library Administration Page";
                 tblMain.Visible = false;
             }
             else
             {
                 //For users with access to the Administration page
                 GetPostList(strAccessType);

                 if (!!String.IsNullOrEmpty(Request.QueryString["PostID"]))
                 {
                     tcData.Visible = false;
                 }
                 else
                 {
                     tcList.Visible = true;
                     tcData.Visible = true;
                     BindLaborDetails();
                     BindJobDetails();
                 }

                 if (strAccessType == "Approver")
                 {
                     lblMainTitle.Text = "Committe Review";
                     trCategoryDD.Visible = false;
                     trTagscbl.Visible = false;
                     trUploads.Visible = false;
                     trPostedcb.Visible = false;
                     lblPendingTitle.Text = "Fixtures Awaiting Approval";
                     lblHistory.Visible = false;
                     gvHistory.Visible = false;
                 }
                 else
                 {
                     if (strAccessType == "Admin" || strAccessType == "Both")
                     {
                         //Retrieve Historical gridview data - all records that have been posted so they can be edited.
                         GetHistory();

                         lblMainTitle.Text = "Administration";
                         trCheckboxes.Visible = false;
                         trComments.Visible = false;
                         lblPendingTitle.Text = "Fixtures Ready to Post";
                         lblHistory.Text = "Posted Fixtures";
                     }
                 }
             }
         }

         //**Used for testing Access Type 
         lblMessage.Visible = true;
         lblMessage.Text = Session["flgroup"].ToString();
     }




    private void GetPostList(string strAccessType)
    {
        //Passes the user's access type to the stored procedure returning list of appropriate Job listing awaiting action.
            using (SqlConnection conn = new SqlConnection(MBIntranet_DEV))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "flGetPending";
                    cmd.Parameters.Add("@AccessType", SqlDbType.VarChar).Value = strAccessType;
                    cmd.Connection = conn;

                    try
                    {
                        //Open connection and bind datasource to gridview
                        conn.Open();
                        gvPending.DataSource = cmd.ExecuteReader();
                        gvPending.DataBind();

                        if (gvPending.Rows.Count == 0)
                        {
                            //Display No records message if no data found.
                            lblMessage.Visible = true;
                            lblMessage.Text = "There are no records that need approval at this time.";
                        }
                        else
                        {
                            lblMessage.Text = string.Empty;
                        }
                    }
                    //Error handeling
                    catch (Exception ex)
                    {
                        lblMessage.Visible = true;
                        lblMessage.Text = "Error Msg: " + ex.Message + " was received. Please contact support@missionbell.com with a screenshot of the page";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
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



    private void GetHistory()
    {
        //Passes the user's access type to the stored procedure returning list of appropriate Job listing awaiting action.
        using (SqlConnection conn = new SqlConnection(MBIntranet_DEV))
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "flGetHistory";
                cmd.Connection = conn;

                try
                {
                    //Open connection and bind datasource to gridview
                    conn.Open();
                    gvHistory.DataSource = cmd.ExecuteReader();
                    gvHistory.DataBind();

                    if (gvHistory.Rows.Count == 0)
                    {
                        //Display No records message if no data found.
                        lblHistory.Visible = false;
                        gvHistory.Visible = false;    
                    }
                }
                //Error handeling
                catch (Exception ex)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Error Msg: " + ex.Message + " was received while trying to retrieve the Fixtures Library images. Please contact support@missionbell.com with a screenshot of the page";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
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
        //
        using (SqlConnection conn = new SqlConnection(MBIntranet_DEV))
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "flGetLaborDetails";
                cmd.Parameters.Add("@PostID", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["PostID"]);
                cmd.Connection = conn;

                try
                {
                    //Open connection and bind datasource to gridview
                    conn.Open();
                    gvLaborDetails.DataSource = cmd.ExecuteReader();
                    gvLaborDetails.DataBind();

                    if (gvLaborDetails.Rows.Count == 0)
                    {
                        //Display No records message if no data found.
                        lblMessage.Visible = true;
                        lblMessage.Text = "No Labor Details were retrieved. Please contact support@missionbell.com for assistance.";
                    }
                    else
                    {
                        lblMessage.Text = string.Empty;
                    }
                }
                //Error handeling
                catch (Exception ex)
                {
                    lblMessage.Text = "Error Msg: " + ex.Message + " was received while trying to retrieve the Labor Details. Please contact support@missionbell.com with a screenshot of the page";
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




    protected void ddCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddSubCategory.Enabled = true;
        BindSubCategory();
    }




    private void BindSubCategory()
    {
        //clear subcategory list before adding to it
        ddSubCategory.Items.Clear();
        //Add Select and All items to the Subcategory drop down list
        ddSubCategory.Items.Insert(0, "Select SubCategory");
        //Filter Subcategory list based upon Category Selected
        dsSubCategories.SelectCommand = "SELECT CategoryID, CategoryName FROM flCategories WHERE ParentID = " + ddCategory.SelectedValue;
        ddSubCategory.DataBind();
    }



    protected void ddCategory_OnItemInserted(object sender, EventArgs e)
    {
        string strCategory = "Category";

        //Insert new item in flCategories table
        using (SqlConnection conn = new SqlConnection(MBIntranet_DEV))
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "flInsertCategories";
                cmd.Parameters.Add("@strItem", SqlDbType.VarChar).Value = strCategory;
                cmd.Parameters.Add("@strText", SqlDbType.VarChar).Value = ddCategory.SelectedItem.ToString();
                cmd.Parameters.Add("@intCategoryID", SqlDbType.Int).Value = 0;
                cmd.Connection = conn;

                try
                {
                    //Open connection and bind datasource to gridview
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        Int32 intCategoryExists = Convert.ToInt32(reader["CategoryExists"]);
                        if (intCategoryExists == 1)
                        {
                            //Display Message
                            lblMessage.Visible = true;
                            lblMessage.Text = "That Category already exists in the Dropdown. If this message is an error, please contact support@missionbell.com.";
                            lblMessage.ForeColor = System.Drawing.Color.Red;
                        }
                        reader.Close();
                    }
                    else
                    {
                        lblMessage.Visible = true;
                        lblMessage.Text = "no data found";
                    }
                }
                //Error handeling
                catch (Exception ex)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Error Msg: " + ex.Message + " was received while trying to retrieve the Labor Details. Please contact support@missionbell.com with a screenshot of the page";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
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



    protected void ddSubCategory_OnItemInserted(object sender, EventArgs e)
    {
        //Insert new item in flCategories table
        string strCategory = "SubCategory";

        if (!string.IsNullOrEmpty(ddCategory.SelectedValue))
        {
            //Insert new item in flCategories table
            using (SqlConnection conn = new SqlConnection(MBIntranet_DEV))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "flInsertCategories";
                    cmd.Parameters.Add("@strItem", SqlDbType.VarChar).Value = strCategory;
                    cmd.Parameters.Add("@strText", SqlDbType.VarChar).Value = ddSubCategory.SelectedItem.ToString();
                    cmd.Parameters.Add("@intCategoryID", SqlDbType.Int).Value = ddCategory.SelectedValue;
                    cmd.Connection = conn;

                    try
                    {
                        //Open connection and bind datasource to gridview
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            reader.Read();
                            Int32 intCategoryExists = Convert.ToInt32(reader["SubCategoryExists"]);
                            if (intCategoryExists == 1)
                            {
                                //Display Message
                                lblMessage.Visible = true;
                                lblMessage.Text = "The SubCategory already exists in the Dropdown. If this message is an error, please contact support@missionbell.com.";
                                lblMessage.ForeColor = System.Drawing.Color.Red;
                            }
                            reader.Close();
                            ddSubCategory.Enabled = true;
                        }
                        else
                        {
                            lblMessage.Visible = true;
                            lblMessage.Text = "no data found";
                        }
                    }
                    //Error handeling
                    catch (Exception ex)
                    {
                        lblMessage.Visible = true;
                        lblMessage.Text = "Error Msg: " + ex.Message + " was received while trying to retrieve the Labor Details. Please contact support@missionbell.com with a screenshot of the page";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
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
        else
        {
            lblMessage.Visible = true;
            lblMessage.Text = "Please choose a Category before inserting a new SubCategory.";
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
    }




    private void BindJobDetails()
    {
        //submit selected values to stored procedure and retrieve results
        //temp populated into gridveiw
        using (SqlConnection conn = new SqlConnection(MBIntranet_DEV))
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "flGetJobDetails";
                cmd.Parameters.Add("@PostID", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["PostID"]);
                cmd.Connection = conn;

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        lblProjectName.Text = reader["txtJobName"].ToString();
                        lblJobNum.Text = reader["txtJobNumber"].ToString();
                        lblGC.Text = reader["txtCustomerName"].ToString();
                        lblArhitect.Text = reader["txtArchitectName"].ToString();
                        lblJobCity.Text = reader["txtJobCity"].ToString();
                        if (reader["CategoryID"] != DBNull.Value)
                        {
                            ddCategory.SelectedValue = Convert.ToInt32(reader["CategoryID"]).ToString();

                            if (reader["SubCategoryID"] != DBNull.Value)
                            {
                                ddSubCategory.Enabled = true;
                                dsSubCategories.SelectCommand = "SELECT CategoryID, CategoryName FROM flCategories WHERE ParentID = " + Convert.ToInt32(reader["CategoryID"]).ToString();
                                ddSubCategory.DataBind();
                                ddSubCategory.SelectedValue = Convert.ToInt32(reader["SubCategoryID"]).ToString();
                            }
                            else
                            {
                                ddSubCategory.Enabled = false;
                            }
                        }

                        GetImages();

                        if (Convert.ToInt32(reader["StatusID"]) == 3)
                        {
                            cbArchive.Visible = true;
                            cbPostedStatus.Visible = false;
                        }
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Error Message: " + ex.Message + " was received while trying to retrieve Project Data. Please contact support@missionbell.com for assistance.";
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




    protected void GetImages()
    {
        using (SqlConnection conn = new SqlConnection(MBIntranet_DEV))
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "flGetImages";
                cmd.Parameters.Add("@PostID", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["PostID"]);
                cmd.Connection = conn;

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        txtCurrPrimFile.Visible = true;
                        lblCurrPrimFile.Visible = true;
                        btnAddImages.Visible = true;
                        tblAdditionalImages.Visible = false;
                        ViewState["strHasImages"] = "1";
                        while (reader.Read())
                        {
                            if (string.IsNullOrEmpty(txtCurrPrimFile.Text))
                            {
                                txtCurrPrimFile.Text = reader["flImageThumb"].ToString();
                            }
                            else
                            {
                                txtCurrPrimFile.Text = txtCurrPrimFile.Text + " <br />" + reader["flImageThumb"].ToString();
                            }
                        }
                    }
                    else
                    {
                        ViewState["strHasImages"] = "0";
                        txtCurrPrimFile.Visible = false;
                        lblCurrPrimFile.Visible = false;
                        btnAddImages.Visible = false;
                        tblAdditionalImages.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Error Message: " + ex.Message + " was received while trying to retrieve Project Data. Please contact support@missionbell.com for assistance.";
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





    protected void cblTags_OnDataBound(object sender, EventArgs e)
    {
        //Loops through each of the tags to determine which one has been selected in the database
        using (SqlConnection conn = new SqlConnection(MBIntranet_DEV))
            {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT TagID, PostID FROM dbo.flTagLookup WHERE PostID = " + Request.QueryString["PostID"];
                cmd.Connection = conn;

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    //Retrieve List of Tags from DB
                    while (reader.Read())
                    {
                        //Loops through each Tag to determine if it needs to be selected or not.
                        for (int i = 0; i < cblTags.Items.Count; i++)
                        {
                            string strTagIndex = cblTags.Items[i].Value;
                            if (strTagIndex == reader["TagID"].ToString())
                            {
                                cblTags.Items[i].Selected = true;
                            }
                        }
                    }
                }
                catch (Exception e2)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Error Msg:" + e2.Message + " Please contact support@missionbell.com";
                }
                finally
                {
                    cmd.Dispose();
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




    protected void cbApprove_OnCheckChanged(object sender, EventArgs e)
    {
        if (cbApprove.Checked)
        {
            cbDecline.Enabled = false;
        }
        else
        {
            cbDecline.Enabled = true;
        }
    }



    protected void cbDecline_OnCheckChanged(object sender, EventArgs e)
    {
        if (cbDecline.Checked)
        {
            cbApprove.Enabled = false;
        }
        else
        {
            cbApprove.Enabled = true;
        }
    }



    protected void btnSubmit_OnClick(object sender, EventArgs e)
    {
        //strAccessType = vaClass.VerifyflAdminAccess();
        strAccessType = Session["flgroup"].ToString();
        //Code below is for Approver hitting submit button
        if (strAccessType == "Approver")
        {
            if ((!cbApprove.Checked) && (!cbDecline.Checked))
            {
                lblMessage.Visible = true;
                lblMessage.Text = "Please check either Approve or Decline before continuing.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                if (cbApprove.Checked)
                {
                    intStatus = 2;
                }
                else
                {
                    if (cbDecline.Checked)
                    {
                        intStatus = 4;
                    }
                    else
                    {
                        intStatus = 1;
                    }
                }


                using (SqlConnection conn = new SqlConnection(MBIntranet_DEV))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "flUpdateApprover";
                        cmd.Parameters.Add("@PostID", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["PostID"]);
                        cmd.Parameters.Add("@strStatus", SqlDbType.VarChar).Value = strAccessType;
                        cmd.Parameters.Add("@intStatusID", SqlDbType.Int).Value = intStatus;
                        cmd.Parameters.Add("@strComments", SqlDbType.VarChar).Value = txtComments.Text.Trim();
                        cmd.Connection = conn;

                        try
                        {
                            conn.Open();
                            SqlDataReader reader = cmd.ExecuteReader();
                            reader.Read();
                            string strJobName = reader["txtJobName"].ToString();
                            string strCurrentStatus = reader["Status"].ToString();
                            reader.Close();

                            GetPostList(strAccessType);
                            lblMessage.Visible = true;
                            lblMessage.Text = "The " + strJobName + " Fixtures Listing has been " + strCurrentStatus + ".";
                            tcData.Visible = false;

                            //send Email to blaine once approved.
                            //if (strCurrentStatus == "Approved")
                            //{
                                //Call Class to send Email
                             //   string strBody = "The Committee has approved a new item for the Fixture Library.  Please visit the Administration at iddo:88/FLAdminReview.aspx page to Post.";
                                //**Need to get from AD flAdmin group
                             //   string strSendTo = "BlaineG@missionbell.com";
                             //   string strSubject = "New Fixtures Library Approval";
                             //   vaClass.SendEmail(strBody, strSendTo, strSubject);
                            //}
                            //Update list of Submitted Fixtures on left hand side
                            GetPostList(strAccessType);

                        }
                        catch (Exception ex)
                        {
                            lblMessage.Visible = true;
                            lblMessage.Text = "Error Message: " + ex.Message + " while attemting to update the record. Please contact support@missionbell.com for assistance.";
                            lblMessage.ForeColor = System.Drawing.Color.Red;

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
        else
        {
            //Code for Admin Submitting Record

            string strNewFilePath;

            //(string.IsNullOrEmpty(categoryList0.SelectedValue)

            if (!string.IsNullOrEmpty(ddCategory.SelectedValue) && string.IsNullOrEmpty(ddSubCategory.SelectedValue))
            {
                lblMessage.Visible = true;
                lblMessage.Text = "Please select both a Category and SubCategory before saving your changes.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                if (cbPostedStatus.Checked)
                {
                    if (((!(PrimaryfileUpload.HasFile) && (!PrimaryfileUploadl.HasFile) && (!FileUpload2s.HasFile) && (!FileUpload2l.HasFile) && (!FileUpload3s.HasFile) && (!FileUpload3l.HasFile) && (!FileUpload4s.HasFile) && (!FileUpload4l.HasFile) && (!FileUpload5s.HasFile) && (!FileUpload5l.HasFile) && (ViewState["strHasImages"] == "0"))) || (string.IsNullOrEmpty(ddCategory.SelectedValue) && string.IsNullOrEmpty(ddSubCategory.SelectedValue)))
                    {
                        lblMessage.Visible = true;
                        lblMessage.Text = "You must select your categories and at least one image before posting.";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        return;
                    }
                    else
                    {
                        //check for any populated tags
                        Int32 iFlagCount = 0;

                        for (int i = 0; i < cblTags.Items.Count; i++)
                        {
                            if (cblTags.Items[i].Selected)
                            {
                                iFlagCount += 1;
                                if (iFlagCount == 3)
                                {
                                    break;
                                }
                            }
                        }
                        if (iFlagCount > 2)
                        {
                            intStatus = 3;
                        }
                        else
                        {
                            lblMessage.Visible = true;
                            lblMessage.Text = "You must select at least three Tags before posting.";
                            lblMessage.ForeColor = System.Drawing.Color.Red;
                            return;
                        }
                    }
                }
                else
                {
                    intStatus = 2;
                }

                using (SqlConnection conn = new SqlConnection(MBIntranet_DEV))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "flUpdateAdmin";
                        cmd.Parameters.Add("@PostID", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["PostID"]);
                        cmd.Parameters.Add("@intSubCategoryID", SqlDbType.VarChar).Value = ddSubCategory.SelectedValue;
                        cmd.Parameters.Add("@intPosted", SqlDbType.Int).Value = intStatus;

                        //Verify File exists in Attachment field before submitting form
                        if (PrimaryfileUpload.HasFile)
                        {
                            strNewFilePath = GetImagePath(PrimaryfileUpload.FileName);
                            PrimaryfileUpload.SaveAs(Server.MapPath(strNewFilePath));
                            cmd.Parameters.Add("@PrimaryImgPaths", SqlDbType.VarChar).Value = strNewFilePath;
                        }
                        else
                        {
                            cmd.Parameters.Add("@PrimaryImgPaths", SqlDbType.VarChar).Value = "N/A";
                        }
                        if (PrimaryfileUploadl.HasFile)
                        {
                            strNewFilePath = GetImagePath(PrimaryfileUploadl.FileName);
                            PrimaryfileUploadl.SaveAs(Server.MapPath(strNewFilePath));
                            cmd.Parameters.Add("@PrimaryImgPathl", SqlDbType.VarChar).Value = strNewFilePath;
                        }
                        else
                        {
                            cmd.Parameters.Add("@PrimaryImgPathl", SqlDbType.VarChar).Value = "N/A";
                        }
                        if (FileUpload2s.HasFile)
                        {
                            strNewFilePath = GetImagePath(FileUpload2s.FileName);
                            FileUpload2s.SaveAs(Server.MapPath(strNewFilePath));
                            cmd.Parameters.Add("@FileUpload2s", SqlDbType.VarChar).Value = strNewFilePath;
                        }
                        else
                        {
                            cmd.Parameters.Add("@FileUpload2s", SqlDbType.VarChar).Value = "N/A";
                        }
                        if (FileUpload2l.HasFile)
                        {
                            strNewFilePath = GetImagePath(FileUpload2l.FileName);
                            FileUpload2l.SaveAs(Server.MapPath(strNewFilePath));
                            cmd.Parameters.Add("@FileUpload2l", SqlDbType.VarChar).Value = strNewFilePath;
                        }
                        else
                        {
                            cmd.Parameters.Add("@FileUpload2l", SqlDbType.VarChar).Value = "N/A";
                        }
                        if (FileUpload3s.HasFile)
                        {
                            strNewFilePath = GetImagePath(FileUpload3s.FileName);
                            FileUpload3s.SaveAs(Server.MapPath(strNewFilePath));
                            cmd.Parameters.Add("@FileUpload3s", SqlDbType.VarChar).Value = strNewFilePath;
                        }
                        else
                        {
                            cmd.Parameters.Add("@FileUpload3s", SqlDbType.VarChar).Value = "N/A";
                        }
                        if (FileUpload3l.HasFile)
                        {
                            strNewFilePath = GetImagePath(FileUpload3l.FileName);
                            FileUpload3l.SaveAs(Server.MapPath(strNewFilePath));
                            cmd.Parameters.Add("@FileUpload3l", SqlDbType.VarChar).Value = strNewFilePath;
                        }
                        else
                        {
                            cmd.Parameters.Add("@FileUpload3l", SqlDbType.VarChar).Value = "N/A";
                        }
                        if (FileUpload4s.HasFile)
                        {
                            strNewFilePath = GetImagePath(FileUpload4s.FileName);
                            FileUpload4s.SaveAs(Server.MapPath(strNewFilePath));
                            cmd.Parameters.Add("@FileUpload4s", SqlDbType.VarChar).Value = strNewFilePath;
                        }
                        else
                        {
                            cmd.Parameters.Add("@FileUpload4s", SqlDbType.VarChar).Value = "N/A";
                        }
                        if (FileUpload4l.HasFile)
                        {
                            strNewFilePath = GetImagePath(FileUpload4l.FileName);
                            FileUpload4l.SaveAs(Server.MapPath(strNewFilePath));
                            cmd.Parameters.Add("@FileUpload4l", SqlDbType.VarChar).Value = strNewFilePath;
                        }
                        else
                        {
                            cmd.Parameters.Add("@FileUpload4l", SqlDbType.VarChar).Value = "N/A";
                        }
                        if (FileUpload5s.HasFile)
                        {
                            strNewFilePath = GetImagePath(FileUpload5s.FileName);
                            FileUpload5s.SaveAs(Server.MapPath(strNewFilePath));
                            cmd.Parameters.Add("@FileUpload5s", SqlDbType.VarChar).Value = strNewFilePath;
                        }
                        else
                        {
                            cmd.Parameters.Add("@FileUpload5s", SqlDbType.VarChar).Value = "N/A";
                        }
                        if (FileUpload5l.HasFile)
                        {
                            strNewFilePath = GetImagePath(FileUpload5l.FileName);
                            FileUpload5l.SaveAs(Server.MapPath(strNewFilePath));
                            cmd.Parameters.Add("@FileUpload5l", SqlDbType.VarChar).Value = strNewFilePath;
                        }
                        else
                        {
                            cmd.Parameters.Add("@FileUpload5l", SqlDbType.VarChar).Value = "N/A";
                        }
                        cmd.Connection = conn;


                        try
                        {
                            conn.Open();
                            SqlDataReader reader = cmd.ExecuteReader();
                            //get code for no records found
                            if (reader.HasRows)
                            {
                                reader.Read();
                                string strJobName = reader["txtJobName"].ToString();
                                lblMessage.Text = "The " + strJobName + " Fixtures Listing has been Updated.";
                            }
                            reader.Close();

                            InsertTags();

                            lblMessage.Visible = true;

                            tcData.Visible = false;
                            
                            //Update the list of approved posts on Left hand side
                            GetPostList(strAccessType);
                            //Update the list of Submitted posts on left hand side
                            GetHistory();
                        }
                        catch (Exception ex)
                        {
                            lblMessage.Visible = true;
                            lblMessage.Text = "Error Msg: " + ex.Message + " while updating the record. Please call support@missionbell.com for assistance.";
                            lblMessage.ForeColor = System.Drawing.Color.Red;
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
    }




    protected void InsertTags()
    {
        //only do this portion if one of the tags are selected
        string cbFlag = "0";
        for (int i = 0; i < cblTags.Items.Count; i++)
        {
            if (cblTags.Items[i].Selected)
            {
                cbFlag = "1";
                break;
            }
        }

        if (cbFlag == "1")
        {

            //Delete record with postid
            using (SqlConnection conn = new SqlConnection(MBIntranet_DEV))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "DELETE FROM dbo.flTagLookup WHERE PostID = " + Request.QueryString["PostID"];
                    cmd.Connection = conn;

                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        reader.Read();
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        //Display error message when form submission unsuccessful
                        lblMessage.Visible = true;
                        lblMessage.Text = "Error (Insert SysInfo Apps): " + ex.Message + ".<br>" + strErrMsg;
                    }
                    finally
                    {
                        cmd.Dispose();
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }



            using (SqlConnection conn = new SqlConnection(MBIntranet_DEV))
            {
                //loop through check box list to insert
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO dbo.flTagLookup (PostID, TagID) VALUES (@PostID, @TagID)";
                    cmd.Connection = conn;

                    try
                    {
                        conn.Open();
                        foreach (ListItem item in cblTags.Items)
                        {
                            if (item.Selected)
                            {
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@PostID", Request.QueryString["PostID"]);
                                cmd.Parameters.AddWithValue("@TagID", item.Value);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //Display error message when form submission unsuccessful
                        lblMessage.Visible = true;
                        lblMessage.Text = "Error (Insert SysInfo Apps): " + ex.Message + ".<br>" + strErrMsg;
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




    private string GetImagePath(string strImageName)
    {
        string strFileName;
        string strFilePath;
        string strDirectory;

        try
        {
            strFileName = System.IO.Path.GetFileName(strImageName);
            strDirectory = "~/images/" + lblProjectName.Text.Trim();

            if (!Directory.Exists(strDirectory))
            {
                Directory.CreateDirectory(MapPath(strDirectory));
            }
            strFilePath = "images/" + lblProjectName.Text.Trim() + "/" + strFileName;

            return strFilePath;
            //PrimaryfileUpload.SaveAs(Server.MapPath(strFilePath));
            
        }
        catch (Exception ex)
        {
            //Display error message when form submission unsuccessful --error with file upload
            lblMessage.Visible = true;
            lblMessage.Text = "Error Msg(Insert Data): " + ex.Message + ".<br>" + strErrMsg;

            return null;
        }
    }




    protected void btnAddImages_Onclick(object sender, EventArgs e)
    {
        txtCurrPrimFile.Visible = false;
        lblCurrPrimFile.Visible = false;
        btnAddImages.Visible = false;
        tblAdditionalImages.Visible = true;
    }





}
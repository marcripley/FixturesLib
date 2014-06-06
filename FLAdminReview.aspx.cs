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

    string MBIntranet_DEV = ConfigurationManager.ConnectionStrings["MBData2005_DEV"].ConnectionString;

    public Int32 ProjSum = 0;
    public Int32 ActualSum = 0;

    public Int32 intStatus;

    //Declare Class - VerifyAccess.cs file in App_Code folder
     VerifyAccess vaClass = new VerifyAccess();

    public Int32 intPostID;


    protected void Page_Load(object sender, EventArgs e)
    {
        //For users with no access - display the following message
        if (vaClass.VerifyflAdminAccess() == "None")
        {
            lblMessage.Visible = true;
            lblMessage.Text = "You do not have access to this page.  If you feel you've reached this message in error, please contact support@missionbell.com.";
            lblMainTitle.Text = "Fixtures Library Administration Page";
            tblMain.Visible = false;
        }
        else
        {
            //For users with access to the Administration page
            GetLaborList();
            
            if (!!String.IsNullOrEmpty(Request.QueryString["JID"]))
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

            if (vaClass.VerifyflAdminAccess() == "Approver")
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
                if (vaClass.VerifyflAdminAccess() == "Admin")
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



    private void GetLaborList()
    {
        //Passes the user's access type to the stored procedure returning list of appropriate Job listing awaiting action.
            using (SqlConnection conn = new SqlConnection(MBIntranet_DEV))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "flGetPending";
                    cmd.Parameters.Add("@AccessType", SqlDbType.VarChar).Value = vaClass.VerifyflAdminAccess();
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
                cmd.Parameters.Add("@JobNumber", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["JID"]);
                cmd.Parameters.Add("@TaskNumber", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["TID"]);
                cmd.Parameters.Add("@strStatus", SqlDbType.VarChar).Value = vaClass.VerifyflAdminAccess();
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
                cmd.Parameters.Add("@JobNumber", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["JID"]);
                cmd.Parameters.Add("@TaskNumber", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["TID"]);
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
                        }
                        if (reader["SubCategoryID"] != DBNull.Value)
                        {
                            ddSubCategory.SelectedValue = Convert.ToInt32(reader["SubCategoryID"]).ToString();
                        }

                        if (string.IsNullOrEmpty(reader["img1"].ToString()))
                        {
                            txtCurrPrimFile.Text = reader["img1"].ToString();
                        }
                        if (Convert.ToInt32(reader["StatusID"]) == 4)
                        {
                            cbArchive.Visible = true;
                            cbArchive.Checked = true;
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




    protected void cblTags_OnDataBound(object sender, EventArgs e)
    {
        //Loops through each of the tags to determine which one has been selected in the database
        using (SqlConnection conn = new SqlConnection(MBIntranet_DEV))
            {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT TagID, PostID FROM dbo.flTagLookup WHERE PostID = 1";
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



    protected void AsyncFileUpload_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)  
    { 
        System.Threading.Thread.Sleep(5000);
        //if (AsyncFileUpload.HasFile)
        {
        //    string strPath = MapPath("~/Uploads/") + Path.GetFileName(e.filename);
        //    string filename = System.IO.Path.GetFileName(AsyncFileUpload.FileName);
        //    AsyncFileUpload.SaveAs(Server.MapPath("FileUploads/") + filename);    
        } 
    } 



    protected void btnSubmit_OnClick(object sender, EventArgs e)
    {
        //Code below is for Approver hitting submit button
        if (vaClass.VerifyflAdminAccess() == "Approver")
        {
            if ((!cbApprove.Checked) && (!cbDecline.Checked))
            {
                lblMessage.Visible = true;
                lblMessage.Text = "Please check either Approve or Decline before continuing.";
            }
            else
            {
                if (cbApprove.Checked)
                {
                    intStatus = 2;
                }
                else
                {
                    intStatus = 3;
                }
           

                using (SqlConnection conn = new SqlConnection(MBIntranet_DEV))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "flUpdateApprover";
                        cmd.Parameters.Add("@intJobNumber", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["JID"]);
                        cmd.Parameters.Add("@intTaskNumber", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["TID"]);
                        cmd.Parameters.Add("@strStatus", SqlDbType.VarChar).Value = vaClass.VerifyflAdminAccess();
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

                            lblMessage.Visible = true;
                            lblMessage.Text = "The " + strJobName + " Fixtures Listing has been Updated to " + strCurrentStatus + " Staus.";
                            tcData.Visible = false;
                            GetLaborList();

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

            //variables for Uploaded files
            string strFileName;
            string strFilePath;
            string strDirectory;

            using (SqlConnection conn = new SqlConnection(MBIntranet_DEV))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "flUpdateAdmin";
                    cmd.Parameters.Add("@intJobNumber", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["JID"]);
                    cmd.Parameters.Add("@intTaskNumber", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["TID"]);
                    cmd.Parameters.Add("@strStatus", SqlDbType.VarChar).Value = vaClass.VerifyflAdminAccess();
                    cmd.Parameters.Add("@intCategoryID", SqlDbType.VarChar).Value = ddCategory.SelectedValue;
                    cmd.Parameters.Add("@intSubCategoryID", SqlDbType.VarChar).Value = ddSubCategory.SelectedValue;
                    cmd.Parameters.Add("@intPosted", SqlDbType.Int).Value = cbPostedStatus.Checked;

                    //Verify File exists in Attachment field before submitting form
                    if (PrimaryfileUpload.HasFile)
                    {
                        try
                        {
                            strFileName = System.IO.Path.GetFileName(PrimaryfileUpload.FileName);
                            strDirectory = "~/images/" + lblProjectName.Text.Trim();

                            if (!Directory.Exists(strDirectory))
                            {
                                Directory.CreateDirectory(MapPath(strDirectory));
                            }
                            strFilePath = strDirectory + "/" + strFileName;
                            PrimaryfileUpload.SaveAs(Server.MapPath(strFilePath));
                            cmd.Parameters.Add("@PrimaryImgPath", SqlDbType.VarChar).Value = strFilePath;
                        }
                        catch (Exception ex)
                        {
                            //Display error message when form submission unsuccessful --error with file upload
                            lblMessage.Visible = true;
                            lblMessage.Text = "Error Msg(Insert Data): " + ex.Message + ".<br>" + strErrMsg;
                        }
                    }
                    else
                    {
                        cmd.Parameters.Add("@PrimaryImgPath", SqlDbType.VarChar).Value = "NA";
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
                            reader.Close();

                            lblMessage.Visible = true;
                            lblMessage.Text = "The " + strJobName + " Fixtures Listing has been Updated.";
                            tcData.Visible = false;
                            GetLaborList();
                            GetHistory();
                        }
                        reader.Close();       
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
                    using (SqlCommand cmd = new SqlCommand())
                    {          
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "DELETE FROM dbo.flTagLookup WHERE PostID = " + intPostID;
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
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@PostID", intPostID);
                                cmd.Parameters.AddWithValue("@TagID", item.Value);   
                                cmd.ExecuteNonQuery();
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
            GetLaborList();
        }
    }




}
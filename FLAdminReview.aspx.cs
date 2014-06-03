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


public partial class FLAdminReview : System.Web.UI.Page
{
    //Public Declarations   
    public string strErrMsg;

    string MBIntranet_DEV = ConfigurationManager.ConnectionStrings["MBData2005_DEV"].ConnectionString;

    public Int32 ProjSum = 0;
    public Int32 ActualSum = 0;

    public Int32 intStatus;

    //Declare Class
     VerifyAccess vaClass = new VerifyAccess();

    public Int32 intPostID;


    protected void Page_Load(object sender, EventArgs e)
    {
        lblMessage.Visible = false;

        if (vaClass.VerifyflAdminAccess() == "None")
        {
            lblMessage.Visible = true;
            lblMessage.Text = "You do not have access to this page.  If you feel you've reached this message in error, please contact IT support.";
            tblMain.Visible = false;
        }
        else
        {
        
            //submit selected values to stored procedure and retrieve results
            //temp populated into gridveiw
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
                        SqlDataAdapter myadapter = new SqlDataAdapter();
                        myadapter.SelectCommand = cmd;
                        DataSet myDataSet = new DataSet();
                        myadapter.Fill(myDataSet);

                        DataView myDataView = new DataView();
                        myDataView = myDataSet.Tables[0].DefaultView;

                        gvPending.DataSource = myDataView;
                        gvPending.DataBind();

                        //Display No records message if no data found.
                        if (gvPending.Rows.Count == 0)
                        {
                            gvPending.Visible = false;
                        }
                        else
                        {
                            gvPending.Visible = true;
                        }
                        myadapter.Dispose();
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Visible = true;
                        lblMessage.Text = "Error Message: " + ex.Message;
                    }
                    finally
                    {
                        cmd.Dispose();
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }


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
                trSubCatDD.Visible = false;
                trCategoryDD.Visible = false;
                trTagscbl.Visible = false;
                trUploads.Visible = false;
            }
            else
            {
                if (vaClass.VerifyflAdminAccess() == "Admin")
                {
                    trCheckboxes.Visible = false;
                    txtComments.Visible = false;
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
                cmd.Parameters.Add("@JobNumber", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["JID"]);
                cmd.Parameters.Add("@TaskNumber", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["TID"]);
                cmd.Parameters.Add("@strStatus", SqlDbType.VarChar).Value = vaClass.VerifyflAdminAccess();
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
                    lblMessage.Visible = true;
                    lblMessage.Text = "Error Message: " + ex.Message;
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
        ddSubCategory.Items.Insert(0, "Select");
        

        //If user selects "All" from subcategory drop down, provide all otherwise filter by selected category
        if (ddCategory.SelectedValue == "1")
        {
            dsSubCategories.SelectCommand = "SELECT CategoryID, CategoryName FROM flCategories WHERE ParentID IS NOT NULL";
        }
        else
        {
            dsSubCategories.SelectCommand = "SELECT CategoryID, CategoryName FROM flCategories WHERE ParentID = " + ddCategory.SelectedValue;
        }

        ddSubCategory.DataBind();
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
                //cmd.Parameters.Add("@StatusID", SqlDbType.Int).Value = intStatus;
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

                       // lblComments.Text = reader["Comments"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Error Message: " + ex.Message;
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

                        lblMessage.Visible = true;
                        lblMessage.Text = "Record Updated";

                    }
                    catch (Exception ex)
                    {
                        lblMessage.Visible = true;
                        lblMessage.Text = "Error Message: " + ex.Message;
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
            //code for admin submitting

            string strFileName;
            string strFilePath;


            using (SqlConnection conn = new SqlConnection(MBIntranet_DEV))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "flUpdate";
                    cmd.Parameters.Add("@JobNumber", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["JID"]);
                    cmd.Parameters.Add("@TaskNumber", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["TID"]);
                    cmd.Parameters.Add("@strStatus", SqlDbType.VarChar).Value = vaClass.VerifyflAdminAccess();
                    cmd.Parameters.Add("@CategoryID", SqlDbType.VarChar).Value = ddCategory.SelectedValue;
                    cmd.Parameters.Add("@SubCategoryID", SqlDbType.VarChar).Value = ddSubCategory.SelectedValue;

                    //Verify File exists in Attachment field before submitting form
                    if (PrimaryfileUpload.HasFile)
                    {
                        try
                        {
                            strFileName = System.IO.Path.GetFileName(PrimaryfileUpload.FileName);
                            PrimaryfileUpload.SaveAs(Server.MapPath("~/images/" + lblProjectName.Text.Trim() + "/") + strFileName);
                            strFilePath = "~/images/" + lblProjectName.Text.Trim() + "/" + strFileName;

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
                            intPostID = Convert.ToInt32(reader["PostID"]);
                        }
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Visible = true;
                        lblMessage.Text = "Error Message: " + ex.Message;
                    }
                    finally
                    {
                        cmd.Dispose();
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }


            //loop through checkboxlist
            using (SqlConnection conn = new SqlConnection(MBIntranet_DEV))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO dbo.flTagLookup (PostID, TagID) VALUES (@PostID, @TagID)";
                    //cmd_Apps.Parameters.Add("@RFSID", SqlDbType.Int).Value = txtRFSID.Text;
                    cmd.Connection = conn;

                    try
                    {
                        conn.Open();

                        foreach (ListItem item in cblTags.Items)
                        {
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@AppID", item.Value);
                            cmd.Parameters.AddWithValue("@RFSID", intPostID);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Visible = true;
                        lblMessage.Text = ex.Message;
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
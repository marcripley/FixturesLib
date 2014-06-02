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

    //remove after testing
    public Int32 intStatus = 2;

    //public string strUsername;
    //public string strIsInAdminGroupFlag = "0";
    //public string strIsInApprovalGroupFlag = "0";
    //public string strAccessType;

    //Declare Class
     VerifyAccess vaClass = new VerifyAccess();


    protected void Page_Load(object sender, EventArgs e)
    {
        if (vaClass.VerifyflAdminAccess() == "None")
        {
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



    protected void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        //clear subcategory list before adding to it
        cbSubCategory.Items.Clear();
        //Add Select and All items to the Subcategory drop down list
        cbSubCategory.Items.Insert(0, "Select");
        

        //If user selects "All" from subcategory drop down, provide all otherwise filter by selected category
        if (cbCategory.SelectedValue == "1")
        {
            dsSubCategories.SelectCommand = "SELECT CategoryID, CategoryName FROM flCategories WHERE ParentID IS NOT NULL";
        }
        else
        {
            dsSubCategories.SelectCommand = "SELECT CategoryID, CategoryName FROM flCategories WHERE ParentID = " + cbCategory.SelectedValue;
        }

        cbSubCategory.DataBind();
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


}
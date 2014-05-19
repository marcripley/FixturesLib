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

public partial class FixtureDetails : System.Web.UI.Page
{
    string MBIntranet_DEV = ConfigurationManager.ConnectionStrings["MBData2005_DEV"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack && !String.IsNullOrEmpty(Request.QueryString["TID"]))
        {
            //First Call
            //put code to go back to home if no querystrings
            BindImages();
            BindJobDetails();
            BindLaborDetails();
        }
        else
        {

        }
    }


    private void BindImages()
    {
        Int32 iImageType = 2;

        //submit selected values to stored procedure and retrieve results
        //temp populated into gridveiw
        using (SqlConnection conn = new SqlConnection(MBIntranet_DEV))
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetImages";
                cmd.Parameters.Add("@JobNumber", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["JID"]);

                cmd.Parameters.Add("@TaskNumber", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["TID"]);

                cmd.Parameters.Add("@ImageType", SqlDbType.Int).Value = iImageType;

                cmd.Connection = conn;

                try
                {
                    SqlDataAdapter myadapter = new SqlDataAdapter();
                    myadapter.SelectCommand = cmd;
                    DataSet myDataSet = new DataSet();
                    myadapter.Fill(myDataSet);

                    DataView myDataView = new DataView();
                    myDataView = myDataSet.Tables[0].DefaultView;

                    gvImages.DataSource = myDataView;
                    gvImages.DataBind();


                    //Display No records message if no data found.
                    if (gvImages.Rows.Count == 0)
                    {
                        gvImages.Visible = false;

                    }
                    else
                    {
                        gvImages.Visible = true;

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



    private void BindLaborDetails()
    {
        //submit selected values to stored procedure and retrieve results
        //temp populated into gridveiw
        using (SqlConnection conn = new SqlConnection(MBIntranet_DEV))
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetLaborDetails";
                cmd.Parameters.Add("@JobNumber", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["JID"]);
                cmd.Parameters.Add("@TaskNumber", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["TID"]);
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


    private void BindJobDetails()
    {
        //submit selected values to stored procedure and retrieve results
        //temp populated into gridveiw
        using (SqlConnection conn = new SqlConnection(MBIntranet_DEV))
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetJobDetails";
                cmd.Parameters.Add("@JobNumber", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["JID"]);
                cmd.Parameters.Add("@TaskNumber", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["TID"]);
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
        Int32 ProjSum = 0;
        Int32 ActualSum = 0;

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
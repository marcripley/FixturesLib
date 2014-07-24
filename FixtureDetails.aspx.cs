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
    public Int32 ProjSum = 0;
    public Int32 ActualSum = 0;

    string MBIntranet_DEV = ConfigurationManager.ConnectionStrings["MBData2005"].ConnectionString;

    //Declare Class - VerifyAccess.cs file in App_Code folder
    VerifyAccess vaClass = new VerifyAccess();
    public string strAccessType;

    //public string strStatus = "Admin";

    protected void Page_Load(object sender, EventArgs e)
    {
        strAccessType = Session["flgroup"].ToString();
        if (strAccessType == "None")
        {
            hpl_FLAdmin.Visible = false;
        }
        else
        {
            hpl_FLAdmin.Visible = true;
        }

        if (!Page.IsPostBack && !String.IsNullOrEmpty(Request.QueryString["PostID"]))
        {
            //First Call
            BindImages();
            BindJobDetails();
            getTags();
            BindLaborDetails();
        }
        else
        {
            //Need PostID to retreive Data and Images so user must be redirected back to FL Home page
            Response.Redirect("Default.aspx");
        }
    }



    private void BindImages()
    {
        //Outershell for Slideshow -- Can remove this during phase II as it is not necessary.
        using (SqlConnection conn = new SqlConnection(MBIntranet_DEV))
        {
            // write the sql statement to execute
            string sql = "SELECT PostID FROM flPosts where Postid = " + Convert.ToInt32(Request.QueryString["PostID"]);

            // instantiate the command object to fire
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Connection = conn;

                try
                {
                    conn.Open();
                    gvPosts.DataSource = cmd.ExecuteReader();
                    gvPosts.DataBind();

                    //Display No records message if no data found.
                    if (gvPosts.Rows.Count == 0)
                    {
                        gvPosts.Visible = false;
                    }
                    //else
                    //{
                    //    lblMessage.Visible = true;
                    //    lblMessage.Text = "no records";
                    //}
                    
                }
                catch (Exception ex)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Error Msg: " + ex.Message + " was received while retrieving the Fixtures Library images. Please contact <u><a href=mailto:support@missionbell.com?subject=FL:ImageSlider;FixturesDetails.aspx>Support</a></u> with a screenshot of the page";
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
                    cmd.Parameters.Add("@PostId", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["PostID"]);
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
                        lblMessage.Visible = true;
                        lblMessage.Text = "Error Msg: " + ex.Message + " was received while trying to retrieve the Fixtures Library images. Please contact <u><a href=mailto:support@missionbell.com?subject=FL:ImageSlider;FixturesDetails.aspx>Support</a></u> with a screenshot of the page";
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




    private void BindLaborDetails()
    {
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
                    conn.Open();
                    gvLaborDetails.DataSource = cmd.ExecuteReader();
                    gvLaborDetails.DataBind();

                    //SqlDataAdapter myadapter = new SqlDataAdapter();
                    //myadapter.SelectCommand = cmd;
                    //DataSet myDataSet = new DataSet();
                   // myadapter.Fill(myDataSet);

                   // DataView myDataView = new DataView();
                   // myDataView = myDataSet.Tables[0].DefaultView;

                    //gvLaborDetails.DataSource = myDataView;
                    //gvLaborDetails.DataBind();

                    //Display No records message if no data found.
                    if (gvLaborDetails.Rows.Count == 0)
                    {
                        gvLaborDetails.Visible = false;
                    }
                    else
                    {
                        gvLaborDetails.Visible = true;
                    }
                    //myadapter.Dispose();
                }
                catch (Exception ex)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Error Msg: " + ex.Message + " was received while retrieving the FL Labor Details. Please contact <u><a href=mailto:support@missionbell.com?subject=FL:LaborDetails;FixturesDetails.aspx>Support</a></u> with a screenshot of the page";
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
                        lblCategory.Text = reader["CategoryName"].ToString();
                        lblSubCat.Text = reader["SubCategoryName"].ToString();
                        lblComments.Text = reader["Comments"].ToString();
                        lblMessage.Text = string.Empty;
                    }
                    else
                    {
                        lblMessage.Visible = true;
                        lblMessage.Text = "No Job Details found for this post.  Please contact contact <u><a href=mailto:support@missionbell.com?subject=FL:NoJobDetails;FixturesDetails.aspx>Support</a></u> with a screenshot of the page";
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Error Msg: " + ex.Message + " was received while retrieving the FL Job Details. Please contact <u><a href=mailto:support@missionbell.com?subject=FL:JobDetails;FixturesDetails.aspx>Support</a></u> with a screenshot of the page";
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




    protected void getTags()
    {
        using (SqlConnection conn = new SqlConnection(MBIntranet_DEV))
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "flGetTags";
                cmd.Parameters.Add("@PostID", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["PostID"]);
                cmd.Connection = conn;

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

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
                    else
                    {
                        lblMessage.Visible = true;
                        lblMessage.Text = "There are no Tags associated for this post.";
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Error Msg: " + ex.Message + " was received while retrieving the FL Tags. Please contact <u><a href=mailto:support@missionbell.com?subject=FL:Tags;FixturesDetails.aspx>Support</a></u> with a screenshot of the page";
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
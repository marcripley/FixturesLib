<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup
    }
    
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown
    }
      
      
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs
    }

    
    void Session_Start(object sender, EventArgs e) 
    {
        Session.Abandon();
        Session["login"] = null;
        
        
        // Code that runs when a new session is started
        //if (Session["login"] == null)
       //{
            string strLogin = User.Identity.Name.ToString();
            
            //used for testing
            //strLogin = "MISSIONBELL\\BlaineG";
            //strLogin = "MISSIONBELL\\Sharond";
            //strLogin = "MISSIONBELL\\RaquelM";
            
            Session["login"] = (strLogin.Substring(strLogin.Length - 12)).ToUpper();
            
            
            if (Session["flgroup"] == null)
            {
                string strIsInAdminGroupFlag;
                string strIsInApprovalGroupFlag;
                string strAccessType;
                                           
                if (User.IsInRole("MISSIONBELL\\flAdmin"))
                {
                    strIsInAdminGroupFlag = "1";
                }
                else
                {
                    strIsInAdminGroupFlag = "0";
                }
                
                //Assigns Approval Flag is user logged in is in the group
                if (User.IsInRole("MISSIONBELL\\flApproval")) 
                {
                strIsInApprovalGroupFlag = "1";
                }
                else
                {
                    strIsInApprovalGroupFlag = "0";
                }


                
                if (strIsInAdminGroupFlag == "0" && strIsInApprovalGroupFlag == "0")
                {
                    strAccessType = "None";
                }
                else
                {
                    if (strIsInAdminGroupFlag == "1" && strIsInApprovalGroupFlag == "1")
                    {
                        strAccessType = "Both";
                    }
                    else
                    {
                        if (strIsInAdminGroupFlag == "1")
                        {
                            strAccessType = "Admin";
                        }
                        else
                        {
                            strAccessType = "Approver";
                        }
                    }
                }

                Session["flgroup"] = strAccessType;  
            }
        //}     
    }

    
    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.
        Session.Abandon();
        
    }
       
</script>

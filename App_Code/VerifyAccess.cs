using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.DirectoryServices.AccountManagement;
using System.Net.Mail; //Used for Email
using System.Net; //Used for Emails

/// Created By: Jenise Marcus
/// Created Date: 6/3/2014
/// Description: Determines user logged onto the site using AD. Checks the two FL groups to determine their access.
/// </summary>
public class VerifyAccess
{
    //Public Declarations
    public string strUsername;
    public string strIsInAdminGroupFlag = "0";
    public string strIsInApprovalGroupFlag = "0";
    public string strAccessType;


    public string VerifyflAdminAccess()
    {
        //Checks if user is in flAdmin or flApproval group to help determine what type of access they will receive.
        string strAdminGroup = "flAdmin";
        string strApprovalGroup = "flApproval";

        //verifyAccess method in CommonMethods.cs class file.
        PrincipalContext ctx = new PrincipalContext(ContextType.Domain);
        // find currently logged in user
        UserPrincipal user = UserPrincipal.Current;
        //strUsername = user.DisplayName;
        //***Need to accomadate ' apostrophe's 
        //strUsername = "Blaine Gulbinas";
        strUsername = "Sharon de la Cruz";
        //strUsername = "Raquel Martinez";
        UserPrincipal user2 = UserPrincipal.FindByIdentity(ctx, strUsername);

        //Find Admin group
        GroupPrincipal Admingroup = GroupPrincipal.FindByIdentity(ctx, strAdminGroup);
        GroupPrincipal Approvalgroup = GroupPrincipal.FindByIdentity(ctx, strApprovalGroup);

        
        if (user2 != null)
        {
            try
            {
                //Assigns Admin Flag is user logged in is in the group
                if (user2.IsMemberOf(Admingroup))
                {
                    strIsInAdminGroupFlag = "1";
                }
                else
                {
                    strIsInAdminGroupFlag = "0";
                }
                //Assigns Approval Flag is user logged in is in the group
                if (user2.IsMemberOf(Approvalgroup))
                {
                    strIsInApprovalGroupFlag = "1";
                }
                else
                {
                    strIsInApprovalGroupFlag = "0";
                }

            }
            catch(Exception e)
            {
                return e.Message;
            }
        }

        //Assigns string depending upon if user is not in either group, in on of the groups or is in both groups.
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

        return strAccessType;
    }




    public void SendEmail(string strBody, string strSendTo, string strSubject)
    {
        string strSendFrom = "test@misisonbell.com";

        //***Remove after testing
        strSendTo = "jenisem@missionbell.com";

        //Create MailMessage Object
        MailMessage email_msg = new MailMessage();
        //Specifying From,Sender & Reply to address
        email_msg.From = new MailAddress(strSendFrom);
        email_msg.Sender = new MailAddress(strSendFrom);
        email_msg.To.Add(strSendTo);
        email_msg.Subject = strSubject;
        email_msg.Body = strBody;
        email_msg.Priority = MailPriority.Normal;
        //Create an object for SmtpClient class
        SmtpClient mail_client = new SmtpClient();
        //Providing Credentials 
        NetworkCredential network_cdr = new NetworkCredential();
        network_cdr.UserName = strSendFrom;
        //SMTP host
        mail_client.Host = "SMTP.in.missionbell.com";
        mail_client.UseDefaultCredentials = false;
        mail_client.Credentials = network_cdr;
        //Now Send the message
        mail_client.Send(email_msg);
    }


}
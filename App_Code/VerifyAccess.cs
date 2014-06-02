using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.DirectoryServices.AccountManagement;

/// <summary>
/// Summary description for VerifyAccess
/// </summary>
public class VerifyAccess
{

    public string strUsername;
    public string strIsInAdminGroupFlag = "0";
    public string strIsInApprovalGroupFlag = "0";
    public string strAccessType;

    public string VerifyflAdminAccess()
    {
        string strAdminGroup = "flAdmin";
        string strApprovalGroup = "flApproval";

        //verifyAccess method in CommonMethods.cs class file.
        PrincipalContext ctx = new PrincipalContext(ContextType.Domain);
        // find currently logged in user
        UserPrincipal user = UserPrincipal.Current;
        strUsername = user.DisplayName;
        UserPrincipal user2 = UserPrincipal.FindByIdentity(ctx, strUsername);

        //Find Admin group
        GroupPrincipal Admingroup = GroupPrincipal.FindByIdentity(ctx, strAdminGroup);
        GroupPrincipal Approvalgroup = GroupPrincipal.FindByIdentity(ctx, strApprovalGroup);

        if (user2 != null)
        {
            if (user2.IsMemberOf(Admingroup))
            {
                strIsInAdminGroupFlag = "1";
            }
            else
            {
                strIsInAdminGroupFlag = "0";
            }
            if (user2.IsMemberOf(Approvalgroup))
            {
                strIsInApprovalGroupFlag = "1";
            }
            else
            {
                strIsInApprovalGroupFlag = "0";
            }
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


        return strAccessType;

    }
}
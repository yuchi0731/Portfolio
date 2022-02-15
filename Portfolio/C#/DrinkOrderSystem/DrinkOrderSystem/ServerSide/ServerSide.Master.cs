using DOS_Auth;
using DOS_DBSoure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace DrinkOrderSystem.ServerSide
{
    public partial class ServerSide : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!AuthManager.IsLogined())
            {
                Response.Redirect("/ClientSide/Login.aspx");
                return;
            }

            var currentuser = AuthManager.GetCurrentUser();
            if (currentuser.JobGrade != 0)
                ltlManger.Visible = true;

            if(currentuser.Account == null)
            {
                Response.Redirect("/ClientSide/Login.aspx");
                return;
            }

            var orderNumber = DrinkListManager.GetUserLastOrderNumber(currentuser.Account);

            var userlevel = "";
            if (currentuser.JobGrade == 0)
                userlevel = "一般使用者";
            else if (currentuser.JobGrade == 1)
                userlevel = "管理者";
            else if (currentuser.JobGrade == 2)
                userlevel = "高階管理者";

            var runMsg = "";
            if(orderNumber == null)
                runMsg = $"～歡迎～{currentuser.FirstName}！ 您目前使用等級為是【{userlevel}】，最近下的訂單是【尚未有訂單】";
            else if(orderNumber != null)
                runMsg = $"～歡迎～{currentuser.FirstName}！ 您目前使用等級為是【{userlevel}】，最近下的訂單是【{orderNumber}】";
            else
                runMsg = $"～歡迎～{currentuser.FirstName}！ 您目前使用等級為是【{userlevel}】，最近下的訂單是【尚未有訂單】";

            string text = "<MARQUEE>" + runMsg + "</MARQUEE>";
            lbTopMsg.Text = text;
        }

        protected void btnLogut_Click(object sender, EventArgs e)
        {
            AuthManager.Logout();
            Response.Write("<script language='JavaScript'>alert('登出成功') { location.href='/ClientSide/Login.aspx'; } </script>");
        }
    }
}
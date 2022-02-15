using DOS_Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DrinkOrderSystem.ClientSide
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write("<Script language='JavaScript'>alert('歡迎使用員工飲料訂購系統');</Script>");
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            this.lbMsg.Visible = false;
            string inp_Account = this.txtAccount.Text;
            string inp_PWD = this.txtPWD.Text;

            string msg;

            if (!AuthManager.TryLogin(inp_Account, inp_PWD, out msg))
            {
                this.lbMsg.Visible = true;
                this.lbMsg.Text = msg;
                return;
            }

            else
                Response.Redirect("/ServerSide/SystemAdmin/UserPage.aspx");
        }
    }
}
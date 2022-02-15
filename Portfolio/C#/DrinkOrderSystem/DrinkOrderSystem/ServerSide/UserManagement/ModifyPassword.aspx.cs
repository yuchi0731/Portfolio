using DOS_Auth;
using DOS_DBSoure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace DrinkOrderSystem.ServerSide.UserManagement
{
    public partial class ModifyPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!AuthManager.IsLogined())
            {
                Response.Redirect("/ClientSide/Login.aspx");
                return;
            }

            var current = AuthManager.GetCurrentUser();
            string hasaccount = this.Request.QueryString["Account"];

            if (hasaccount != null)
            {
                if (current.JobGrade != 0)
                {
                    string account = this.Request.QueryString["Account"];
                    var useraccount = UserInfoManager.GetUserAccount(account);

                    this.ltUserAccount.Text = useraccount.Account as string;
                }
                else
                {
                    var userAccount = UserInfoManager.GetUserAccount(current.Account);
                    var pwd = userAccount.Password;
                    this.ltUserAccount.Text = userAccount.Account as string;
                }
            }
            else
            {
                var userAccount = UserInfoManager.GetUserAccount(current.Account);
                var pwd = userAccount.Password;
                this.ltUserAccount.Text = userAccount.Account as string;
            }
        }

        protected void btnChange_Click(object sender, EventArgs e)
        {
            var PWD = txtPWD.Text;
            var newPWD = txtNewPWD.Text;
            var RenewPWD = txtReNewPWD.Text;
            var newPWDL = newPWD.Length;
            var RenewPWDL = RenewPWD.Length;
            if (string.IsNullOrWhiteSpace(PWD) || string.IsNullOrWhiteSpace(newPWD) || string.IsNullOrWhiteSpace(RenewPWD))
            {
                this.lbMsg.Visible = true;
                this.lbMsg.Text = "密碼選項為必填";
                this.txtPWD = null;
                this.txtNewPWD = null;
                this.txtReNewPWD = null;
            }

            //檢查密碼長度
            if (newPWDL < 8 || RenewPWDL > 16)
            {
                this.lbMsg.Visible = true;
                this.lbMsg.Text = "密碼需介於8～16個字";
                return;
            }

            var current = AuthManager.GetCurrentUser();
            string hasaccount = this.Request.QueryString["Account"];

            if (hasaccount != null)
            {
                //取得從UserList選到的帳號
                string account = this.Request.QueryString["Account"];
                var useraccount = UserInfoManager.GetUserAccount(account);
                var pwd = useraccount.Password;

                if (string.Compare(pwd, PWD, false) == 0)
                {
                    if (string.Compare(newPWD, RenewPWD, false) == 0)
                    {

                        //判斷當前使用者是否為普通使用者
                        if (current.JobGrade != 0)
                        {
                            UserInfoManager.UpdateUserPWDlinq(account, newPWD);
                            Response.Write(
                                    $"<script>alert('變更成功，將導回使用者清單頁');location.href='/ServerSide/UserManagement/UserList.aspx';</script>");
                        }
                        else
                        {
                            UserInfoManager.UpdateUserPWDlinq(account, newPWD);
                            Response.Write(
                                    $"<script>alert('變更成功');location.href='/ServerSide/UserManagement/UserList.aspx';</script>");
                        }
                    }
                    else
                    {
                        this.lbMsg.Visible = true;
                        this.lbMsg.Text = "新密碼與確認密碼不符，請重新輸入";
                        this.txtNewPWD = null;
                        this.txtReNewPWD = null;
                    }
                }
                else
                {
                    this.lbMsg.Text = "密碼輸入錯誤，請確認密碼";
                    this.txtPWD = null;
                    this.txtNewPWD = null;
                    this.txtReNewPWD = null;
                }
            }
            else
            {
                var userAccount = UserInfoManager.GetUserAccount(current.Account);

                if (string.Compare(userAccount.Password, PWD, false) == 0)
                {

                    if (string.Compare(newPWD, RenewPWD, false) == 0)
                    {
                        UserInfoManager.UpdateUserPWDlinq(current.Account, newPWD);
                        AuthManager.Logout();
                        Response.Write(
                                $"<script>alert('變更成功，請重新登入');location.href='/ClientSide/Login.aspx';</script>");
                    }
                    else
                    {
                        lbMsg.Visible = true;
                        lbMsg.Text = "新密碼與確認密碼不符，請重新輸入";
                        txtNewPWD = null;
                        txtReNewPWD = null;
                    }
                }
                else
                {
                    this.lbMsg.Text = "密碼輸入錯誤，請確認密碼";
                    this.txtPWD = null;
                    this.txtNewPWD = null;
                    this.txtReNewPWD = null;
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("/ServerSide/UserManagement/UserList.aspx");
        }
    }
}
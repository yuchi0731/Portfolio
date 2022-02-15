using DOS_Auth;
using DOS_DBSoure;
using DOS_ORM.DOSmodel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace DrinkOrderSystem.ServerSide.SystemAdmin
{
    public partial class SendOrder : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack) //可能是按按鈕跳回本頁，所以要判斷Postback
            {
                if (!AuthManager.IsLogined())
                {
                    Response.Redirect("/ClientSide/Login.aspx");
                    return;
                }

                string orderNumber = this.Request.QueryString["OrderNumber"];
                var orderDetail = DrinkListManager.GetOrderDetailInfo(orderNumber);

                //判斷訂單結帳時間，若預計送達時間是三十分鐘內則取消訂單
                if (orderDetail.Established == "Fail" || orderDetail.Established == "Complete")
                {
                    Response.Write(
                            $"<script>alert('此訂單已完成或超過結帳時間');location.href='/ServerSide/SystemAdmin/OrderRecords.aspx';</script>");
                }

                var list = DrinkListManager.GetOrderListbyorderNumber(orderNumber);

                if (list.Count > 0) //check is empty data (大於0就做資料繫結)
                {
                    this.gvOrderList.DataSource = list;
                    this.gvOrderList.DataBind();
                }
                else
                {
                    this.gvOrderList.Visible = false;
                    this.btnExport.Visible = false;
                    this.btnCancel.Visible = false;
                    this.lbMsg.Visible = true;
                    this.lbMsg.Text = "找不到訂單資料";
                }
            }
        }

        /// <summary>
        /// 將GridView的資料輸出到Excel檔案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExport_Click(object sender, EventArgs e)
        {
            string orderNumber = this.Request.QueryString["OrderNumber"];
            var orderDetail = DrinkListManager.GetOrderDetailInfo(orderNumber);
            if (orderDetail.RequiredTime < DateTime.Now.AddMinutes(30))
            {
                Response.Write(
                        $"<script>alert('此訂單已超過結帳時間');location.href='/ServerSide/SystemAdmin/OrderRecords.aspx';</script>");
            }
            else
            {
                bool sendmail = sendGmail();
                if (sendmail == true)
                {
                    //更改訂單成立狀況
                    DrinkListManager.UpdateEstablished(orderNumber);
                    this.Session.Remove("OrderNumber");
                    Response.Write(
                            $"<script>alert('訂購成功，已有寄系統確認信至您的信箱，請確認');location.href='/ServerSide/SystemAdmin/OrderRecords.aspx';</script>");
                }
                else
                {
                    this.Session.Remove("OrderNumber");
                    Response.Write(
                            $"<script>alert('訂購失敗，請檢查連線是否成功');location.href='/ServerSide/SystemAdmin/OrderRecords.aspx';</script>");
                }
            }
        }
        internal static void WriteText(string orderNumber, string txt)
        {
            string dirPath = @"D:\Text";
            string path = $@"D:\Text\{orderNumber}List.txt";
            if (Directory.Exists(dirPath))
            {
                //須先建立資料夾
                if (!File.Exists(path))
                {
                    string text = $"訂單編號：{orderNumber}" + "\r\n" + txt;
                    File.WriteAllText(path, text, Encoding.UTF8);
                }
            }
        }

        protected void btnViewDetail_Click(object sender, EventArgs e)
        {
            this.txtCheck.Visible = true;
            this.btnText.Visible = true;
            this.btnCancel.Visible = true;
            this.btnExport.Visible = true;
            string orderNumber = this.Request.QueryString["OrderNumber"];
            var allDetail = DrinkListManager.GetOrderDetailListbyorderNumber(orderNumber);

            string DetailInfo = "";
            foreach (var item in allDetail)
            {
                DetailInfo +=
                    "【訂購人】" + item.Account.ToString()
                    + Environment.NewLine
                    + "【飲料】" + item.ProductName.ToString()
                    + Environment.NewLine
                    + "【單價】" + item.UnitPrice.ToString()
                    + Environment.NewLine
                    + "【杯數】" + item.Quantity.ToString()
                    + Environment.NewLine
                    + "【甜度】" + item.Suger.ToString()
                    + Environment.NewLine
                    + "【冰量】" + item.Ice.ToString()
                    + Environment.NewLine
                    + "【加料】" + item.Toppings.ToString()
                    + Environment.NewLine
                    + "【加料單價】" + item.ToppingsUnitPrice.ToString()
                    + Environment.NewLine
                    + "-----------------------------------"
                    + Environment.NewLine;
            }
            this.txtCheck.Text = DetailInfo;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("/ServerSide/SystemAdmin/OrderRecords.aspx");
        }

        public bool sendGmail()
        {
            string orderNumber = this.Request.QueryString["OrderNumber"];
            var allDetail = DrinkListManager.GetOrderDetailListbyorderNumber(orderNumber);

            var DetailInfo = "";
            foreach (var item in allDetail)
            {
                DetailInfo +=
                    "【訂購人】" + item.Account.ToString()
                    + "\n"
                    + "【飲料】" + item.ProductName.ToString()
                    + "\n"
                    + "【單價】" + item.UnitPrice.ToString()
                    + "\n"
                    + "【杯數】" + item.Quantity.ToString()
                    + "\n"
                    + "【甜度】" + item.Suger.ToString()
                    + "\n"
                    + "【冰量】" + item.Ice.ToString()
                    + "\n"
                    + "【加料】" + item.Toppings.ToString()
                    + "\n"
                    + "【加料單價】" + item.ToppingsUnitPrice.ToString()
                    + "\n"
                    + "-----------------------------------"
                    + "\n";
            }

            var reqtime = DrinkListManager.GetOrderDetailListfromorderNumber(orderNumber).RequiredTime.ToString("MM-dd HH:mm");
            var currentUser = AuthManager.GetCurrentUser();
            var userData = UserInfoManager.GetUserInfo(currentUser.Account);
            var userEmail = userData.Email.ToString();
            var mailAddress = "fuchiharayuchi@gmail.com";

            var adminEmail = "DrinkOrderServer@gmail.com";
            var ademin = "管理者";

            var title = $"{currentUser.FirstName}您好，訂購編號：{orderNumber} 已訂購完成！";
            var message =
                    $@"<h1>～您好，您所開團的團購已訂購完成～</h1>
                    <p> 訂單明細為：</ br > {DetailInfo}  </p>
                    </br>
                    <p> 目前已送出訂單給予廠商，待廠商送達 </p>
                    </br>
                    <p> 請依照送達時間 {reqtime} 至公司門口領取，謝謝 </p>
                    </br>
                    <p><a href='https://www.ubay.co.jp/tw/index.html'>UBay website</a> SMTP port</p>
                    </br>
                    <p> 歡迎下次訂購~ </p>";

            try
            {
                System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
                msg.To.Add(mailAddress);
                //msg.To.Add("b@b.com");
                //msg.CC.Add("c@c.com");
                //msg.BCC.Add("c@c.com");

                msg.From = new MailAddress(adminEmail, ademin, System.Text.Encoding.UTF8);
                /* 上面3個參數分別是發件人地址（可以隨便寫），發件人姓名，編碼*/
                msg.Subject = title;//郵件標題
                msg.SubjectEncoding = System.Text.Encoding.UTF8;//郵件標題編碼
                msg.Body = message; //郵件內容
                msg.BodyEncoding = System.Text.Encoding.UTF8;//郵件內容編碼 
                //msg.Attachments.Add(new Attachment(@"D:\Text.docx"));  //附件
                msg.IsBodyHtml = true;//是否是HTML郵件 
                msg.Priority = MailPriority.Normal;//郵件優先級 

                SmtpClient client = new SmtpClient();
                client.Credentials = new System.Net.NetworkCredential("fuchiharayuchi", "kogdvbltanvzdmgy"); //這裡要填正確的帳號跟密碼
                client.Host = "smtp.gmail.com"; //設定smtp Server
                client.Port = 25; //設定Port
                client.EnableSsl = true; //gmail預設開啟驗證
                client.Send(msg); //寄出信件
                client.Dispose();
                msg.Dispose();

                return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Message);
                return false;
            }
        }

        protected void btnText_Click(object sender, EventArgs e)
        {
            string orderNumber = this.Request.QueryString["OrderNumber"];
            var allDetail = DrinkListManager.GetOrderDetailListbyorderNumber(orderNumber);

            var DetailInfo = "";
            foreach (var item in allDetail)
            {
                DetailInfo +=
                    "【訂購人】" + item.Account.ToString()
                    + Environment.NewLine
                    + "【飲料】" + item.ProductName.ToString()
                    + Environment.NewLine
                    + "【單價】" + item.UnitPrice.ToString()
                    + Environment.NewLine
                    + "【杯數】" + item.Quantity.ToString()
                    + Environment.NewLine
                    + "【甜度】" + item.Suger.ToString()
                    + Environment.NewLine
                    + "【冰量】" + item.Ice.ToString()
                    + Environment.NewLine
                    + "【加料】" + item.Toppings.ToString()
                    + Environment.NewLine
                    + "【加料單價】" + item.ToppingsUnitPrice.ToString()
                    + Environment.NewLine
                    + "-----------------------------------"
                    + Environment.NewLine;
            }
            WriteText(orderNumber, DetailInfo);
        }
    }
}
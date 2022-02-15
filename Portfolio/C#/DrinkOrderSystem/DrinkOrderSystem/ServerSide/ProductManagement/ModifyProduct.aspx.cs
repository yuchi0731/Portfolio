using DOS_Auth;
using DOS_DBSoure;
using DOS_ORM.DOSmodel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace DrinkOrderSystem.ServerSide.ProductManagement
{
    public partial class ModifyProduct : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if (!AuthManager.IsLogined())
                {
                    Response.Redirect("/ClientSide/Login.aspx");
                    return;
                }

                string productID = this.Request.QueryString["ProductID"];
                var prodData = ProductManager.GetProductInfoByID(Convert.ToInt32(productID));

                this.txtProduct.Text = prodData.ProductName.ToString();
                this.txtUnitPrice.Text = prodData.UnitPrice.ToString();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string productID = this.Request.QueryString["ProductID"];
            var prodData = ProductManager.GetProductInfoByID(Convert.ToInt32(productID));

            var pdName = this.txtProduct.Text;
            var txtunitPrice = this.txtUnitPrice.Text;

            if (string.IsNullOrWhiteSpace(pdName))
            {
                Response.Write($"<script>alert('未填商品名稱');</script>");
                this.lbMsg.Text = "未填商品名稱";
                return;
            }
            else if (string.IsNullOrWhiteSpace(txtunitPrice))
            {
                Response.Write($"<script>alert('未填價格');</script>");
                this.lbMsg.Text = "未填價格";
                return;
            }
            else
            {
                int pID = 0;
                int.TryParse(productID, out pID);

                Product product = new Product()
                {
                    ProductID = pID,
                    ProductName = pdName,
                    Picture = prodData.Picture
                };

                decimal unitPrice;
                if (decimal.TryParse(txtunitPrice, out unitPrice))
                {
                    product.UnitPrice = unitPrice;
                }

                //假設有上傳檔案，就寫入檔名
                if (this.fdPictrue.HasFile && FileUploadManager.VaildFileUpload(this.fdPictrue, out List<string> tempList))
                {
                    string saveFileName = FileUploadManager.GetNewFileName(this.fdPictrue);
                    string filePath = Path.Combine(this.GetSaveFolderPath(), saveFileName);
                    this.fdPictrue.SaveAs(filePath);

                    product.Picture = saveFileName;
                }
                ProductManager.UpdateProduct(product);

                Response.Write(
                    $"<script>alert('～修改成功～');location.href='/ServerSide/ProductManagement/ProductList.aspx';</script>");
            }
        }

        private string GetSaveFolderPath()
        {
            return Server.MapPath("~/ServerSide/ImagesServer");
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            this.txtProduct.Text = "";
            this.txtUnitPrice.Text = "";
            this.fdPictrue = null;
        }
    }
}
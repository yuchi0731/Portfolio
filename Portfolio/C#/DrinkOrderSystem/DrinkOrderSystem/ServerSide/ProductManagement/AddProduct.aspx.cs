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
    public partial class AddProduct : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!AuthManager.IsLogined())
                {
                    Response.Redirect("/ClientSide/Login.aspx");
                    return;
                }

                var current = AuthManager.GetCurrentUser();
                var jobGrade = current.JobGrade;
                if (jobGrade < 1)
                {
                    Response.Redirect("/ServerSide/SystemAdmin/UserPage.aspx");
                    return;
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var pdName = this.txtNewProduct.Text;
            var supName = this.ddSupplier.SelectedValue.ToString();
            var txtunitPrice = this.txtUnitPrice.Text;
            var category = this.ddCategory.SelectedValue.ToString();
            if (string.IsNullOrWhiteSpace(pdName))
            {
                Response.Write($"<script>alert('未填商品名稱');</script>");
                this.lbMsg.Text = "未填商品名稱";
            }
            else if (supName == "non")
            {
                Response.Write($"<script>alert('店家尚未選取');</script>");
                this.lbMsg.Text = "店家尚未選取";
            }
            else if (string.IsNullOrWhiteSpace(txtunitPrice))
            {
                Response.Write($"<script>alert('未填價格');</script>");
                this.lbMsg.Text = "未填價格";
            }
            else if (category == "non")
            {
                Response.Write($"<script>alert('種類尚未選取');</script>");
                this.lbMsg.Text = "種類尚未選取";
            }
            else
            {
                Product product = new Product()
                {
                    ProductName = pdName,
                    SupplierName = supName,
                    CategoryName = category
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

                ProductManager.CreateNewProduct(product);

                Response.Write(
                    $"<script>alert('～建立成功～');location.href='/ServerSide/ProductManagement/ProductList.aspx';</script>");
            }
        }

        private string GetSaveFolderPath()
        {
            return Server.MapPath("~/ServerSide/ImagesServer");
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            this.txtNewProduct.Text = null;
            this.ddSupplier.SelectedIndex = 0;
            this.txtUnitPrice.Text = null;
            this.ddCategory.SelectedIndex = 0;
        }
    }
}
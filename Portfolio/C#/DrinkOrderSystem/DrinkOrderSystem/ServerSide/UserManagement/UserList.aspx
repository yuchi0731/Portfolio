<%@ Page Title="" Language="C#" MasterPageFile="~/ServerSide/ServerSide.Master" AutoEventWireup="true" CodeBehind="UserList.aspx.cs" Inherits="DrinkOrderSystem.ServerSide.UserManagement.UserList" %>

<%@ Register Src="~/ServerSide/UserControl/ucPager.ascx" TagPrefix="uc1" TagName="ucPager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>所有使用者資料</h1>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <asp:Panel ID="pnlbase" runat="server" Style="background-color: ghostwhite" ScrollBars="Auto">
        <div>
            <asp:Button ID="btnCreate" runat="server" Text="新增使用者" OnClick="btnCreate_Click" />
            <asp:Button ID="btnSortingN" runat="server" Text="以修改時間近至遠排序" OnClick="btnSortingN_Click" />
            <asp:Button ID="btnSortingF" runat="server" Text="以修改時間遠至近排序" OnClick="btnSortingF_Click" />
            <asp:GridView ID="gvUserlist" runat="server" AutoGenerateColumns="False" HorizontalAlign="Center" OnRowDataBound="gvUserlist_RowDataBound" OnRowCommand="gvUserlist_RowCommand">
                <Columns>
                    <asp:BoundField DataField="Account" HeaderText="帳號" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="EmployeeID" HeaderText="員工編號" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="DepartmentID" HeaderText="部門代號" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="Department" HeaderText="部門" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="LastName" HeaderText="姓氏" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="FirstName" HeaderText="名字" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="Contact" HeaderText="聯絡方式" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="Email" HeaderText="Email" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="ext" HeaderText="分機號碼" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="Phone" HeaderText="聯絡電話" ItemStyle-HorizontalAlign="Center" />

                    <asp:TemplateField HeaderText="使用者等級" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbluserlevel"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="CreateDate" DataFormatString="{0:yyyy-MM-dd}" HeaderText="建日期" ItemStyle-HorizontalAlign="Center" />

                    <asp:BoundField DataField="LastModified" DataFormatString="{0:yyyy-MM-dd}" HeaderText="最後修改日期" ItemStyle-HorizontalAlign="Center" />

                    <asp:TemplateField HeaderText="個人照片">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgPhoto" runat="server" Visible="false" Width="35" Height="45" CommandName="btnLarger" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="修改資料" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <a href="/ServerSide/UserManagement/ModifyUserInfo.aspx?EmployeeID=<%# Eval("EmployeeID") %>">修改</a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="變更密碼" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <a href="/ServerSide/UserManagement/ModifyPassword.aspx?Account=<%# Eval("Account") %>">修改</a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="刪除使用者" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Button ID="btnDelete" runat="server" Text="刪除" onclientclick="javascript:return confirm('若刪除使用者將無法復原，繼續請按確定');" CommandName="btndeleteUser" CommandArgument='<%#Eval("Account") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>

            <uc1:ucPager runat="server" ID="ucPager" PageSize="10" Url="/ServerSide/UserManagement/UserList.aspx" />
            <br />
            <asp:Label ID="lblselect" runat="server" Text="以下方式查詢"></asp:Label>
            <br />
            <asp:DropDownList ID="ddsearch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddsearch_SelectedIndexChanged">
                <asp:ListItem Value="non">未選擇</asp:ListItem>
                <asp:ListItem Value="account">帳號</asp:ListItem>
                <asp:ListItem Value="eid">員工編號</asp:ListItem>
                <asp:ListItem Value="lastname">姓氏</asp:ListItem>
                <asp:ListItem Value="firstname">名字</asp:ListItem>
                <asp:ListItem Value="departmentID">部門代號</asp:ListItem>
            </asp:DropDownList>

            <asp:TextBox ID="txtSearch" runat="server" Visible="false"></asp:TextBox>
            <asp:Button ID="btnSearch" runat="server" Text="查詢" Visible="false" OnClick="btnSearch_Click" />
            <asp:Button ID="btnSearchClear" runat="server" Text="清除查詢" Visible="false" OnClick="btnSearchClear_Click" /><br />
            <asp:PlaceHolder ID="plcNoData" runat="server" Visible="false">
                <asp:Label ID="lbMsg" runat="server" ForeColor="Red"></asp:Label>
            </asp:PlaceHolder>
        </div>
    </asp:Panel>
</asp:Content>

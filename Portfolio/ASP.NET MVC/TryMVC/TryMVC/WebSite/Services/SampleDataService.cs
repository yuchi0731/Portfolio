using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TryMVC.Models;

namespace TryMVC.WebSite.Services
{
    public class SampleDataService
    {
        private static List<UserInfoModels> _userinfolist { get; set; }
        private static List<ProductModels> _productinfolist { get; set; }

        /// <summary>
        /// 查詢使用者清單
        /// </summary>
        /// <returns></returns>
        public static List<UserInfoModels> GetUserList()
        {
            var query = from item in CreateUserData()
                        where(item.DeleteUser == null && item.DeleteDate == null)
                        select item;

            var result = query.ToList();
            return result;
        }

        /// <summary>
        /// 查詢單筆使用者
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public static UserInfoModels GetUserInfo(string account)
        {
            var list = CreateUserData();
            var query = list.Where(obj => obj.Account == account).FirstOrDefault();

            return query;
        }

        /// <summary>
        /// 建立使用者
        /// </summary>
        /// <param name="models"></param>
        /// <param name="userID"></param>
        /// <param name="moditime"></param>
        public static void CreateUser(UserInfoModels models)
        {
            var ctime = DateTime.Now;
            var checkmodel = GetUserInfo(models.Account);
            if (checkmodel != null)
                throw new Exception("帳號: " + models.Account + "已存在");

            else
            {
                UserInfoModels item = new UserInfoModels();
                item.Account = models.Account;
                item.pwd = models.pwd;
                item.Name = models.Name;
                item.Email = models.Email;
                item.Birthday = models.Birthday;
                item.IsEnable = true;
                item.UserID = Guid.NewGuid();
                item.CreateDate = DateTime.Now;
                item.ModifyDate = ctime;

                List<UserInfoModels> newlist = CreateUserData();
                newlist.Add(item);
            }
        }

        /// <summary>
        /// 修改使用者資料
        /// </summary>
        /// <param name="models"></param>
        /// <param name="userID"></param>
        /// <param name="moditime"></param>
        public static void ModifyUserInfo(UserInfoModels models)
        {
            var ctime = DateTime.Now;
            var checkmodel = GetUserInfo(models.Account);

            if(checkmodel == null)
                throw new Exception("帳號: " + models.Account + " 不存在");

            else
            {
                UserInfoModels modimodel = GetUserInfo(models.Account);
                modimodel.Name = models.Name;
                modimodel.pwd = models.pwd;
                modimodel.Email = models.Email;
                modimodel.Birthday = models.Birthday;
                modimodel.IsEnable = true;
                modimodel.ModifyUser = models.UserID;
                modimodel.ModifyDate = ctime;

            }
        }

        /// <summary>
        /// 修改使用者密碼
        /// </summary>
        /// <param name="models"></param>
        /// <param name="userID"></param>
        /// <param name="moditime"></param>
        public static void ModifyUserPWD(UserInfoModels models)
        {
            var ctime = DateTime.Now;
            var item = GetUserInfo(models.Account);

            if(item == null)
                throw new Exception("帳號: " + models.Account + " 不存在");

            item.pwd = models.Name;
            item.ModifyDate = ctime;
        }

        /// <summary>
        /// 刪除使用者
        /// </summary>
        /// <param name="acc"></param>
        /// <param name="userID"></param>
        /// <param name="moditime"></param>
        public static void DeleteUser(string acc)
        {
            var ctime = DateTime.Now;
            var checkmodel = GetUserInfo(acc);

            if (checkmodel.DeleteUser != null)
                throw new Exception("帳號: " + acc + " 不存在");
            else
            {
                var model = GetUserInfo(acc);
                model.ModifyDate = ctime;
                model.DeleteUser = model.UserID;
                model.DeleteDate = ctime;
            }
        }

        /// <summary>
        /// 建立假資料(Fake data)
        /// </summary>
        /// <returns></returns>
        private static List<UserInfoModels> CreateUserData()
        {
            if(_userinfolist == null)
            {
                var list = new List<UserInfoModels>();

                for(int i = 1; i <=50; i++)
                {
                    list.Add(new UserInfoModels()
                    {
                        UserID = Guid.NewGuid(),
                        Account = "Account_" + i,
                        Name = "Name_" + i,
                        Email = i + "@gmai.com",
                        pwd = i.ToString(),
                        Birthday = DateTime.Now.ToString("yyyy/MM/dd"),
                    });
                }
                _userinfolist = list;

            }
            return _userinfolist;
        }

        private static List<ProductModels> CreateProductData()
        {
            if (_productinfolist == null)
            {
                var list = new List<ProductModels>();

                for (int i = 1; i <= 50; i++)
                {
                    list.Add(new ProductModels()
                    {
                        pId = "pId_" + i,
                        pName = "Name_" + i,
                        Price = i * 10,

                    });
                }
                _productinfolist = list;
            }
            return _productinfolist;
        }

        private static List<UserInfoModels> GetUserData()
        {
            if (_userinfolist == null)
            {
                var list = new List<UserInfoModels>();

                for (int i = 1; i <= 50; i++)
                {
                    list.Add(new UserInfoModels()
                    {
                        UserID = Guid.NewGuid(),
                        Account = "Account_" + i,
                        Name = "Name_" + i,
                        Email = i + "Email",
                        pwd = i.ToString(),
                        Birthday = DateTime.Now.ToShortDateString(),
                    });
                }
                _userinfolist = list;
            }
            return _userinfolist;
        }

    }
}
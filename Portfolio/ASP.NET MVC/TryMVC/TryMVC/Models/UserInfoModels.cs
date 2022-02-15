using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TryMVC.WebSite.Services;

namespace TryMVC.Models
{
    public class UserInfoModels
    {
        public Guid UserID { get; set; }
        public string Account { get; set; }
        public string Name { get; set; }
        public string pwd { get; set; }
        public string Email { get; set; }
        public string Birthday { get; set; }

        #region 管理
        /// <summary> 是否啟用 </summary>
        public bool IsEnable { get; set; }
        /// <summary> 建立者帳號代碼 </summary>
        public Guid CreateUser { get; set; }
        /// <summary> 建立時間 </summary>
        public DateTime CreateDate { get; set; }
        /// <summary> 修改者帳號代碼 </summary>
        public Guid? ModifyUser { get; set; }
        /// <summary> 修改時間 </summary>
        public DateTime? ModifyDate { get; set; }
        /// <summary> 刪除者帳號代碼 </summary>
        public Guid? DeleteUser { get; set; }
        /// <summary> 刪除時間 </summary>
        public DateTime? DeleteDate { get; set; }
        #endregion

        public static UserInfoModels GetDefault()
        {
            return new UserInfoModels()
            {
                Account = string.Empty,
                Name = string.Empty,
                pwd = string.Empty,
                Email = string.Empty,
                Birthday = DateTime.Now.ToShortDateString()
            };
        }

        public static UserInfoModels GetModifyDefault(string account)
        {
            UserInfoModels model = SampleDataService.GetUserInfo(account);

            return model;
        }
    }
}
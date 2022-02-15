using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TryMVC.Models;
using TryMVC.WebSite.Services;

namespace TryMVC.Controllers
{
    public class UserServiceController : Controller
    {
        // GET: UserService
        public ActionResult Index()
        {
            var list = SampleDataService.GetUserList();
            return View(list);
        }

        // GET: UserService/Create
        public ActionResult Create()
        {
            var obj = UserInfoModels.GetDefault();
            return View("Create", obj);
        }
 
        // Post: UserService/Create
        [HttpPost]
        public ActionResult Create(UserInfoModels model)
        {
            var newmodel = new UserInfoModels();
            newmodel.Account = model.Account;
            newmodel.Name = model.Name;
            newmodel.pwd = model.pwd;
            newmodel.Email = model.Email;
            newmodel.Birthday = model.Birthday;
            SampleDataService.CreateUser(newmodel);

            return RedirectToAction("Index");
        }

        // GET: UserService/Edit
        public ActionResult Edit(UserInfoModels model)
        {
            var viewModel = SampleDataService.GetUserInfo(model.Account);
            return View("Edit", viewModel);
        }

        // Post: UserService/Edit
        [HttpPost]
        public ActionResult Edit(UserInfoModels model, string account)
        {
            var orgModel = SampleDataService.GetUserInfo(model.Account);

            var newmodel = new UserInfoModels();
            newmodel.Account = orgModel.Account;
            newmodel.Name = model.Name;
            newmodel.pwd = model.pwd;
            newmodel.Email = model.Email;
            newmodel.Birthday = orgModel.Birthday;
            SampleDataService.ModifyUserInfo(newmodel);

            return RedirectToAction("Index");
        }

        // Post: UserService/Delete
        [HttpPost]
        public ActionResult Delete(UserInfoModels model)
        {
            SampleDataService.DeleteUser(model.Account);

            return RedirectToAction("Index");
        }
    }
}
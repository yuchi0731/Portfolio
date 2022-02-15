using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using prjView.Models;

namespace prjView.Controllers
{
    public class BootstrapSampleController : Controller
    {
        // GET: BootstrapSample
        public ActionResult Index()
        {
            string[] id = new string[] { "A01", "A02", "A03", "A04", "A05", "A06", "A07" };
            string[] name = new string[]
                            { "逢甲夜市",
                              "一中街商圈",
                              "中華路夜市",
                              "忠孝路夜市",
                              "豐原廟東夜市",
                              "東海夜市",
                              "霧峰樹仁商圈夜市" };

            string[] address = new string[]
                          { "407台中市西屯區文華路",
                            "404台中市北區一中街",
                            "400台中市中區公園路",
                            "402台中市南區忠孝路",
                            "420台中市豐原區中正路167巷",
                            "433台中市龍井區新興路",
                            "413台中市霧峰區樹仁路" };
            List<NightMarket> list = new List<NightMarket>();
            for (var i = 0; i < id.Length; i++)
            {
                NightMarket nightmarket = new NightMarket();
                nightmarket.Id = id[i];
                nightmarket.Name = name[i];
                nightmarket.Address = address[i];
                list.Add(nightmarket);
            }
            return View(list);
        }
    }
}
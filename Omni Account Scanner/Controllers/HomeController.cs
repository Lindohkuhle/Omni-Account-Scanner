using Omni_Account_Scanner.Models;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Net.Http.Handlers;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace Omni_Account_Scanner.Controllers
{
    public class HomeController : Controller
    {
        String baseUrl = "http://st.omniaccounts.co.za:55683/";
        public async Task<ActionResult> Index(string search)
        {
            ViewBag.EmptyMessageHolder = string.Empty;

            Product prod = new Product();

            if(search != null)
            {
                using (var item = new HttpClient())
                {
                    item.BaseAddress = new Uri(baseUrl);
                    item.DefaultRequestHeaders.Clear();
                    item.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage resp = await item.GetAsync("Report/Stock Export?CompanyName=SA Example Company [Demo]&UserName=Guest&password=Dev2021&IBarCode=" + search);
                    if (resp.IsSuccessStatusCode)
                    {
                        String ProdResponse = resp.Content.ReadAsStringAsync().Result.Replace("\\", "").Trim(new char[1] { '"' });

                        prod = JsonConvert.DeserializeObject<Product>(ProdResponse);
                    }

                }

                var response = prod.stock_export.FirstOrDefault();
                if (response == null)
                {
                    ViewBag.EmptyMessageHolder = "Product could not be found";
                }
                return View(response);
            }
            else
            {
                
                ViewBag.EmptyMessageHolder = "No Product";
                
                return View();
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
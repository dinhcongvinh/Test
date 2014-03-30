using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace NM.Controllers
{
    public class InformationController : Controller
    {
        //
        // GET: /Information/
        DataNMDataContext _dbProject = new DataNMDataContext();

        public ActionResult Index()
        {
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {

                    return View(_dbProject.Imformations.Single(item=>item.IDInformation==1));
                }
                else
                {

                    return RedirectToAction("Login", "Admin");
                }
            }
            catch
            {
                return RedirectToAction("Login", "Admin");
            }      
        }        

        public ActionResult Lienhevoichungtoi()
        {
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {

                    return View(_dbProject.Imformations.Single(item=>item.IDInformation==2));
                }
                else
                {

                    return RedirectToAction("Login", "Admin");
                }
            }
            catch
            {
                return RedirectToAction("Login", "Admin");
            }      
        }

        public ActionResult WebsiteEcommerce()
        {
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {

                    return View(_dbProject.Imformations.Single(item=>item.IDInformation==3));
                }
                else
                {

                    return RedirectToAction("Login", "Admin");
                }
            }
            catch
            {
                return RedirectToAction("Login", "Admin");
            }      
        }   

        [HttpPost, ValidateInput(false)]
        public ActionResult EditLienhevoichungtoi()
        {
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {
                    _dbProject.Imformations.Single(item => item.IDInformation == 2).DetailInformation = Request.Form["detailInformation"];
                    _dbProject.SubmitChanges();

                    return RedirectToAction("Lienhevoichungtoi", "Information");
                }
                else
                {

                    return RedirectToAction("Login", "Admin");
                }
            }
            catch
            {
                return RedirectToAction("Login", "Admin");
            }     
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EditWebsiteEcommerce()
        {
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {
                    _dbProject.Imformations.Single(item => item.IDInformation == 3).DetailInformation = Request.Form["detailInformation"];
                    _dbProject.SubmitChanges();

                    return RedirectToAction("WebsiteEcommerce", "Information");
                }
                else
                {

                    return RedirectToAction("Login", "Admin");
                }
            }
            catch
            {
                return RedirectToAction("Login", "Admin");
            }
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EditContact()
        {
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {
                    _dbProject.Imformations.Single(item => item.IDInformation == 1).DetailInformation = Request.Form["detailInformation"];
                    _dbProject.SubmitChanges();

                    return RedirectToAction("Index", "Information");
                }
                else
                {

                    return RedirectToAction("Login", "Admin");
                }
            }
            catch
            {
                return RedirectToAction("Login", "Admin");
            }
        }

        public ActionResult SettingGeneral()
        {
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {
                    return View(_dbProject.SetingSystems.FirstOrDefault(p=>p.ID==1));
                }
                else
                {

                    return RedirectToAction("Login", "Admin");
                }
            }
            catch
            {
                return RedirectToAction("Login", "Admin");
            }
        }

        public ActionResult UpdatePageSize()
        {
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {
                    SetingSystem itemResult = _dbProject.SetingSystems.FirstOrDefault(p=>p.ID==1);

                    itemResult.PageSize = int.Parse(Request.Form["pageSize"]);

                    _dbProject.SubmitChanges();

                    return RedirectToAction("SettingGeneral","Information");
                }
                else
                {

                    return RedirectToAction("Login", "Admin");
                }
            }
            catch
            {
                return RedirectToAction("Login", "Admin");
            }
        }

        public ActionResult UpdateLogo(HttpPostedFileBase file)
        {
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {
                    SetingSystem itemResult = _dbProject.SetingSystems.FirstOrDefault(p => p.ID == 1);

                    string path = Server.MapPath("~/Frondend_files/images");

                    string strName = file.FileName;

                    if (file != null && file.ContentLength > 0)
                    {
                        string paths = System.IO.Path.Combine(path, System.IO.Path.GetFileName(strName));
                        file.SaveAs(paths);
                    }

                    itemResult.Logo = "Frondend_files/images/" + strName;            

                    _dbProject.SubmitChanges();

                    return RedirectToAction("SettingGeneral", "Information");
                }
                else
                {

                    return RedirectToAction("Login", "Admin");
                }
            }
            catch
            {
                return RedirectToAction("Login", "Admin");
            }
        }

        public ActionResult UpdateImages(HttpPostedFileBase fileImages)
        {
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {
                    SetingSystem itemResult = _dbProject.SetingSystems.FirstOrDefault(p => p.ID == 1);

                    string path = Server.MapPath("~/Frondend_files/images");

                    string strName = fileImages.FileName;

                    if (fileImages != null && fileImages.ContentLength > 0)
                    {
                        string paths = System.IO.Path.Combine(path, System.IO.Path.GetFileName(strName));
                        fileImages.SaveAs(paths);
                    }

                    itemResult.ImagesDC = "Frondend_files/images/" + strName;            

                    _dbProject.SubmitChanges();

                    return RedirectToAction("SettingGeneral", "Information");
                }
                else
                {

                    return RedirectToAction("Login", "Admin");
                }
            }
            catch
            {
                return RedirectToAction("Login", "Admin");
            }
        }        

        public ActionResult UpdateRate()
        {
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {
                    DataNMDataContext _dbProject = new DataNMDataContext();

                    string sUrl = "http://vietcombank.com.vn/ExchangeRates/ExrateXML.aspx";
                    XmlDocument rssDoc = new XmlDocument();
                    XmlTextReader rssReader = new XmlTextReader(sUrl.ToString());

                    WebRequest wrGETURL;
                    wrGETURL = WebRequest.Create(sUrl);

                    Stream objStream;
                    objStream = wrGETURL.GetResponse().GetResponseStream();
                    StreamReader objReader = new StreamReader(objStream, Encoding.UTF8);
                    WebResponse wr = wrGETURL.GetResponse();
                    Stream receiveStream = wr.GetResponseStream();
                    StreamReader reader = new StreamReader(receiveStream, Encoding.UTF8);
                    string content = reader.ReadToEnd();

                    string Usd = content.Substring(content.IndexOf("USD"), 100);
                    string Eur = content.Substring(content.IndexOf("EUR"), 100);
                    string aud = content.Substring(content.IndexOf("AUD"), 100);

                    string EurLast = Eur.Substring(Eur.IndexOf("Sell=") + 6, Eur.IndexOf("/>") - (Eur.IndexOf("Sell=")) - 8);
                    string UsdLast = Usd.Substring(Usd.IndexOf("Sell=") + 6, Usd.IndexOf("/>") - (Usd.IndexOf("Sell=")) - 8);
                    string AUDLast = aud.Substring(aud.IndexOf("Sell=") + 6, aud.IndexOf("/>") - (aud.IndexOf("Sell=")) - 8);

                    SetingSystem setingSystemResult = _dbProject.SetingSystems.FirstOrDefault(p => p.ID == 1);

                    setingSystemResult.EUR = EurLast;
                    setingSystemResult.USD = UsdLast;
                    setingSystemResult.AUD = AUDLast;

                    _dbProject.SubmitChanges();

                   return RedirectToAction("SettingGeneral", "Information");
                }
                else
                {

                    return RedirectToAction("Login", "Admin");
                }
            }
            catch
            {
                return RedirectToAction("Login", "Admin");
            }
        }

        public ActionResult UpdateFacebook()
        {
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {
                    SetingSystem itemResult = _dbProject.SetingSystems.FirstOrDefault(p => p.ID == 1);

                    itemResult.Facebook = Request.Form["facebook"];

                    _dbProject.SubmitChanges();

                    return RedirectToAction("SettingGeneral", "Information");
                }
                else
                {

                    return RedirectToAction("Login", "Admin");
                }
            }
            catch
            {
                return RedirectToAction("Login", "Admin");
            }
 
        }

        public ActionResult UpdateAdd()
        {
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {
                    SetingSystem itemResult = _dbProject.SetingSystems.FirstOrDefault(p => p.ID == 1);

                    itemResult.Add = Request.Form["Add"];

                    _dbProject.SubmitChanges();

                    return RedirectToAction("SettingGeneral", "Information");
                }
                else
                {

                    return RedirectToAction("Login", "Admin");
                }
            }
            catch
            {
                return RedirectToAction("Login", "Admin");
            }
 
        }
    }
}

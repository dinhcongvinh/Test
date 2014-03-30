using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NM.Models;
using System.Net;
using System.Net.Mail;

namespace NM.Controllers
{
    public class CartController : Controller
    {
        //
        // GET: /Cart/
        #region Avarivable       

        #endregion

        public ActionResult Index()
        {
            try
            {
                 ViewData["intoMoney"] = IntoMoneyInCart();
            }
            catch
            {
                ViewData["intoMoney"] = "0";
            }
            return View();
        }

        public string AddProduct()
        {
            DataNMDataContext _dbProject = new DataNMDataContext();

            int idProduct = int.Parse(Request.QueryString["idProduct"]);
            bool isExsit = false;
            string returnString = "0";
            try
            {
                List<ItemCarts> listCart = (List<ItemCarts>)Session["listCart"];
                foreach (var item in listCart)
                {
                    if (item.idProduct == idProduct)
                    {
                        item.numberProduct++;
                        item.intoMoney = item.numberProduct * item.priceProduct;
                        
                        isExsit = true;
                        // gán giá trị cho session gio hang
                        Session["listCart"] = listCart;
                        listCart = null;

                        //san pham ton tai trong gio hang
                        returnString = "1";
                        break;
                    }
                }

                if (isExsit == false)
                {
                    Product itemResult = _dbProject.Products.Single(item => item.IDProduct == idProduct);
                    ItemCarts itemNew = new ItemCarts();

                    itemNew.idProduct = itemResult.IDProduct;
                    itemNew.imageProduct = itemResult.ImagesLogoProduct;
                    itemNew.namProduct = itemResult.NamesProduct;
                    itemNew.priceProduct = double.Parse(itemResult.PriceProduct.ToString());
                    itemNew.numberProduct = 1;
                    itemNew.intoMoney = itemNew.numberProduct * itemNew.priceProduct;

                    listCart.Add(itemNew);
                    Session["listCart"] = listCart;
                    listCart = null;

                    // sản phẩm chưa tồn tài và thêm mới 
                    returnString = "2";
                }
            }
                //session gio hang chua ton tai
            catch
            {
                List<ItemCarts> listCart = new List<ItemCarts> ();
                Product itemResult = _dbProject.Products.Single(item => item.IDProduct == idProduct);
                ItemCarts itemNew = new ItemCarts();

                itemNew.idProduct = itemResult.IDProduct;
                itemNew.imageProduct = itemResult.ImagesLogoProduct;
                itemNew.namProduct = itemResult.NamesProduct;
                itemNew.priceProduct = double.Parse(itemResult.PriceProduct.ToString());
                itemNew.numberProduct = 1;
                itemNew.intoMoney = itemNew.numberProduct * itemNew.priceProduct;

                listCart.Add(itemNew);
                Session["listCart"] = listCart;
                listCart = null;
                // sản phẩm chưa tồn tài và thêm mới 
                returnString = "2";
            }
            return returnString;
        }

        public string CountItemInCart()
        {
            List<ItemCarts> listCart = (List<ItemCarts>)Session["listCart"];
            int countItem = listCart.Count();
            Session["countProduct"] = countItem;

            listCart = null;

            return countItem.ToString();
        }

        public string IntoMoneyInCart()
        {
            List<ItemCarts> listCart = (List<ItemCarts>)Session["listCart"];
            double tatolAmount = 0;

            foreach (var item in listCart)
            {
                tatolAmount += item.intoMoney;
            }
            Session["sumMoney"] = int.Parse(tatolAmount.ToString()).ToString("0,0") + " đ";

            return Session["sumMoney"].ToString();
        }

        public string IntoMoneyInCartCurrency( )
        {
            DataNMDataContext _dbProject = new DataNMDataContext();

            int Idcurrency = int.Parse(Request.QueryString["id"]);
            try
            {
                List<ItemCarts> listCart = (List<ItemCarts>)Session["listCart"];
                double totalAmount = 0;

                foreach (var item in listCart)
                {
                    totalAmount += item.intoMoney;
                }
                SetingSystem itemResult = _dbProject.SetingSystems.FirstOrDefault(p => p.ID == 1);
                double totalAmountCurrentcy = 0;
                string end = " đ";

                switch (Idcurrency)
                {
                    case 1:
                        totalAmountCurrentcy = totalAmount;
                        break;
                    //
                    case 2:
                        if (totalAmount % Convert.ToDouble(itemResult.EUR) != 0)
                        {
                            totalAmountCurrentcy = totalAmount / Convert.ToDouble(itemResult.EUR) + 1;
                        }
                        else
                        {
                            totalAmountCurrentcy = totalAmount / Convert.ToDouble(itemResult.EUR);
                        }
                        end = " eur";
                        break;
                    case 3:
                        if (totalAmount % Convert.ToDouble(itemResult.USD) != 0)
                        {
                            totalAmountCurrentcy = totalAmount / Convert.ToDouble(itemResult.USD) + 1;
                        }
                        else
                        {
                            totalAmountCurrentcy = totalAmount / Convert.ToDouble(itemResult.USD);
                        }
                        end = " usd";
                        break;

                    case 4:
                        if (totalAmount % Convert.ToDouble(itemResult.AUD) != 0)
                        {
                            totalAmountCurrentcy = totalAmount / Convert.ToDouble(itemResult.AUD) + 1;
                        }
                        else
                        {
                            totalAmountCurrentcy = totalAmount / Convert.ToDouble(itemResult.AUD);
                        }
                        end = " aud";

                        break;
                }


                return double.Parse(totalAmountCurrentcy.ToString()).ToString("0,0") + end;
            }
            catch
            {
                return "";
            }
        }

        public string DeleteProduct()
        {
            int idProduct = int.Parse(Request.QueryString["idproduct"]);
            List<ItemCarts> listCart = (List<ItemCarts>)Session["listCart"];

            listCart.Remove(listCart.Single(item=>item.idProduct==idProduct));

            listCart = null;

            return IntoMoneyInCart();
        }

        public string EditProduct()
        {
            string intoMoney = "0";
            int idProduct = int.Parse(Request.QueryString["idProduct"]);
            int numberProduct = int.Parse(Request.QueryString["numberProduct"]);

            List<ItemCarts> listCart = (List<ItemCarts>)Session["listCart"];

            foreach (var item in listCart)
            {
                if (item.idProduct == idProduct)
                {
                    item.numberProduct = numberProduct;
                    item.intoMoney = item.priceProduct * item.numberProduct;
                    intoMoney = int.Parse(item.intoMoney.ToString()).ToString("0,0");
                    break;
                }
            }
            listCart = null;

            return intoMoney;
        }

        public ActionResult Purchase()
        {
            DataNMDataContext _dbProject = new DataNMDataContext();
            try
            {
                List<ItemCarts> listCart = (List<ItemCarts>)Session["listCart"];

                if (listCart.Count() > 0)
                {
                    double totalAmount = 0; 
                    foreach (var item in listCart)
                    {
                        totalAmount += item.intoMoney;
                    }

                    Order itemOrder = new Order();

                    itemOrder.NamePeopleBuy = Request.Form["Custommer"].ToString();
                    itemOrder.PhonePeopleBuy = Request.Form["Phone"].ToString();
                    itemOrder.EmailPeopleBuy = Request.Form["Email"].ToString();
                    itemOrder.AdressPeopleBuy = Request.Form["Address"].ToString();
                    itemOrder.DescriptionPeopleBuy = Request.Form["Note"].ToString();
                    itemOrder.TotalMoney = totalAmount;
                    itemOrder.CreateDay = DateTime.Parse(Request.Form["Date"].ToString());
                    itemOrder.IsDelete = false;
                    itemOrder.IsPayment = false;

                    _dbProject.Orders.InsertOnSubmit(itemOrder);
                    _dbProject.SubmitChanges();

                    OrdersDetail itemDetail;
                    foreach (var item in listCart)
                    {
                        itemDetail = new OrdersDetail();

                        itemDetail.IDOrders = itemOrder.IDOrders;
                        itemDetail.IDProduct = item.idProduct;
                        itemDetail.NameProduct = item.namProduct;
                        itemDetail.PriceNameProduct = item.priceProduct;
                        itemDetail.NumberProduct = item.numberProduct;
                        itemDetail.TotalMoney = item.intoMoney;
                        itemDetail.Images = item.imageProduct;

                        _dbProject.OrdersDetails.InsertOnSubmit(itemDetail);
                        _dbProject.SubmitChanges();
                    }

                    listCart = null;
                    Session["listCart"] = listCart;

                    string content = " khách hàng " + itemOrder.NamePeopleBuy + " vừa đặt hàng tại HAYFASHION." + "<br/> <br/>"
                        + " Thông tin đơn đặt hàng: " + "<br/>"
                        + "- Điện thoại liên hệ: " + itemOrder.PhonePeopleBuy + "<br/>"
                        + "- Email liên hệ: " + itemOrder.EmailPeopleBuy + "<br/>"
                        + "- Địa chỉ liên hệ: " + itemOrder.AdressPeopleBuy + "<br/>"
                        + "- Ngày đặt hàng: " + DateTime.Now.ToString() + "<br/>"
                        + "- Tổng tiền đơn hàng: " + double.Parse(itemOrder.TotalMoney.ToString()).ToString("0,0") + " VNĐ" + "<br/>"
                        + "Xem chi tiết tại Admin HAYFASHION.VN ";

                    SendEmail("hayfashion2013@gmail.com", content, "Thông báo từ HAYFASHION.VN -- " + itemOrder.NamePeopleBuy + " vừa đặt hàng ");

                    ViewBag.Mess = "Đặt hàng thành công. Chúng tôi sẽ liên hệ với bạn trong vòng 24h ";
                }
                else
                {
                    ViewBag.Mess = "Giỏ hàng của bạn không có sản phẩm nào! ";
                }  
            }
            //session gio hang chua ton tai
            catch
            {
               
                ViewBag.Mess = "Giỏ hàng bạn không có sản phẩm! ";
            }

            return View();
        }

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
    }
}

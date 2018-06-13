using Portelinha.Helpers;
using Portelinha.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Portelinha.Controllers
{
    public class AccountController : Controller
    {
        private Model1 db = new Model1();
       
        public ActionResult Index(string user)
        {
            ViewData["User_Name"] = "Bem vindo" + user;
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            DefaultController.Funcionario = new Funcionario();
            DefaultController.Funcionario.Pass_word = password;
            int userName = Int32.Parse(username);
            if (ModelState.IsValid)
            {
                var userC = (from m in db.Cliente where (m.Id == userName) select m);

                if (userC.ToList().Count > 0)
                {
                    Cliente cliente = userC.ToList().ElementAt<Cliente>(0);
                    using (MD5 md5Hash = MD5.Create())
                    {
                        if (MyHelpers.VerifyMd5Hash(md5Hash, password, cliente.Pass_word))
                        {
                            string client = cliente.Id.ToString();
                            HttpCookie cookie = MyHelpers.CreateAuthorizeTicket(cliente.Id.ToString(), cliente.Role);
                            Response.Cookies.Add(cookie);
                            ViewData["User_Name"] = "Bem vindo" + cliente.Nome;
                            return RedirectToAction("Index", "Cliente");
                        }
                        else
                        {
                            ModelState.AddModelError("password", "Password incorreta!");
                            return View();
                        }
                    }
                }
                else
                {
                    var userF = (from m in db.Funcionario where (m.Id == userName) select m);
                    if (userF.ToList().Count > 0)
                    {
                        Funcionario funcionario = userF.ToList().ElementAt<Funcionario>(0);
                        using (MD5 md5Hash = MD5.Create())
                        {
                            if (MyHelpers.VerifyMd5Hash(md5Hash, password, funcionario.Pass_word))
                            {
                                string func = funcionario.Id.ToString();
                                HttpCookie cookie = MyHelpers.CreateAuthorizeTicket(funcionario.Id.ToString(), funcionario.Role);
                                Response.Cookies.Add(cookie);
                                if (funcionario.Role.Equals("func"))
                                {
                                    ViewData["User_Name"] = "Bem vindo" + funcionario.Nome;
                                    return RedirectToAction("Index", "Funcionario");
                                }
                                else
                                {
                                    var t = ViewData["User_Name"] = "Bem vindo" + funcionario.Nome;
                                    return RedirectToAction("Index", "Admin");
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("password", "Password incorreta!");
                                return View();
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Login data is incorrect!");
                        return View();
                    }
                }
            }
            return View();
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Registar(Cliente client)
        {
            if (ModelState.IsValid)
            {
                using (Model1 db = new Model1())
                {
                    db.Cliente.Add(client);
                    db.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.Message = client.Nome + " registada com sucesso.";
            }
            return View();
        }

        public ActionResult sucessAction()
        {
            ViewBag.title = "Sucesso";
            ViewBag.mensagem = "Login realizado com sucesso";
            ViewBag.controller = "Home";
            return View("_sucessView");
        }
    }
}
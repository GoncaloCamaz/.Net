using Portelinha.Helpers;
using Portelinha.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portelinha.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private Model1 db = new Model1();

        public ActionResult Index()
        {
            return View();
        }

        public void consultarFaturas()
        {
            var faturas = from m in db.Fatura select m;
        }

        public void consultarClientes()
        {
            var clientes = from m in db.Cliente select m;
        }

        public ActionResult VerFuncionarios()
        {
            List<Funcionario> listaFunc = new List<Funcionario>();

            listaFunc = (from m in db.Funcionario select m).ToList();

            return View(listaFunc);
        }

        [HttpPost]
        public void adicionarFuncionario([Bind(Include = "nif,nome,email,telemovel,CP,nr,apartamento,rua,password")] Funcionario func)
        {
            Model1 db = new Model1();
            func.setRole();

            if (ModelState.IsValid)
            {
                func.Pass_word = MyHelpers.HashPassword(func.Pass_word);
                db.Funcionario.Add(func);
                db.SaveChanges();
            }
           // return RedirectToAction("sucessOperation");
        }
    }
}
using Portelinha.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portelinha.Controllers
{
    public class CatalogoController : Controller
    {
        // GET: Produto
        private Model1 db = new Model1();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult consultarCatalogo()
        {
            var packs = (from m in db.Pack select m);

            List<Pack> lista = packs.ToList<Pack>();

            return View(lista);
        }
    }
}
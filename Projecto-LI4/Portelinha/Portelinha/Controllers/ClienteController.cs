using Portelinha.Helpers;
using Portelinha.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Portelinha.Controllers
{
    public class ClienteController : Controller
    {
        private Model1 db = new Model1();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AdicionarCliente()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        // Função para adicionar novo cliente. Mais tarde poderá ir para o Account controller
        [HttpPost]
        public ActionResult AdicionarCliente(string name, int NIF, string Email, int Telemovel,string CP, int Nr, string Ap, string Rua,string password)
        {
            var localidade = (from m in db.CP_Localidade where m.Id == CP select m).ToList().FirstOrDefault();
          
            Cliente cliente = new Cliente()
            {
                Apartamento = Ap,
                CP = CP,
                CP_Localidade = localidade,
                Email = Email,
                Id = NIF,
                Nome = name,
                Nr = Nr,
                Pass_word = password,
                Role = "user",
                Rua = Rua,
                Telemovel = Telemovel,
            };
  
            cliente.setRole();


            if (ModelState.IsValid)
            {
                cliente.Pass_word = MyHelpers.HashPassword(cliente.Pass_word);
                db.Cliente.Add(cliente);
                db.SaveChanges();
            }
      
            return RedirectToAction("Index","Home");
        }

        // Consultar catálogo de packs
        [Authorize]
        public ActionResult verPacks()
        {
            var packs = (from m in db.Pack select m);

            List<Pack> lista = packs.ToList<Pack>();

            return View(lista);
        }

        ///////////////////////////////////////////////////////// RELATIVO A REQUISITAR SERVIÇO ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [Authorize]
        public ActionResult VerPacksData(DateTime? data, int? preco)
        {
            bool temFiltros = false;
            int precoLS = 0;
            if (preco.HasValue)
            {
                temFiltros = true;
                precoLS = preco.Value;
            }
            
            List<Pack> pack = new List<Pack>();
            List<Servico> servicos = new List<Servico>();
            DateTime date;
            if(data.HasValue)
            {
                date = data.Value;
                Helpers.CacheController.Data = date;
                servicos = (from m in db.Servico select m).ToList<Servico>();

                if (ModelState.IsValid && date >= DateTime.Now.AddDays(7))
                {
                    if(temFiltros)
                    {
                        if (servicos.Count == 0)
                        {
                            pack = (from m in db.Pack where (m.Preco <= precoLS) select m).ToList<Pack>();
                        }
                        else
                        {
                            var packsToIgnore = (from m in db.Servico
                                                 join n in db.Pack on m.Id_Pack equals n.Id
                                                 where (DbFunctions.TruncateTime(m.DataHora) == DbFunctions.TruncateTime(date))
                                                 select n).ToList<Pack>();

                            pack = ((from m in db.Pack where (m.Preco <= precoLS) select m).ToList<Pack>()).Except(packsToIgnore).ToList<Pack>();
                        }
                    }
                   else
                    {
                        if (servicos.Count == 0)
                        {
                            pack = (from m in db.Pack select m).ToList<Pack>();
                        }
                        else
                        {
                            var packsToIgnore = (from m in db.Servico
                                                 join n in db.Pack on m.Id_Pack equals n.Id
                                                 where (DbFunctions.TruncateTime(m.DataHora) == DbFunctions.TruncateTime(date))
                                                 select n).ToList<Pack>();

                            pack = ((from m in db.Pack select m).ToList<Pack>()).Except(packsToIgnore).ToList<Pack>();
                        }
                    }
                } 
            }
            else
            {
                ModelState.AddModelError("", "Insira uma data");
            }
            return View(pack);
        }

        public ActionResult Selecionar(int? id_Pack)
        {
            Helpers.CacheController.id_Pack = id_Pack.Value;

            return View("RequisitarServico");
        }

        [Authorize]
        public ActionResult RequisitarServico(string cp, string rua, int? nrPorta, string apartamento)
        {
            FuncionarioController funcC = new FuncionarioController();
            var id_Pack = Helpers.CacheController.id_Pack;
            var data = Helpers.CacheController.Data;

            var servs = from m in db.Servico select m;
            int lengthS = servs.ToList<Servico>().Count;
            lengthS += 1;

            var fatur = from m in db.Fatura select m;
            int lengthF = fatur.ToList<Fatura>().Count;
            lengthF += 1;

            string estado = "Agendado";
            int idCliente = Int32.Parse(User.Identity.Name);

            var pack = (from m in db.Pack where m.Id == id_Pack select m).First();
            decimal precoAux = pack.Preco;

            var localidade = (from m in db.CP_Localidade where m.Id == cp select m).ToList().FirstOrDefault();
            var cliente = (from c in db.Cliente where c.Id == idCliente select c).ToList().FirstOrDefault();

            Fatura fact = new Fatura()
            {
                Id = lengthF,
                Apartamento = apartamento,
                CP = cp,
                CP_Localidade = localidade,
                Data = data,
                Id_Pack = id_Pack,
                Localidade = localidade.Localidade,
                NIF = cliente.Id,
                Nome_Cliente = cliente.Nome,
                Nome_Pack = pack.Nome,
                Nr = nrPorta.Value,
                Preco = pack.Preco,
                Rua = rua,
                Servico = servs.Where(x => x.Id_Pack == id_Pack && (DbFunctions.TruncateTime(x.DataHora) == DbFunctions.TruncateTime(data))).ToList()
            };

            string comentario = "";

            Servico serv = new Servico(lengthS, data, precoAux, estado, apartamento, cp, rua ,nrPorta.Value , idCliente, id_Pack, lengthF, comentario);

            if (ModelState.IsValid)
            {
                db.Fatura.Add(fact);
                db.SaveChanges();
                db.Servico.Add(serv);
                db.SaveChanges();
                funcC.AlocarFuncionarios(lengthS);
            }
            ViewBag.Message = "Serviço Registado Com Sucesso";

            ///////////////////////////////////////////Geração de Email com confirmação do serviço//////////////////////////////////////////////////////////
            string emailC = cliente.Email;
            string nomeP = pack.Nome;
            string loc = localidade.Localidade;

            Helpers.GeradorFaturas.ConfirmarServicoEmail(emailC, precoAux, data, nomeP, rua, loc, cp);
            /////////////////////////////////////////////////////////////////////////////////////////////////////

            return RedirectToAction("Index", "Cliente");
        }

        [Authorize]
        public ActionResult CancelarServico(int? idServico)
        {
            FuncionarioController funcC = new FuncionarioController();
            if(idServico.HasValue)
            {
                var servico = (from m in db.Servico where m.Id == idServico.Value select m).First();
                int idFatura = servico.Id_Fatura;
                var fatura = (from m in db.Fatura where m.Id == idFatura select m).First();
                var servicoFunc = servico.Funcionario;

                servico.SetEstadoCancalado();
                db.SaveChanges();
            }

            return RedirectToAction("HistoricoServicos", "Cliente");
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public ActionResult HistoricoServicos()
        {
            int idCliente = Int32.Parse(User.Identity.Name);
            var servicos = (from m in db.Servico where m.Id_Cliente == idCliente && (m.Estado.Equals("Agendado") || m.Estado.Equals("Completo")) select m).ToList();

            return View(servicos);
        }

        public ActionResult VerDetalhes(int idServico)
        {
            var servico = (from m in db.Servico where (m.Id == idServico) select m).ToList();

            return View(servico);
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public ActionResult SuccessActionRequestServ()
        {
            ViewBag.title = "Sucesso";
            ViewBag.mensagem = "Serviço registado com sucesso";
            ViewBag.controller = "Cliente";
            return View("_sucessView");
        }

        public ActionResult sucessActionDeleteServ()
        {
            ViewBag.title = "Sucesso";
            ViewBag.mensagem = "Serviço cancelado com sucesso";
            ViewBag.controller = "Cliente";
            return View("_sucessView");
        }

        public ActionResult sucessActionDeleteServFailed()
        {
            ViewBag.title = "Insucesso";
            ViewBag.mensagem = "O serviço não foi cancelado";
            ViewBag.controller = "Cliente";
            return View("_sucessView");
        }

        private bool equals(string v)
        {
            return (equals(v) ? true : false);

        }
    }
}
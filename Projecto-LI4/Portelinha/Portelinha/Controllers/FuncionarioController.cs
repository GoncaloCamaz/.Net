using Portelinha.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portelinha.Controllers
{
    public class FuncionarioController : Controller
    {
        private Model1 db = new Model1();
     
        public ActionResult Index()
        {
            return View();
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void AlocarFuncionarios(int idServico)
        {
            // Saber a data do serviço
            var servico = (from m in db.Servico where m.Id == idServico select m).ToList().FirstOrDefault();

            DateTime dataServico = servico.DataHora;

            // Ver os funcionários Que estão disponíveis para essa data
            var servicosNaData = (from n in db.Servico where (DbFunctions.TruncateTime(n.DataHora) == DbFunctions.TruncateTime(dataServico)) select n).ToList();
            var funcionarios = (from n in db.Funcionario where n.Role.Equals("func") select n).ToList();
            int i = 0;

            if (servicosNaData.Count == 0)
            {
                while(i < 2)
                {
                    Funcionario f = funcionarios.ElementAt(i);
                    servico.Funcionario.Add(f);
                    i++;
                }
                db.SaveChanges();
            }
            else
            {
                i = 0;
                foreach (var s in servicosNaData)
                {
                    if(!s.Estado.Equals("Cancelado"))
                    {
                        var func = s.Funcionario.ToList();
                        foreach (var fa in func)
                        {
                            funcionarios.Remove(fa);
                        }
                    }
                }
                while (i < 2)
                {
                    Funcionario f = funcionarios.ElementAt(i);
                    servico.Funcionario.Add(f);

                    i++;
                }
                db.SaveChanges();
            }
                  
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [Authorize]
        public ActionResult ConsultarCatalogo()
        {
            var catalogo = (from m in db.Pack select m).ToList();

            return View(catalogo);
        }

        [Authorize]
        public ActionResult VerServicos()
        {
            var serv = (from n in db.Servico where n.Estado == "Agendado" select n).ToList();
            var servicosPorFunc = new List<Servico>();
            int idFuncionario = Int32.Parse(User.Identity.Name);

            foreach (var s in serv)
            {
                foreach(var f in serv)
                {
                    var func = new List<Funcionario>();
                    func = s.Funcionario.ToList();
                    foreach(var fu in func)
                    {
                        if(fu.Id.Equals(idFuncionario))
                        {
                            servicosPorFunc.Add(s);
                        }
                    }
                }
            }

            return View(servicosPorFunc);
        }

        [Authorize]
        public ActionResult ObterDirecoes(int idServico)
        {
            var servico = (from m in db.Servico where m.Id == idServico select m);

            return View();
        }

        [Authorize]
        public ActionResult ConfirmarServico(int? idServico)
        {
            if(idServico.HasValue)
            {
                Helpers.CacheController.id_Servico = -1;
                Helpers.CacheController.id_Servico = idServico.Value;
                return View("Confirmar");
            }
            else return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Confirmar(string comentario)
        {
            int idServico = Helpers.CacheController.id_Servico;
            var servico = (from s in db.Servico where s.Id == idServico select s).ToList().FirstOrDefault();
            int idFatura = servico.Id_Fatura;
            var fatura = (from f in db.Fatura where f.Id == idFatura select f).ToList().FirstOrDefault();
            int idCliente = servico.Id_Cliente;
            var cliente = (from c in db.Cliente where c.Id == idCliente select c).ToList().FirstOrDefault();
            string email = cliente.Email;
            string nomeC = cliente.Nome;
            int telm = cliente.Telemovel;
            string rua = servico.Rua;
            int num = servico.Nr;
            string apartamento = servico.Apartamento;
            string localidade = servico.CP_Localidade.Localidade;
            string cp = servico.CP;
            string nomePack = servico.Pack.Nome;
            decimal preco = servico.Preco;

            DateTime data = servico.DataHora;

            Helpers.GeradorFaturas.GeraFatura(idServico, data, email, telm, idCliente, nomeC, rua, num, apartamento, localidade, cp, nomePack, preco);
            servico.SetComentario(comentario);
            servico.SetEstadoCompleto();
            db.SaveChanges();

            return View("Index");
        }

    }
}
using System;
using System.Net.Mail;

namespace Portelinha.Helpers
{
    public class GeradorFaturas
    {
        public static void GeraFatura(int idS, DateTime dataEvento, string emailC, int telm, int nif, string nomeC, string rua, int nr, string apartamento, string localidade, string cp,  string nomePack, decimal preco)
        {
            string nomeEmpresa = "Portelinha, Lda";
            string moradaEmpresa = "Universidade do Minho, Campus Gualtar";
            string data = dataEvento.Date.ToString();
            int precoA = (int)preco;

            var text = String.Concat("Fatura Simplificada \n"
                , "--------------------------------------------\n"
                , "Nome Empresa: "
                , nomeEmpresa
                , " \n"
                , "Morada Empresa: "
                , moradaEmpresa
                ," \n"
                , "--------------------------------------------\n"
                , "Nome Cliente: "
                , nomeC
                ," \n"
                , "NIF: " 
                , nif 
                ," \n"
                , "Morada Cliente: " 
                , rua
                ," "
                ,nr 
                ," "
                , " \n"
                , "Código Postal: "
                ,cp
                ," \n"
                , "Localidade: "
                ,localidade
                ," \n"
                , "--------------------------------------------\n"
                , "Data: "
                ,data
                ," \n"
                , "Produto: "
                ,nomePack
                ," \n"
                , "Quantidade: "
                ,"1"
                ," \n"
                , "Preço: "
                ,precoA
                ,"€"
                ,"\n"
                , "--------------------------------------------\n");
   

            enviarEmailSMS(emailC, telm, text, idS);
        }

        public static void enviarEmailSMS(string emailcliente, int numerocliente, string dados, int idServico)
        {

            // INITIATE SMTP
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.UseDefaultCredentials = false;
            smtp.EnableSsl = true;
            smtp.Credentials = new System.Net.NetworkCredential("portelinhaLI4@gmail.com", "portelinha_LI4");

            // MESSAGE EMAIL
            MailMessage message = new MailMessage();
            message.To.Add(emailcliente);
            message.From = new MailAddress("portelinhaLI4@gmail.com", "Portelinha");
            message.Subject = "Envio de Fatura";
            message.Body = dados;

            smtp.Send(message);

            // MESSAGE SMS
            MailMessage message2 = new MailMessage(); ;
            message2.To.Add(string.Concat(numerocliente, "@txtlocal.co.uk"));
            message2.From = new MailAddress("portelinhaLI4@gmail.com", "Portelinha");
            message2.Subject = "Envio de Fatura";
            message2.Body = string.Concat("A fatura foi enviada para o email: " + emailcliente);

            smtp.Send(message2);
        }

        public static void ConfirmarServicoEmail(string emailCliente, decimal preco, DateTime data, string nomePack, string rua, string localidade, string cp)
        {
            string entidade = "100200300";
            string referencia = "20000";
            int precoL = (int) preco;
            DateTime dataS = data.Date;

            var textC = String.Concat("Pagamento do serviço \n"
                , "--------------------------------------------\n"
                , "Entidade: "
                , entidade
                , "\n"
                , "Referência: "
                , referencia
                , "\n"
                , "Valor: "
                , precoL
                , "€ \n"
                , "--------------------------------------------\n"
                , "Data Limite Pagamento: "
                , dataS
                , " \n"
                , "Nome Pack: "
                , nomePack
                , "\n"
                , "--------------------------------------------\n"
                , "Rua: "
                , rua
                , " \n"
                , "Código-Postal: "
                , cp
                , "\n"
                , "Localidade: "
                , localidade
                , " \n"
                , "--------------------------------------------\n");

            enviarEmailConfirmacaoServico(emailCliente, textC);

        }

        public static void enviarEmailConfirmacaoServico(string emailCliente, string texto)
        {
            // Initiate SMTP
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.UseDefaultCredentials = false;
            smtp.EnableSsl = true;
            smtp.Credentials = new System.Net.NetworkCredential("portelinhaLI4@gmail.com", "portelinha_LI4");

            // MESSAGE EMAIL
            MailMessage message = new MailMessage();
            message.To.Add(emailCliente);
            message.From = new MailAddress("portelinhaLI4@gmail.com", "Portelinha");
            message.Subject = "Confirmação de Serviço";
            message.Body = texto;

            smtp.Send(message);
        }
    }
}
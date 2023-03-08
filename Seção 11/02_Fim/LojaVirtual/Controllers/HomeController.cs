using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LojaVirtual.Libraries.Email;
using LojaVirtual.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text;
using LojaVirtual.Database;
using LojaVirtual.Repositories.Contracts;
using Microsoft.AspNetCore.Http;
using LojaVirtual.Libraries.Login;

namespace LojaVirtual.Controllers
{
    public class HomeController : Controller
    {
        private IClienteRepository _repositoryCliente;
        private INewsletterRepository _repositoryNewsletter;
        private LoginCliente _loginCliente;

        public HomeController(IClienteRepository repositoryCliente, INewsletterRepository repositoryNewsletter, LoginCliente loginCliente)
        {
            _repositoryCliente = repositoryCliente;
            _repositoryNewsletter = repositoryNewsletter;
            _loginCliente = loginCliente;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index([FromForm]NewsletterEmail newsletter)
        {
            if (ModelState.IsValid)
            {
                _repositoryNewsletter.Cadastrar(newsletter);

                TempData["MSG_S"] = "E-mail cadastrado! Agora você vai receber promoções especiais no seu e-mail! Fique atento as novidades!";

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View();
            }
        }

        public IActionResult Contato()
        {
            return View();
        }

        public IActionResult ContatoAcao()
        {
            try
            {
                Contato contato = new Contato();
                contato.Nome = HttpContext.Request.Form["nome"];
                contato.Email = HttpContext.Request.Form["email"];
                contato.Texto = HttpContext.Request.Form["texto"];

                var listaMensagens = new List<ValidationResult>();
                var contexto = new ValidationContext(contato);
                bool isValid = Validator.TryValidateObject(contato, contexto, listaMensagens, true);

                if (isValid)
                {
                    //ContatoEmail.EnviarContatoPorEmail(contato);

                    //return new ContentResult() { Content = string.Format("Dados recebidos com sucesso!<br /> Nome: {0}<br /> E-mail: {1}<br /> Texto: {2}",contato.Nome, contato.Email, contato.Texto), ContentType = "text/html" };

                    ViewData["MSG_S"] = "Mensagem enviada com sucesso!";
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var texto in listaMensagens)
                    {
                        sb.Append(texto.ErrorMessage + "<br />");
                    }

                    ViewData["MSG_E"] = sb.ToString();
                    ViewData["CONTATO"] = contato;
                }
            }
            catch (Exception e)
            {
                ViewData["MSG_E"] = "Opps! Tivemos um erro, tente novamente mais tarde!";

                //TODO - Implementar Log
            }

            return View("Contato");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login([FromForm]Cliente cliente)
        {
            Cliente clienteDB = _repositoryCliente.Login(cliente.Email, cliente.Senha);

            if (clienteDB != null)
            {
                //HttpContext.Session.Set("ID", new byte[] { 52 });
                //HttpContext.Session.SetString("Email", cliente.Email);
                //HttpContext.Session.SetInt32("Idade", 25);

                _loginCliente.Login(clienteDB);

                //return new ContentResult() { Content = "Logado!" };

                return new RedirectResult(Url.Action(nameof(Painel)));
            }
            else
            {
                ViewData["MSG_E"] = "Usuário não encontrado, verifique o e-mail e senha digitado!";
                return View();
            }
        }

        [HttpGet]
        public IActionResult Painel()
        {
            Cliente cliente = _loginCliente.GetCliente();

            if (cliente != null)
            {
                return new ContentResult() { Content = "Usuário " + cliente.Id.ToString() + " - E-mail: " + cliente.Email + " - Idade: " + (DateTime.Now.Year - cliente.Nascimento.Year).ToString() + " >> Logado!" };
            }
            else
            {
                return new ContentResult() { Content = "Acesso Negado." };
            }
          
        }

        [HttpGet]
        public IActionResult CadastroCliente()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CadastroCliente([FromForm]Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _repositoryCliente.Cadastrar(cliente);

                TempData["MSG_S"] = "Cadastro realizado com sucesso!";

                //TODO - Implementar redirecionamentos diferentes (Painel, Carrinho de Compras, etc.)
                return RedirectToAction(nameof(CadastroCliente));
            }
            return View();
        }

        public IActionResult CarrinhoCompras()
        {
            return View();
        }
    }
}
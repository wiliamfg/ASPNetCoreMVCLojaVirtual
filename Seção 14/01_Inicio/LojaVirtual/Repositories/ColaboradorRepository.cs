using LojaVirtual.Database;
using LojaVirtual.Models;
using LojaVirtual.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace LojaVirtual.Repositories
{
    public class ColaboradorRepository : IColaboradorRepository
    {
        private IConfiguration _conf;
        private LojaVirtualContext _banco;

        public ColaboradorRepository(LojaVirtualContext banco, IConfiguration configuration)
        {
            _banco = banco;
            _conf = configuration;
        }

        public void Atualizar(Colaborador colaborador)
        {
            _banco.Update(colaborador);
            _banco.Entry(colaborador).Property(p => p.Senha).IsModified = false;
            _banco.SaveChanges();
        }

        public void AtualizarSenha(Colaborador colaborador)
        {
            _banco.Update(colaborador);
            _banco.Entry(colaborador).Property(p => p.Nome).IsModified = false;
            _banco.Entry(colaborador).Property(p => p.Email).IsModified = false;
            _banco.Entry(colaborador).Property(p => p.Tipo).IsModified = false;
            _banco.SaveChanges();
        }

        public void Cadastrar(Colaborador colaborador)
        {
            _banco.Add(colaborador);
            _banco.SaveChanges();
        }

        public void Excluir(int Id)
        {
            Colaborador colaborador = ObterColaborador(Id);
            _banco.Remove(colaborador);
            _banco.SaveChanges();
        }

        public Colaborador Login(string Email, string Senha)
        {
            Colaborador colaborador = _banco.Colaboradores.Where(c => c.Email == Email && c.Senha == Senha).FirstOrDefault();
            return colaborador;
        }

        public Colaborador ObterColaborador(int Id)
        {
            return _banco.Colaboradores.Find(Id);
        }

        public List<Colaborador> ObterColaboradorPorEmail(string email)
        {
            return _banco.Colaboradores.Where(c => c.Email == email).AsNoTracking().ToList();
        }

        public IPagedList<Colaborador> ObterTodosColaboradores(int? pagina)
        {
            int registroPorPagina = _conf.GetValue<int>("RegistroPorPagina");

            int numeroPagina = pagina ?? 1;
            return _banco.Colaboradores.Where(c => c.Tipo != "G").ToPagedList<Colaborador>(numeroPagina, registroPorPagina);
        }
    }
}

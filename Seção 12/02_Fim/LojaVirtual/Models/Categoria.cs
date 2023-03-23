using LojaVirtual.Libraries.Lang;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual.Models
{
    public class Categoria
    {
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E001")]
        [MinLength(4, ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E002")]
        //TODO - Criar Validação - Nome Categoria único no banco de dados
        public string Nome { get; set; }

        /*
         * Nome: Telefone sem fio
         * Slug: telefone-sem-fio
         * URL Normal: www.lojavirtual.com.br/Categoria/5
         * URL Amigável: www.lojavirtual.com.br/Categoria/informatica
         */
        [Required(ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E001")]
        [MinLength(4, ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E002")]
        public string Slug { get; set; }

        /*
         * Auto-relacionamento
         */
        [Display(Name = "Categoria Pai")]
        public int? CategoriaPaiId { get; set; }

        /*
         * ORM - EntityFrameworkCore
         * 
         */
        [ForeignKey("CategoriaPaiId")]
        public virtual Categoria CategoriaPai { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DisciplineTeam.Area52.Web.Models;
using System.ComponentModel.DataAnnotations;

namespace DisciplineTeam.Area52.Web.Models
{
    public class ViewAll
    {
        //Confirmado.cs
        public int CIdUsuario { get; set; }
        public int CIdGrupo { get; set; }
        public int CIdEvento { get; set; }
        public int CStatus { get; set; }
        
        //Evento.cs
        public int EIdEvento { get; set; }
        public int EIdGrupo { get; set; }
        public string ENome { get; set; }
        public string EData { get; set; }
        public string EHora { get; set; }
        public int ETipo { get; set; }
        public string EEndereco { get; set; }
        public string EDescricao { get; set; }

        //Grupo.cs
        public int GIdGrupo { get; set; }
        public int GIdJogo { get; set; }
        public string GNome { get; set; }
        public string GDescricao { get; set; }
        public string GImagem { get; set; }

        //Jogo.cs
        public int JIdJogo { get; set; }
        public int JIdAdmin { get; set; }
        public string JNome { get; set; }
        public string JDescricao { get; set; }
        public string JImagem { get; set; }

        //Mensagem.cs
        public int MsgIdMensagem { get; set; }
        public int MsgIdUsuario { get; set; }
        public int MsgIdGrupo { get; set; }
        public string MsgDatahora { get; set; }
        public string MsgTexto { get; set; }

        //Participante.cs
        public int PartIdUsuario { get; set; }
        public int PartIdGrupo { get; set; }
        public int PartStatus { get; set; }

        //Pessoa.cs
        public int PIdPessoa { get; set; }
        public string PNome { get; set; }
        public string PEmail { get; set; }
        public string PSenha { get; set; }
        public int PStatus { get; set; }

        //Usuario.cs
        [Required(ErrorMessage = "Nick is required")]
        public string UNick { get; set; }
        public string USexo { get; set; }
        [Required(ErrorMessage = "Date of birth is required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public string UDatanasc { get; set; }
        public string UCidade { get; set; }
        public string UEstado { get; set; }
        public string UCep { get; set; }
        public string UDescricao { get; set; }
        public string UImagem { get; set; }

        //ChangePassword.cs
        [Required(ErrorMessage = "Old Password is required")]
        public string OldPwd { get; set; }
        [Required(ErrorMessage = "New Password is required")]
        public string NewPwd { get; set; }
        [Required(ErrorMessage = "New Password confirmation is required")]
        public string NewPwdConfirm { get; set; }
    }
}
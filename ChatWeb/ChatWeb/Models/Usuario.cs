using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatWeb.Models
{
    public class Usuario
    {
        public int idUser { get; set; }
        public string email { get; set; }
        public string pass { get; set; }
        public string ConfirmarPass { get; set; }
    }
}
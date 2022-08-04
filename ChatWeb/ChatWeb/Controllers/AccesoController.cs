using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ChatWeb.Models;
using System.Data.SqlClient;
using System.Data;

namespace ChatWeb.Controllers
{
    public class AccesoController : Controller
    {

        static string cadena = "Data Source=LAPTOP-BQTIKUOI\\SQL; Initial Catalog=LoginAcceso;Integrated Security=true";
        // GET: Acceso
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Registrar()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Registrar(Usuario usuario)
        {
            bool registrado;
            string mensaje;

            if (usuario.pass == usuario.ConfirmarPass)
            {
                usuario.pass = ConvertirSha256(usuario.pass);//Encriptar password
            }
            else
            {
                ViewData["Mensaje"] = "Las contraseñas no coinciden";//ViewData envia datos del controlador a la vista
                return View();
            }

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("RegistrarUser", cn);
                cmd.Parameters.AddWithValue("email", usuario.email);
                cmd.Parameters.AddWithValue("pass", usuario.pass);
                cmd.Parameters.Add("Registrado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();
                cmd.ExecuteNonQuery();

                registrado = Convert.ToBoolean(cmd.Parameters["Registrado"].Value);
                mensaje = cmd.Parameters["Mensaje"].Value.ToString();

            }
            ViewData["Mensaje"] = mensaje;
            if (registrado)
            {
                return RedirectToAction("Login", "Acceso");
            }
            else
            {
                return View();
            }

        }
        [HttpPost]

        public ActionResult Login(Usuario usuario)
        {
            usuario.pass = ConvertirSha256(usuario.pass);
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("ValidarUser", cn);
                cmd.Parameters.AddWithValue("email", usuario.email);
                cmd.Parameters.AddWithValue("pass", usuario.pass);
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();
                usuario.idUser = Convert.ToInt32 (cmd.ExecuteScalar().ToString()); //solo lee la primera fila y la primera columna 

            }
            if (usuario.idUser != 0)//Si se encuentra un usuario
            {
                Session["usuario"] = usuario; //session almacena datos
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewData["Mensaje"] = "usuario no encontrado";
                return View();


            }
        }

        public static string ConvertirSha256(string texto)
        {
            //using System.Tex
            //using System.Security.Criptography

            StringBuilder sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));

                foreach (byte b in result)
                    sb.Append(b.ToString("x2"));

            } 
            return sb.ToString();
        }
    }
}
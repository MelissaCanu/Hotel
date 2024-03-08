using Hotel.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hotel.Controllers
{
    public class DipendentiController : Controller
    {
        // GET: Dipendenti/Login
        public ActionResult Login()
        {
            return View(new Dipendente());
        }

        //POST: Dipendenti/Login - Mi connetto al db e verifico se c'è un dipendente con le credenziali inserite. Se c'è, verifico la password. Se corretta, memorizzo l'id nella session.

        [HttpPost]
        public ActionResult Login(string username, string password)
        {

            string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString.ToString();
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM Dipendenti WHERE Username = @username", connection);
                cmd.Parameters.AddWithValue("@username", username);

                SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.Read())
                {
                    // nome utente non esiste, errore
                    return View("Error");
                }

                // la password dell'utente nel database 

                string passwordDalDatabase = reader["Password"].ToString();

                // Verifico se la password inserita è corretta 

                bool passwordCorretta = VerifyPassword(password, passwordDalDatabase);

                if (!passwordCorretta)
                {
                    // password errata, errore
                    return View("Error");
                }

                Session["DipendenteID"] = reader["ID"].ToString();
                TempData["SuccessMessage"] = "Login riuscito!";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {   
                //errori dettagliati in caso di fallimento
                Console.WriteLine(e.Message);
                return Content("Si è verificato un errore: " + e.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        // Metodo per verificare la password, ripreso sopra nel metodo Login
        private bool VerifyPassword(string passwordInserita, string passwordDalDatabase)
        {
            return passwordInserita == passwordDalDatabase;
        }

        // GET: Dipendenti/Logout - Cancello l'id del dipendente dalla sessione e reindirizzo alla Home

        public ActionResult Logout()
        {
            Session["DipendenteID"] = null;
            return RedirectToAction("Login", "Dipendenti");
        }

    }
}

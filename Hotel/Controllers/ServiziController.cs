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
    public class ServiziController : Controller
    {   

        //aggiungo un nuovo servizio al database 
        public void AggiungiServizio(string descrizione, decimal prezzo)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString.ToString();
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Servizi (Descrizione, Prezzo) VALUES (@Descrizione, @Prezzo)",
                    connection);

                cmd.Parameters.AddWithValue("@Descrizione", descrizione);
                cmd.Parameters.AddWithValue("@Prezzo", prezzo);

                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        //associo il servizio a una prenotazione
        public void AggiungiServizioPrenotato(DateTime data, int quantita, decimal prezzo, int idPrenotazione, int idServizio)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString.ToString();
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO ServiziPrenotati (Data, Quantita, Prezzo, IDPrenotazione, IDServizio) VALUES (@Data, @Quantita, @Prezzo, @IDPrenotazione, @IDServizio)",
                    connection);

                cmd.Parameters.AddWithValue("@Data", data);
                cmd.Parameters.AddWithValue("@Quantita", quantita);
                cmd.Parameters.AddWithValue("@Prezzo", prezzo);
                cmd.Parameters.AddWithValue("@IDPrenotazione", idPrenotazione);
                cmd.Parameters.AddWithValue("@IDServizio", idServizio);

                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        //**************************** CRUD ****************************

        // visualizzo tutti i servizi
        public ActionResult Index()
        {
            List<Servizio> servizi = new List<Servizio>();
            string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString.ToString();
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Servizi", connection);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Servizio servizio = new Servizio
                    {
                        ID = reader.GetInt32(0),
                        Descrizione = reader.GetString(1),
                        Prezzo = reader.GetDecimal(2)
                    };

                    servizi.Add(servizio);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                connection.Close();
            }

            return View(servizi);
        }

        // GET: Servizi/Crea
        public ActionResult Crea()
        {
            return View();
        }

        // POST: creo servizio 
        [HttpPost]
        public ActionResult Crea(Servizio servizio)
        {
            if (ModelState.IsValid)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString.ToString();
                SqlConnection connection = new SqlConnection(connectionString);

                try
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(
                        "INSERT INTO Servizi (Descrizione, Prezzo) VALUES (@Descrizione, @Prezzo)",
                        connection);

                    cmd.Parameters.AddWithValue("@Descrizione", servizio.Descrizione);
                    cmd.Parameters.AddWithValue("@Prezzo", servizio.Prezzo);

                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    connection.Close();
                }

                return RedirectToAction("Index");
            }

            return View(servizio);
        }

        // GET: Servizi/Modifica/5
        public ActionResult Modifica(int id)
        {
            Servizio servizio = null;
            string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString.ToString();
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Servizi WHERE ID = @ID", connection);
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    servizio = new Servizio
                    {
                        ID = reader.GetInt32(0),
                        Descrizione = reader.GetString(1),
                        Prezzo = reader.GetDecimal(2)
                    };
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                connection.Close();
            }

            if (servizio == null)
            {
                return HttpNotFound();
            }

            return View(servizio);
        }

        //POST: modifico un servizio

        [HttpPost]
        public ActionResult Modifica(Servizio servizio)
        {
            if (ModelState.IsValid)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString.ToString();
                SqlConnection connection = new SqlConnection(connectionString);

                try
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(
                        "UPDATE Servizi SET Descrizione = @Descrizione, Prezzo = @Prezzo WHERE ID = @ID",
                        connection);

                    cmd.Parameters.AddWithValue("@Descrizione", servizio.Descrizione);
                    cmd.Parameters.AddWithValue("@Prezzo", servizio.Prezzo);
                    cmd.Parameters.AddWithValue("@ID", servizio.ID);

                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    connection.Close();
                }

                return RedirectToAction("Index");
            }

            return View(servizio);
        }

        // GET: Servizi/Cancella/5
        public ActionResult Cancella(int id)
        {
            Servizio servizio = null;
            string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString.ToString();
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Servizi WHERE ID = @ID", connection);
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    servizio = new Servizio
                    {
                        ID = reader.GetInt32(0),
                        Descrizione = reader.GetString(1),
                        Prezzo = reader.GetDecimal(2)
                    };
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                connection.Close();
            }

            if (servizio == null)
            {
                return HttpNotFound();
            }

            return View(servizio);
        }

        // POST: Cancello un servizio

        [HttpPost, ActionName("Cancella")]
        public ActionResult CancellaConfermato(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString.ToString();
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Servizi WHERE ID = @ID", connection);
                cmd.Parameters.AddWithValue("@ID", id);

                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                connection.Close();
            }

            return RedirectToAction("Index");
        }


    }
}

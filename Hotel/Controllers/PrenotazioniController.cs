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
    public class PrenotazioniController : Controller
    {

        //********************************** INDEX PRENOTAZIONI ************************************//


        // GET: Prenotazioni - Visualizza tutte le prenotazioni presenti nel database 
        public ActionResult Index()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString.ToString();
            SqlConnection connection = new SqlConnection(connectionString);
            List<Prenotazione> prenotazioni = new List<Prenotazione>();

            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Prenotazioni", connection);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Prenotazione prenotazione = new Prenotazione
                    {
                        DataPrenotazione = (DateTime)reader["DataPrenotazione"],
                        NumeroProgressivo = (int)reader["NumeroProgressivo"],
                        Anno = (int)reader["Anno"],
                        PeriodoSoggiornoDal = (DateTime)reader["PeriodoSoggiornoDal"],
                        PeriodoSoggiornoAl = (DateTime)reader["PeriodoSoggiornoAl"],
                        Caparra = (decimal)reader["Caparra"],
                        Tariffa = (decimal)reader["Tariffa"],
                        Dettagli = (string)reader["Dettagli"],
                        CodiceFiscaleCliente = (string)reader["CodiceFiscaleCliente"],
                        NumeroCamera = (int)reader["NumeroCamera"]
                    };
                    prenotazioni.Add(prenotazione);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Content("Si è verificato un errore: " + e.Message);
            }
            finally
            {
                connection.Close();
            }

            return View(prenotazioni);
        }

        //********************************** AGGIUNGI PRENOTAZIONE ************************************//


        // GET: Prenotazioni/AggiungiPrenotazione
        public ActionResult AggiungiPrenotazione()
        {
        
            ViewBag.DettagliOptions = new List<string> // creo lista di opzioni per il campo Dettagli 
            {
                "Mezza pensione",
                "Pensione completa",
                "Pernottamento con prima colazione"
            };

            ViewBag.AnnoOptions = Enumerable.Range(2024, 76).ToList(); // creo lista di anni da 2024 a 2100 per il campo Anno


            return View(new Prenotazione());
        }

        // POST: Prenotazioni/AggiungiPrenotazione
        [HttpPost]
        public ActionResult AggiungiPrenotazione(Prenotazione prenotazione)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString.ToString();
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Prenotazioni (DataPrenotazione, NumeroProgressivo, Anno, PeriodoSoggiornoDal, PeriodoSoggiornoAl, Caparra, Tariffa, Dettagli, CodiceFiscaleCliente, NumeroCamera) " +
                    "VALUES (@DataPrenotazione, @NumeroProgressivo, @Anno, @PeriodoSoggiornoDal, @PeriodoSoggiornoAl, @Caparra, @Tariffa, @Dettagli, @CodiceFiscaleCliente, @NumeroCamera)",
                    connection);

                cmd.Parameters.AddWithValue("@DataPrenotazione", prenotazione.DataPrenotazione);
                cmd.Parameters.AddWithValue("@NumeroProgressivo", prenotazione.NumeroProgressivo);
                cmd.Parameters.AddWithValue("@Anno", prenotazione.Anno);
                cmd.Parameters.AddWithValue("@PeriodoSoggiornoDal", prenotazione.PeriodoSoggiornoDal);
                cmd.Parameters.AddWithValue("@PeriodoSoggiornoAl", prenotazione.PeriodoSoggiornoAl);
                cmd.Parameters.AddWithValue("@Caparra", prenotazione.Caparra);
                cmd.Parameters.AddWithValue("@Tariffa", prenotazione.Tariffa);
                cmd.Parameters.AddWithValue("@Dettagli", prenotazione.Dettagli);
                cmd.Parameters.AddWithValue("@CodiceFiscaleCliente", prenotazione.CodiceFiscaleCliente);
                cmd.Parameters.AddWithValue("@NumeroCamera", prenotazione.NumeroCamera);

                cmd.ExecuteNonQuery();

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Content("Si è verificato un errore: " + e.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        //********************************** OPERAZIONI CRUD ************************************//

        // GET: Prenotazioni/Edit/5
        public ActionResult Edit(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString.ToString();
            SqlConnection connection = new SqlConnection(connectionString);
            Prenotazione prenotazione = null; // inizializzo prenotazione a null per evitare errori di compilazione 

            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Prenotazioni WHERE NumeroProgressivo = @NumeroProgressivo", connection);
                cmd.Parameters.AddWithValue("@NumeroProgressivo", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    prenotazione = new Prenotazione
                    {
                        DataPrenotazione = (DateTime)reader["DataPrenotazione"],
                        NumeroProgressivo = (int)reader["NumeroProgressivo"],
                        Anno = (int)reader["Anno"],
                        PeriodoSoggiornoDal = (DateTime)reader["PeriodoSoggiornoDal"],
                        PeriodoSoggiornoAl = (DateTime)reader["PeriodoSoggiornoAl"],
                        Caparra = (decimal)reader["Caparra"],
                        Tariffa = (decimal)reader["Tariffa"],
                        Dettagli = (string)reader["Dettagli"],
                        CodiceFiscaleCliente = (string)reader["CodiceFiscaleCliente"],
                        NumeroCamera = (int)reader["NumeroCamera"]
                    };
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Content("Si è verificato un errore: " + e.Message);
            }
            finally
            {
                connection.Close();
            }

            // se la prenotazione non esiste, ritorno HttpNotFound - errore 404 

            if (prenotazione == null)
            {
                return HttpNotFound();
            }

            return View(prenotazione);
        }




    }
}
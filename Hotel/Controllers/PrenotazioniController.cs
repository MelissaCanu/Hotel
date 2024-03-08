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

    // Creo una lista per contenere i servizi
    List<Servizio> servizi = new List<Servizio>();

   
    string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString.ToString();
    using (SqlConnection con = new SqlConnection(connectionString))
    {
        
        using (SqlCommand cmd = new SqlCommand("SELECT * FROM Servizi", con))
        {
          
            con.Open();

          
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
      
                while (rdr.Read())
                {
                    // Creo un nuovo oggetto Servizio per ogni riga
                    Servizio servizio = new Servizio
                    {
                        ID = Convert.ToInt32(rdr["ID"]),
                        Descrizione = rdr["Descrizione"].ToString(),
                        Prezzo = Convert.ToDecimal(rdr["Prezzo"])
                    };

                    // Aggiungo l'oggetto Servizio alla lista
                    servizi.Add(servizio);
                }
            }
        }
    }

    // Passo la lista di servizi alla vista
    ViewBag.Servizi = servizi;

    // Ritorno la vista con un nuovo modello Prenotazione
    return View(new Prenotazione());
}

        // POST: Prenotazioni/AggiungiPrenotazione + aggiunta servizi prenotati 
        [HttpPost]
        public ActionResult AggiungiPrenotazione(Prenotazione prenotazione, List<int> ServiziPrenotati)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString.ToString();
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                // Creo un nuovo record nella tabella Prenotazioni
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Prenotazioni (DataPrenotazione, NumeroProgressivo, Anno, PeriodoSoggiornoDal, PeriodoSoggiornoAl, Caparra, Tariffa, Dettagli, CodiceFiscaleCliente, NumeroCamera) " +
                    "OUTPUT INSERTED.ID " +
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

                int idPrenotazione = (int)cmd.ExecuteScalar();

                // Creo un nuovo record nella tabella ServiziPrenotati per ogni servizio selezionato
                foreach (var idServizio in ServiziPrenotati)
                {
                    cmd = new SqlCommand(
                        "INSERT INTO ServiziPrenotati (Data, Quantita, Prezzo, IDPrenotazione, IDServizio) " +
                        "VALUES (@Data, @Quantita, @Prezzo, @IDPrenotazione, @IDServizio)",
                        connection);

                    cmd.Parameters.AddWithValue("@Data", DateTime.Now); 
                    cmd.Parameters.AddWithValue("@Quantita", 1); 
                    cmd.Parameters.AddWithValue("@Prezzo", 100); 
                    cmd.Parameters.AddWithValue("@IDPrenotazione", idPrenotazione);
                    cmd.Parameters.AddWithValue("@IDServizio", idServizio);

                    cmd.ExecuteNonQuery();
                }

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

        // POST: Prenotazioni/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Prenotazione prenotazione)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString.ToString();
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(
                    "UPDATE Prenotazioni SET DataPrenotazione = @DataPrenotazione, Anno = @Anno, PeriodoSoggiornoDal = @PeriodoSoggiornoDal, PeriodoSoggiornoAl = @PeriodoSoggiornoAl, Caparra = @Caparra, Tariffa = @Tariffa, Dettagli = @Dettagli, CodiceFiscaleCliente = @CodiceFiscaleCliente, NumeroCamera = @NumeroCamera WHERE NumeroProgressivo = @NumeroProgressivo",
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

            return RedirectToAction("Index");
        }


        // GET: Prenotazioni/Details/5 + aggiunta servizi
        public ActionResult Details(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString.ToString();
            SqlConnection connection = new SqlConnection(connectionString);

            //inizializzo prenotazione e importototale a null, creo lista serviziprenotati
            Prenotazione prenotazione = null;
            List<ServizioPrenotato> serviziPrenotati = new List<ServizioPrenotato>();
            decimal importoTotale = 0;

            try
            {
                connection.Open();
                
                SqlCommand cmd = new SqlCommand("SELECT * FROM Prenotazioni WHERE NumeroProgressivo = @NumeroProgressivo", connection);
                cmd.Parameters.AddWithValue("@NumeroProgressivo", id); 
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {   
                    //creo oggetto prenotazione e lo popolo con i dati del reader
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

                reader.Close();

                //creo query per selezionare i servizi prenotati associati alla prenotazione

                cmd = new SqlCommand("SELECT * FROM ServiziPrenotati WHERE IDPrenotazione = @IDPrenotazione", connection);
                cmd.Parameters.AddWithValue("@IDPrenotazione", id);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {   
                    //creo oggetto serviziPrenotati e lo popolo con i dati del reader
                    var servizioPrenotato = new ServizioPrenotato
                    {
                        Data = (DateTime)reader["Data"],
                        Quantita = (int)reader["Quantita"],
                        Prezzo = (decimal)reader["Prezzo"],
                        IDPrenotazione = (int)reader["IDPrenotazione"]
                    };

                    serviziPrenotati.Add(servizioPrenotato);
                    importoTotale += servizioPrenotato.Quantita * servizioPrenotato.Prezzo; //calcolo importo totale
                }

                importoTotale += prenotazione.Tariffa - prenotazione.Caparra; //aggiungo tariffa e sottraggo caparra
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

            if (prenotazione == null)
            {
                return HttpNotFound();
            }
            //passo i dati alla view tramite ViewBag 
            ViewBag.ImportoTotale = importoTotale;
            ViewBag.ServiziPrenotati = serviziPrenotati;

            //ritorno la view con i dati della prenotazione e i servizi prenotati associati 
            return View(prenotazione);
        }


        // GET: Prenotazioni/Delete/5
        public ActionResult Delete(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString.ToString();
            SqlConnection connection = new SqlConnection(connectionString);
            Prenotazione prenotazione = null;

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

            if (prenotazione == null)
            {
                return HttpNotFound();
            }

            return View(prenotazione);
        }

        // POST: Prenotazioni/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString.ToString();
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Prenotazioni WHERE NumeroProgressivo = @NumeroProgressivo", connection);
                cmd.Parameters.AddWithValue("@NumeroProgressivo", id);
                cmd.ExecuteNonQuery();
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

            return RedirectToAction("Index");
        }

        //********************************** CALCOLO TOTALE + SERVIZI ************************************//
        //(aggiunta servizi/associazione servizi-prenotazione in ServiziController)

        public decimal CalcolaImportoTotale(int idPrenotazione)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString.ToString();
            SqlConnection connection = new SqlConnection(connectionString);
            decimal importoTotale = 0; // inizializzo importoTotale a 0

            try
            {
                connection.Open();

        //calcolo importo totale sommando la tariffa della prenotazione, sottraendo la caparra e sommando il prezzo dei servizi prenotati
                SqlCommand cmd = new SqlCommand(
                    "SELECT (Prenotazioni.Tariffa - Prenotazioni.Caparra + SUM(ServiziPrenotati.Prezzo * ServiziPrenotati.Quantita)) AS ImportoTotale " +
                    "FROM Prenotazioni INNER JOIN ServiziPrenotati ON Prenotazioni.ID = ServiziPrenotati.IDPrenotazione " +
                    "WHERE Prenotazioni.ID = @IDPrenotazione",
                    connection);

                cmd.Parameters.AddWithValue("@IDPrenotazione", idPrenotazione);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {   
                    importoTotale = (decimal)reader["ImportoTotale"];
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

            return importoTotale;
        }




    }
}
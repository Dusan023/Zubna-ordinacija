using Klase;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlojPodataka
{
    public class TerminDB
    {
        private readonly string _connectionString;

        public TerminDB()
        {
            _connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=Zubna_Ordinacija;Integrated Security=True";
        }

        public List<Termin> GetAll()
        {
            var lista = new List<Termin>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Termin", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Termin
                        {
                            IDTermina = (int)reader["IDTermina"],
                            Datum = (DateTime)reader["Datum"],
                            Vreme = (TimeSpan)reader["Vreme"],
                            VrstaUsluge = reader["VrstaUsluge"].ToString(),
                            IDPacijenta = (int)reader["IDPacijenta"],
                            IDZubara = (int)reader["IDZubara"]
                        });
                    }
                }
            }

            return lista;
        }

        public void Insert(Termin termin)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    "INSERT INTO Termin(Datum, Vreme, VrstaUsluge, IDPacijenta, IDZubara) " +
                    "VALUES(@Datum, @Vreme, @VrstaUsluge, @IDPacijenta, @IDZubara)", connection);

                command.Parameters.AddWithValue("@Datum", termin.Datum);
                command.Parameters.AddWithValue("@Vreme", termin.Vreme);
                command.Parameters.AddWithValue("@VrstaUsluge", termin.VrstaUsluge);
                command.Parameters.AddWithValue("@IDPacijenta", termin.IDPacijenta);
                command.Parameters.AddWithValue("@IDZubara", termin.IDZubara);

                command.ExecuteNonQuery();
            }
        }

        public void Update(Termin termin)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    "UPDATE Termin SET Datum=@Datum, Vreme=@Vreme, VrstaUsluge=@VrstaUsluge, " +
                    "IDPacijenta=@IDPacijenta, IDZubara=@IDZubara WHERE IDTermina=@Id", connection);

                command.Parameters.AddWithValue("@Id", termin.IDTermina);
                command.Parameters.AddWithValue("@Datum", termin.Datum);
                command.Parameters.AddWithValue("@Vreme", termin.Vreme);
                command.Parameters.AddWithValue("@VrstaUsluge", termin.VrstaUsluge);
                command.Parameters.AddWithValue("@IDPacijenta", termin.IDPacijenta);
                command.Parameters.AddWithValue("@IDZubara", termin.IDZubara);

                command.ExecuteNonQuery();
            }
        }

        public void Delete(int idTermina)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("DELETE FROM Termin WHERE IDTermina=@Id", connection);
                command.Parameters.AddWithValue("@Id", idTermina);
                command.ExecuteNonQuery();
            }
        }

        public List<Termin> GetTerminiFromPacijent(int ID)
        {
            var list = new List<Termin>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Termin where IDPacijenta=@IDPacijenta", connection);
                command.Parameters.AddWithValue("@IDPacijenta", ID);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Termin
                        {
                            IDTermina = (int)reader["IDTermina"],
                            Datum = (DateTime)reader["Datum"],
                            Vreme = (TimeSpan)reader["Vreme"],
                            VrstaUsluge = reader["VrstaUsluge"].ToString(),
                            IDPacijenta = (int)reader["IDPacijenta"],
                            IDZubara = (int)reader["IDZubara"]
                        });
                    }
                }
            }
            return list;

        }
    }
}

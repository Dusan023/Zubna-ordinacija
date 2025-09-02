using Klase;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlojPodataka
{
    public class PregledDB
    {

        private readonly string _connectionString;

        public PregledDB()
        {
            _connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=Zubna_Ordinacija;Integrated Security=True";
        }
        public List<Pregled> GetAll()
        {
            var lista = new List<Pregled>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Pregled", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Pregled
                        {
                            IDPregleda = (int)reader["IDPregleda"],
                            DatumSledecePosete = (DateTime)reader["DatumSledecePosete"],
                            IDTermina = (int)reader["IDTermina"],
                            IDLeka = (int)reader["IDLeka"]
                        });
                    }
                }
            }

            return lista;
        }

        public void Insert(Pregled pregled)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    "INSERT INTO Pregled(DatumSledecePosete, IDTermina, IDLeka) VALUES(@Datum, @Termin, @Lek)", connection);

                command.Parameters.AddWithValue("@Datum", pregled.DatumSledecePosete);
                command.Parameters.AddWithValue("@Termin", pregled.IDTermina);
                command.Parameters.AddWithValue("@Lek", pregled.IDLeka);

                command.ExecuteNonQuery();
            }
        }

        public void Update(Pregled pregled)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    "UPDATE Pregled SET DatumSledecePosete=@Datum, IDTermina=@Termin, IDLeka=@Lek WHERE IDPregleda=@Id", connection);

                command.Parameters.AddWithValue("@Id", pregled.IDPregleda);
                command.Parameters.AddWithValue("@Datum", pregled.DatumSledecePosete);
                command.Parameters.AddWithValue("@Termin", pregled.IDTermina);
                command.Parameters.AddWithValue("@Lek", pregled.IDLeka);

                command.ExecuteNonQuery();
            }
        }

        public void Delete(int idPregleda)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("DELETE FROM Pregled WHERE IDPregleda=@Id", connection);
                command.Parameters.AddWithValue("@Id", idPregleda);
                command.ExecuteNonQuery();
            }
        }

        public List<Pregled> GetTerminiFromPacijent(int ID)
        {
            var list = new List<Pregled>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Pregled where IDTermina=@IDTermina", connection);
                command.Parameters.AddWithValue("@IDTermina", ID);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Pregled
                        {
                            IDPregleda = (int)reader["IDPregleda"],
                            DatumSledecePosete = (DateTime)reader["DatumSledecePosete"],
                            IDTermina = (int)reader["IDTermina"],
                            IDLeka = (int)reader["IDLeka"]
                        });
                    }
                }
            }
            return list;

        }

        public Termin GetTerminWithPregled(int id)
        {
            Termin termin = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Termin WHERE IDTermina = @IDTermina", connection);
                command.Parameters.AddWithValue("@IDTermina", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        termin = new Termin
                        {
                            IDTermina = (int)reader["IDTermina"],
                            Datum = (DateTime)reader["Datum"],
                            Vreme = (TimeSpan)reader["Vreme"],
                            VrstaUsluge = reader["VrstaUsluge"].ToString(),
                            IDPacijenta = (int)reader["IDPacijenta"],
                            IDZubara = (int)reader["IDZubara"]
                        };
                    }
                }
            }

            return termin;
        }

    }
}
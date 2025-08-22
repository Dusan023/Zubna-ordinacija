using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SlojPodataka.Klase;

namespace SlojPodataka //
{
    public class PacijentiDB
    {
        private readonly string _connectionString;

        public PacijentiDB()
        {
            _connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=Zubna_Ordinacija;Integrated Security=True";
        }

        public List<Pacijenti> GetAll()
        {
            var pacijenti = new List<Pacijenti>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Pacijenti", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    pacijenti.Add(new Pacijenti
                    {
                        IDPacijenta = (int)reader["IDPacijenta"],
                        Ime = reader["Ime"].ToString(),
                        Prezime = reader["Prezime"].ToString(),
                        JMBG = reader["JMBG"].ToString(),
                        BrojTelefona = reader["BrojTelefona"].ToString(),
                        Pol = reader["Pol"].ToString(),
                        Alergije = reader["Alergije"].ToString(),
                        Trudnoca = reader["Trudnoca"] != DBNull.Value && (bool)reader["Trudnoca"],
                        BrojZuba = reader["BrojZuba"] != DBNull.Value ? (int)reader["BrojZuba"] : 0,
                        IDZubara = reader["IDZubara"] != DBNull.Value ? (int)reader["IDZubara"] : 0
                    });
                }
            }
            return pacijenti;
        }

        public void Insert(Pacijenti pacijent)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    @"INSERT INTO Pacijenti (Ime, Prezime, JMBG, BrojTelefona, Pol, Alergije, Trudnoca, BrojZuba, IDZubara) 
                      VALUES (@Ime, @Prezime, @JMBG, @BrojTelefona, @Pol, @Alergije, @Trudnoca, @BrojZuba, @IDZubara)", conn);

                cmd.Parameters.AddWithValue("@Ime", pacijent.Ime);
                cmd.Parameters.AddWithValue("@Prezime", pacijent.Prezime);
                cmd.Parameters.AddWithValue("@JMBG", pacijent.JMBG);
                cmd.Parameters.AddWithValue("@BrojTelefona", pacijent.BrojTelefona);
                cmd.Parameters.AddWithValue("@Pol", pacijent.Pol);
                cmd.Parameters.AddWithValue("@Alergije", pacijent.Alergije ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Trudnoca", pacijent.Trudnoca);
                cmd.Parameters.AddWithValue("@BrojZuba", pacijent.BrojZuba);
                cmd.Parameters.AddWithValue("@IDZubara", pacijent.IDZubara);

                cmd.ExecuteNonQuery();
            }
        }

        public void Update(Pacijenti pacijent)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    @"UPDATE Pacijenti
                      SET Ime=@Ime, Prezime=@Prezime, JMBG=@JMBG, BrojTelefona=@BrojTelefona, Pol=@Pol, 
                          Alergije=@Alergije, Trudnoca=@Trudnoca, BrojZuba=@BrojZuba, IDZubara=@IDZubara 
                      WHERE IDPacijenta=@IDPacijenta", conn);

                cmd.Parameters.AddWithValue("@IDPacijenta", pacijent.IDPacijenta);
                cmd.Parameters.AddWithValue("@Ime", pacijent.Ime);
                cmd.Parameters.AddWithValue("@Prezime", pacijent.Prezime);
                cmd.Parameters.AddWithValue("@JMBG", pacijent.JMBG);
                cmd.Parameters.AddWithValue("@BrojTelefona", pacijent.BrojTelefona);
                cmd.Parameters.AddWithValue("@Pol", pacijent.Pol);
                cmd.Parameters.AddWithValue("@Alergije", pacijent.Alergije ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Trudnoca", pacijent.Trudnoca);
                cmd.Parameters.AddWithValue("@BrojZuba", pacijent.BrojZuba);
                cmd.Parameters.AddWithValue("@IDZubara", pacijent.IDZubara);

                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Pacijenti WHERE IDPacijenta=@IDPacijenta", conn);
                cmd.Parameters.AddWithValue("@IDPacijenta", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}

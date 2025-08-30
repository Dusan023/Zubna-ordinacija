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
                SqlCommand cmd = new SqlCommand("SELECT * FROM Pacijent", conn);
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
                    @"INSERT INTO Pacijent (Ime, Prezime, JMBG, BrojTelefona, Pol, Alergije, Trudnoca, BrojZuba, IDZubara) 
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

                try
                { cmd.ExecuteNonQuery(); } 
                catch (Exception e) {
                    Console.WriteLine(e);
                }
                
            }
        }

        public void Update(Pacijenti pacijent)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    @"UPDATE Pacijent
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

                try
                { cmd.ExecuteNonQuery(); }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Pacijent WHERE IDPacijenta=@IDPacijenta", conn);
                cmd.Parameters.AddWithValue("@IDPacijenta", id);
                cmd.ExecuteNonQuery();
            }
        }

        public bool checkIfJMBGExists(string JMBG) {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT TOP 1 * FROM Pacijent WHERE JMBG = @jmbg", conn);
                cmd.Parameters.AddWithValue("@jmbg", JMBG);
                SqlDataReader nadjen = cmd.ExecuteReader();
                
                if (nadjen.Read())
                {
                    return true;
                }
                else return false;
            }
        }
    }
}

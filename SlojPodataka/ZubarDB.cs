using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlojPodataka.Klase;

namespace SlojPodataka //
{
    public class ZubarDB
    {
        private readonly string _connectionString;

        public ZubarDB()
        {
            _connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=Zubna_Ordinacija;Integrated Security=True";
        }
        public List<Zubar> GetAll()
        {
            var lista = new List<Zubar>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Zubar", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new Zubar
                    {
                        IDZubara = (int)reader["IDZubara"],
                        Ime = reader["Ime"].ToString(),
                        Prezime = reader["Prezime"].ToString(),
                        JMBG = reader["JMBG"].ToString(),
                        BrojTelefona = reader["BrojTelefona"].ToString(),
                        Email = reader["Email"].ToString(),
                        isDeleted = (bool)reader["isDeleted"]
                });
                }
            }
            return lista;
        }

        public List<Zubar> GetAllActiveZubar()
        {
            var lista = new List<Zubar>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Zubar where IsDeleted = 0", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new Zubar
                    {
                        IDZubara = (int)reader["IDZubara"],
                        Ime = reader["Ime"].ToString(),
                        Prezime = reader["Prezime"].ToString(),
                        JMBG = reader["JMBG"].ToString(),
                        BrojTelefona = reader["BrojTelefona"].ToString(),
                        Email = reader["Email"].ToString()
                    });
                }
            }
            return lista;
        }
        public void Insert(Zubar zubar)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Zubar (Ime, Prezime, JMBG, BrojTelefona, Email) " +
                    "VALUES (@Ime, @Prezime, @JMBG, @BrojTelefona, @Email)", conn);

                cmd.Parameters.AddWithValue("@Ime", zubar.Ime);
                cmd.Parameters.AddWithValue("@Prezime", zubar.Prezime);
                cmd.Parameters.AddWithValue("@JMBG", zubar.JMBG);
                cmd.Parameters.AddWithValue("@BrojTelefona", zubar.BrojTelefona);
                cmd.Parameters.AddWithValue("@Email", zubar.Email);

                cmd.ExecuteNonQuery();
            }
        }

        public void Update(Zubar zubar)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "UPDATE Zubar SET Ime=@Ime, Prezime=@Prezime, JMBG=@JMBG, " +
                    "BrojTelefona=@BrojTelefona, Email=@Email WHERE IDZubara=@IDZubara", conn);

                cmd.Parameters.AddWithValue("@IDZubara", zubar.IDZubara);
                cmd.Parameters.AddWithValue("@Ime", zubar.Ime);
                cmd.Parameters.AddWithValue("@Prezime", zubar.Prezime);
                cmd.Parameters.AddWithValue("@JMBG", zubar.JMBG);
                cmd.Parameters.AddWithValue("@BrojTelefona", zubar.BrojTelefona);
                cmd.Parameters.AddWithValue("@Email", zubar.Email);

                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Zubar WHERE IDZubara=@IDZubara", conn);
                cmd.Parameters.AddWithValue("@IDZubara", id);
                cmd.ExecuteNonQuery();
            }
        }

        public void SoftDelete(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Zubar SET isDeleted = 1 WHERE IDZubara = @IDZubara",conn);              
                cmd.Parameters.AddWithValue("@IDZubara", id);
               int check =  cmd.ExecuteNonQuery();
            }
        }

        public bool checkIfJMBGExists(string JMBG)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT TOP 1 * FROM Zubar WHERE JMBG = @jmbg", conn);
                cmd.Parameters.AddWithValue("@jmbg", JMBG);
                SqlDataReader nadjen = cmd.ExecuteReader();

                if (nadjen.Read())
                {
                    return true;
                }
                else return false;
            }
        }

        public bool checkIfEmailForZubarExists(string email)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT TOP 1 * FROM Zubar WHERE Email = @Email", conn);
                cmd.Parameters.AddWithValue("@Email", email);
                SqlDataReader nadjen = cmd.ExecuteReader();

                if (nadjen.Read())
                {
                    return true;
                }
                else return false;
            }
        }

        public int GetAppointmentCountFromDentist(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                int brojTermina = 0;
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Termin WHERE IDZubara = @ZubarId", conn);
                cmd.Parameters.AddWithValue("@ZubarId", id);
                
                brojTermina = (int)cmd.ExecuteScalar();

                return brojTermina;

            }
        }

        public Zubar findZubaraByID(int id)
        {
            Zubar zubar = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Zubar WHERE IDZubara = @IDZubara", conn);
                cmd.Parameters.AddWithValue("@IDZubara", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    zubar = new Zubar
                    {
                        IDZubara = (int)reader["IDZubara"],
                        Ime = reader["Ime"].ToString(),
                        Prezime = reader["Prezime"].ToString(),
                        JMBG = reader["JMBG"].ToString(),
                        BrojTelefona = reader["BrojTelefona"].ToString(),
                        Email = reader["Email"].ToString(),
                        isDeleted = (bool)reader["isDeleted"]
                    };
                }
            }
            return zubar;
        }   
    }
}
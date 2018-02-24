using System;
using System.Data;
using System.Data.SqlClient;

namespace SimpleBot.Repo
{
    public class UserSqlRepo : IUserRepo
    {
        string _connectionString;
        public UserSqlRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        public UserProfile GetProfile(string id)
        {
            UserProfile profile = default(UserProfile);
            using (var conn = new SqlConnection())
            {
                var cmd = new SqlCommand();
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                cmd.CommandText = "SELECT * FROM USERPROFILE WHERE Id = @id";
                cmd.Connection = new SqlConnection(_connectionString);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    profile = new UserProfile()
                    {
                        Id = reader["Id"].ToString(),
                        Visitas = Convert.ToInt32(reader["Visitas"])
                    };
                }
                reader.Close();
            }
            return profile;
        }

        public void SalvarHistorico(Message message)
        {
            using (var conn = new SqlConnection())
            {
                string sql = "INSERT INTO Message(User,Text) VALUES(@user,@text)";
                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@user", SqlDbType.VarChar, 50).Value = message.User;
                cmd.Parameters.Add("@text", SqlDbType.VarChar, 50).Value = message.Text;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void SetProfile(string id, UserProfile profile)
        {
            var profileOld = GetProfile(id);
            if (profile != null)
            {
                using (var conn = new SqlConnection())
                {
                    string sql = "UPDATE INTO UserProfile SET Visitas = @visita WHERE id = @id";
                    var cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.Add("@visita", SqlDbType.Int).Value = profile.Visitas;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = profile.Id;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            else
            {
                using (var conn = new SqlConnection())
                {
                    string sql = "INSERT INTO UserProfile VALUES(@Visitas)";
                    var cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.Add("@Visitas", SqlDbType.Int).Value = profile.Visitas;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace All4Ole_Server.Model
{
    public class Manager : IManager
    {
        private readonly string connStr = "server=localhost;user=root;database=all4oledb;port=3306;password=Mcgilad1l@";
        private User GetUser(string userName)
        {

            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                string sql = $"SELECT * FROM users WHERE user_name='{userName}'";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                User user = new User();
                while (rdr.Read())
                {
                    user = MakeUserFromMySqlObject(rdr);
                }
                conn.Close();
                rdr.Close();
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.WriteLine("Done.");
            return null;
        }

        //makes user from mysql reader
        public User MakeUserFromMySqlObject(MySqlDataReader reader)
        {
            User user = new User()
            {
                UserName = reader["user_name"].ToString(),
                FirstName = reader["first_name"].ToString(),
                LastName = reader["last_name"].ToString(),
                Email = reader["email"].ToString(),
                Password = reader["password"].ToString(),
                Language = reader["language"].ToString(),
                Phone = reader["phone"].ToString(),
                PreviousCountry = reader["origin_country"].ToString(),
                ResidentialArea = reader["residential_area"].ToString(),
                Hobbies = (int)reader["hobbies"],
                MaritalStatus = reader.GetBoolean("marital_status") ? 1 : 0,
                HasChildrenUnder18 = reader.GetBoolean("has_little_children") ? 1 : 0,
                Help = (int)reader["help"]
            };
            return user;
        }


        public List<User> LookForHelp(string userName, int help)
        {
            User user = GetUser(userName);
            MySqlConnection conn = new MySqlConnection(connStr);
            List<User> users = new List<User>();
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                string sql = $"SELECT * FROM users WHERE user_name!='{userName}' AND language='{user.Language}' AND residential_area='{user.ResidentialArea}' AND help&{help}>0";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    users.Add(MakeUserFromMySqlObject(rdr));
                }
            }
            catch (Exception)
            {
                return null;
            }

            Console.WriteLine("Done.");
            return users;
        }


        public string Login(string userName, string password)
        {
            User user = GetUser(userName);
            if (user == null || password != user.Password)
            {
                return "can't login";
            }
            return "can login";
        }


        public List<User> FindFriendsForHobbies(string userName, int hobbies)
        {
            User user = GetUser(userName);
            MySqlConnection conn = new MySqlConnection(connStr);
            List<User> users = new List<User>();
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                string sql = $"SELECT * FROM users WHERE user_name!='{userName}' AND language='{user.Language}' AND residential_area='{user.ResidentialArea}' AND hobbies &{hobbies}>0";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    users.Add(MakeUserFromMySqlObject(rdr));
                }
            }
            catch (Exception)
            {
                return null;
            }
            return users;
        }

        public List<User> PeopleLikeMe(string userName)
        {
            User user = GetUser(userName);
            string forHobbies = IsPowerOf2(user.Hobbies) ? ">0" : ">1";
            MySqlConnection conn = new MySqlConnection(connStr);
            List<User> users = new List<User>();
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                string sql = $"SELECT * FROM users WHERE user_name!='{userName}' AND language='{user.Language}' AND residential_area='{user.ResidentialArea}' And origin_country='{user.PreviousCountry}' AND hobbies&{user.Hobbies}" + forHobbies +
                    $" AND marital_status='{user.MaritalStatus}' AND has_little_children='{user.HasChildrenUnder18}'";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    users.Add(MakeUserFromMySqlObject(rdr));
                }
            }
            catch (Exception)
            {
                return null;
            }
            return users;
        }

        private bool IsPowerOf2(int number)
        {
            return (number & (number - 1)) == 0;
        }


        public string SetHelp(string userName, int help)
        {
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();


                string sql = $"UPDATE users SET help='{help}' WHERE user_name='{userName}'";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            Console.WriteLine("Done.");
            return "success";
        }

        // Insert user to mysql
        public string InsertUser(User user)
        {
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                string query = "INSERT INTO all4oledb.users(user_name, password, first_name, last_name, phone,  email, origin_country, residential_area, language, marital_status, has_little_children, hobbies, help) VALUES (?user_name,?password,?first_name,?last_name,?phone,?email,?origin_country,?residential_area,?language,?marital_status,?has_little_children,?hobbies,?help);";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                AddParametersToQuery(cmd, user);
                cmd.ExecuteNonQuery();


            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

            conn.Close();
            return "Good";

        }

        //add the parameters of user to the mysql command
        public void AddParametersToQuery(MySqlCommand cmd, User user)
        {
            cmd.Parameters.Add("?user_name", MySqlDbType.VarChar).Value = user.UserName;
            cmd.Parameters.Add("?password", MySqlDbType.VarChar).Value = user.Password;
            cmd.Parameters.Add("?first_name", MySqlDbType.VarChar).Value = user.FirstName;
            cmd.Parameters.Add("?last_name", MySqlDbType.VarChar).Value = user.LastName;
            cmd.Parameters.Add("?phone", MySqlDbType.VarChar).Value = user.Phone;
            cmd.Parameters.Add("?email", MySqlDbType.VarChar).Value = user.Email;
            cmd.Parameters.Add("?origin_country", MySqlDbType.VarChar).Value = user.PreviousCountry;
            cmd.Parameters.Add("?residential_area", MySqlDbType.VarChar).Value = user.ResidentialArea;
            cmd.Parameters.Add("?language", MySqlDbType.VarChar).Value = user.Language;

            cmd.Parameters.Add("?marital_status", MySqlDbType.Bit).Value = user.MaritalStatus;
            cmd.Parameters.Add("?has_little_children", MySqlDbType.Bit).Value = user.HasChildrenUnder18;

            cmd.Parameters.Add("?hobbies", MySqlDbType.Int32).Value = user.Hobbies;
            cmd.Parameters.Add("?help", MySqlDbType.Int32).Value = user.Help;
        }
    }
}

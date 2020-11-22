using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace All4Ole_Server.Model
{
    public class Manager : IManager
    {
        // the connection string for the mysql server in AWS
        private readonly string connStr = "server=all4oleserverdb.ck3ec6yeqlpt.us-east-2.rds.amazonaws.com; user=admin;database=all4oledb;port=3306;password=Acgilad1s";

        // Gets user from the db
        private User GetUser(string userName)
        {

            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();

                string sql = $"SELECT * FROM users WHERE user_name='{userName}'";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                User user = new User();
                while (rdr.Read())
                {
                    user = MakeUserFromMySqlObject(rdr);
                }
                rdr.Close();
                return user;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }

        // Makes user from mysql reader
        private User MakeUserFromMySqlObject(MySqlDataReader reader)
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

        // Looks for people who are close to the user speaks his language and who can help him
        public List<User> LookForHelp(string userName, int help)
        {
            User user = GetUser(userName);
            string query = $"SELECT * FROM users WHERE user_name!='{userName}' AND language='{user.Language}' AND residential_area='{user.ResidentialArea}' AND help&{help}>0";
            return TakeUsers(query);
        }

        // Tries to login to the user's account - if it's not in the db or password is wrong returns null, else returns the user
        public User Login(string userName, string password)
        {
            User user = GetUser(userName);
            if (user == null || password != user.Password)
            {
                return null;
            }
            return user;
        }

        // Finds users for hobbies (at least one) - that lives near the user and speaks his language
        public List<User> FindFriendsForHobbies(string userName, int hobbies)
        {
            User user = GetUser(userName);
            string query = $"SELECT * FROM users WHERE user_name!='{userName}' AND language='{user.Language}' AND residential_area='{user.ResidentialArea}' AND hobbies &{hobbies}>0";
            return TakeUsers(query);
        }

        // Finds similar users to the user- that live near the user, speak his language, have similar merital status, 
        public List<User> PeopleLikeMe(string userName)
        {
            User user = GetUser(userName);
            // if he has 1 hobby than people needs only 1 hobby similar to him, else they need 2 and above
            string forHobbies = IsPowerOf2(user.Hobbies) ? ">0" : ">1";
            string query = $"SELECT * FROM users WHERE user_name!='{userName}' AND language='{user.Language}' AND residential_area='{user.ResidentialArea}' And origin_country='{user.PreviousCountry}' AND hobbies&{user.Hobbies}" + forHobbies +
                   $" AND marital_status='{user.MaritalStatus}' AND has_little_children='{user.HasChildrenUnder18}'";
            return TakeUsers(query);
        }

        // Gets Users from mysql db
        private List<User> TakeUsers(string query)
        {
            MySqlConnection conn = new MySqlConnection(connStr);
            List<User> users = new List<User>();
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
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

        // Checks if a number has only 1 bit turned on
        private bool IsPowerOf2(int number)
        {
            return (number & (number - 1)) == 0;
        }

        // Sets the help for the user (bitwise - every bit is another help topic)
        public string SetHelp(string userName, int help)
        {
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();
                string query = $"UPDATE users SET help='{help}' WHERE user_name='{userName}'";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                }
                if (rdr != null)
                    rdr.Close();
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }

        }

        // Inserts user to mysql
        public string InsertUser(User user)
        {
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();

                string query = "INSERT INTO all4oledb.users(user_name, password, first_name, last_name, phone,  email, origin_country, residential_area, language, marital_status, has_little_children, hobbies, help) VALUES (?user_name,?password,?first_name,?last_name,?phone,?email,?origin_country,?residential_area,?language,?marital_status,?has_little_children,?hobbies,?help);";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                AddParametersToQuery(cmd, user);
                cmd.ExecuteNonQuery();
                return "Good";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }

        }

        // Adds the parameters of user to the mysql command
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

using System;
using System.Collections.Generic;
using AuthSystemBackend.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace AuthSystemBackend
{
    public class Database
    {
        private string connStr { get; set; }
        public Database()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            connStr = config.GetSection("ConnectionStrings:DefaultConnection").Value;
        }

        // Buscar usuario por username y password para hacer el inicio de sesion
        public UserModel FindUserToLogin(string username, string password)
        {

            // creamos instancia del modelo user
            UserModel user = new UserModel();

            // db conn
            using(MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string SQL = "SELECT * FROM users WHERE username = @username AND password = @password;";

                    MySqlCommand cmd = new MySqlCommand(SQL, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user.Id = Convert.ToInt32(reader["id"]);
                            user.Name = reader["name"].ToString();
                            user.UserName = reader["username"].ToString();
                            user.Mail = reader["mail"].ToString();
                            user.Phone = reader["phone"].ToString();
                            user.Address = reader["address"].ToString();
                            user.PostalCode = reader["postalCode"].ToString();
                            user.TypeOfUser = reader["typeOfUser"].ToString();
                            user.State = reader["state"].ToString();
                            user.City = reader["city"].ToString();
                        }
                    }
                    cmd.Dispose();
                }
                catch (MySqlException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
            return user;
        }

        // Listar usuarios con store procedure
        public List<UserModel> GetUsersList()
        {
            List<UserModel> users = new();
            //UserModel user = new();

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string SQL = "CALL GetAllUsers();";
                    MySqlCommand cmd = new MySqlCommand(SQL, conn);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new UserModel()
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Name = reader["name"].ToString(),
                                UserName = reader["username"].ToString(),
                                Mail = reader["mail"].ToString(),
                                Phone = reader["phone"].ToString(),
                                Address = reader["address"].ToString(),
                                PostalCode = reader["postalCode"].ToString(),
                                TypeOfUser = reader["typeOfUser"].ToString(),
                                State = reader["state"].ToString(),
                                City = reader["city"].ToString()
                            });
                        }
                    }

                    cmd.Dispose();
                    conn.Close();
                }
                catch(MySqlException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
            return users;
        }

        // Agregar nuevo usuario
        public void CreateUser(UserModel user)
        {
            MySqlTransaction transaction = null;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    transaction = conn.BeginTransaction();

                    //string SQL = "INSERT INTO users VALUES (null,@name, @username, @mail, @phone, @address, @postalCode, @typeOfUser, @state, @city, @password);";
                    string SQL = "CALL InsertUser(@name, @username, @mail, @phone, @address, @postalCode, @typeOfUser, @state, @city, @password);";
                    MySqlCommand cmd = new MySqlCommand(SQL, conn);

                    // pasando valores en los parametros
                    cmd.Parameters.AddWithValue("@name", user.Name);
                    cmd.Parameters.AddWithValue("@username", user.UserName);
                    cmd.Parameters.AddWithValue("@mail", user.Mail);
                    cmd.Parameters.AddWithValue("@phone", user.Phone);
                    cmd.Parameters.AddWithValue("@address", user.Address);
                    cmd.Parameters.AddWithValue("@postalCode", user.PostalCode);
                    cmd.Parameters.AddWithValue("@typeOfUser", user.TypeOfUser);
                    cmd.Parameters.AddWithValue("@state", user.State);
                    cmd.Parameters.AddWithValue("@city", user.City);
                    cmd.Parameters.AddWithValue("@password", user.Password);
                    cmd.ExecuteNonQuery(); // Ejecuta la query

                    transaction.Commit();
                    cmd.Dispose();
                    conn.Close();
                }
                catch (MySqlException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }

        // Eliminar usuario
        public void DeleteUser(int id)
        {
            MySqlTransaction transaction = null;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    transaction = conn.BeginTransaction();

                    string SQL = "CALL DeleteUser(@id);";
                    MySqlCommand cmd = new MySqlCommand(SQL, conn);

                    // pasando valores en los parametros
                    cmd.Parameters.AddWithValue("@id", id);
                    
                    cmd.ExecuteNonQuery(); // Ejecuta la query

                    transaction.Commit();
                    cmd.Dispose();
                    conn.Close();
                }
                catch (MySqlException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }

        // Actualizar usuario
        public void UpdateUser(UserModel user)
        {
            MySqlTransaction transaction = null;
            Console.Write(user.TypeOfUser);
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    transaction = conn.BeginTransaction();

                    //string SQL = "INSERT INTO users VALUES (null,@name, @username, @mail, @phone, @address, @postalCode, @typeOfUser, @state, @city, @password);";
                    string SQL = "CALL UpdateUser(@id, @name, @username, @mail, @phone, @address, @postalCode, @typeOfUser, @state, @city, @password);";
                    MySqlCommand cmd = new MySqlCommand(SQL, conn);

                    // pasando valores en los parametros
                    cmd.Parameters.AddWithValue("@id", user.Id);
                    cmd.Parameters.AddWithValue("@name", user.Name);
                    cmd.Parameters.AddWithValue("@username", user.UserName);
                    cmd.Parameters.AddWithValue("@mail", user.Mail);
                    cmd.Parameters.AddWithValue("@phone", user.Phone);
                    cmd.Parameters.AddWithValue("@address", user.Address);
                    cmd.Parameters.AddWithValue("@postalCode", user.PostalCode);
                    cmd.Parameters.AddWithValue("@typeOfUser", user.TypeOfUser);
                    cmd.Parameters.AddWithValue("@state", user.State);
                    cmd.Parameters.AddWithValue("@city", user.City);
                    cmd.Parameters.AddWithValue("@password", user.Password);
                    cmd.ExecuteNonQuery(); // Ejecuta la query

                    transaction.Commit();
                    cmd.Dispose();
                    conn.Close();
                }
                catch (MySqlException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }

        // Buscar el usuario
        public UserModel SearchUser(int id)
        {

            // creamos instancia del modelo user
            UserModel user = new UserModel();

            // db conn
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string SQL = "CALL SearchUser(@id);";

                    MySqlCommand cmd = new MySqlCommand(SQL, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user.Id = Convert.ToInt32(reader["id"]);
                            user.Name = reader["name"].ToString();
                            user.UserName = reader["username"].ToString();
                            user.Mail = reader["mail"].ToString();
                            user.Phone = reader["phone"].ToString();
                            user.Address = reader["address"].ToString();
                            user.PostalCode = reader["postalCode"].ToString();
                            user.TypeOfUser = reader["typeOfUser"].ToString();
                            user.State = reader["state"].ToString();
                            user.City = reader["city"].ToString();
                            user.Password = reader["password"].ToString();
                        }
                    }
                    cmd.Dispose();
                }
                catch (MySqlException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
            return user;
        }

    }
}

using DataAccess.Interfaces;
using Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace DataAccess {
    public class DBPlayer : IDBPlayer {
        /// <summary>
        /// Persists Player object
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns>PlayerId given to the persisted player</returns>
        public int CreatePlayer(Player player) {
            TransactionOptions to = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, to)) {
                using (SqlConnection connection = DBConnection.GetSqlConnection()) {

                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand()) {
                        command.CommandText = "INSERT INTO Player (NickName, Age, Role, TeamId) OUTPUT INSERTED.ID VALUES(@NickName, @Age, @Role, @TeamId)";
                        command.Parameters.AddWithValue("NickName", player.NickName);
                        command.Parameters.AddWithValue("Age", player.Age);
                        command.Parameters.AddWithValue("Role", player.Role);
                        command.Parameters.AddWithValue("TeamId", player.TeamId);
                        player.Id = (int)command.ExecuteScalar();
                    }
                    connection.Close();
                }
                scope.Complete();
            }
            return player.Id;
        }
        /// <summary>
        /// Deletes the player.
        /// </summary>
        /// <param name="player">The player.</param>
        public void DeletePlayer(Player player) {
            TransactionOptions to = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, to)) {
                using (SqlConnection connection = DBConnection.GetSqlConnection()) {

                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand()) {
                        command.Parameters.Clear();
                        command.CommandText = "DELETE FROM Player WHERE Id=@id; DBCC CHECKIDENT (@tableName, RESEED);";
                        command.Parameters.AddWithValue("tableName", "Player");
                        command.Parameters.AddWithValue("id", player.Id);
                        command.ExecuteNonQuery();

                    }
                    connection.Close();
                }
                scope.Complete();
            }
        }
        /// <summary>
        /// Edits information on a persisted player.
        /// </summary>
        /// <param name="player">The player.</param>
        public void EditPlayer(Player player) {
            TransactionOptions to = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, to)) {
                using (SqlConnection connection = DBConnection.GetSqlConnection()) {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand()) {
                        command.CommandText = "UPDATE Player SET NickName=@NickName, Age=@Age, Role=@Role, TeamId=@TeamId WHERE @Id=id";
                        command.Parameters.AddWithValue("NickName", player.NickName);
                        command.Parameters.AddWithValue("Age", player.Age);
                        command.Parameters.AddWithValue("Role", player.Role);
                        command.Parameters.AddWithValue("TeamId", player.TeamId);
                        command.Parameters.AddWithValue("id", player.Id);
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
                scope.Complete();
            }
        }
        /// <summary>
        /// Gets the player via a unique identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The found player</returns>
        public Player GetPlayer(int id) {
            Player player = null;
            DBTeam dBTeam = new DBTeam();
            TransactionOptions to = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, to)) {
                using (SqlConnection connection = DBConnection.GetSqlConnection()) {

                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand()) {

                        command.CommandText = "SELECT Id, NickName, Age, Role, TeamId FROM Player WHERE Id=@id";
                        command.Parameters.AddWithValue("Id", id);
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read()) {
                            player = new Player {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                NickName = reader.GetString(reader.GetOrdinal("NickName")),
                                Age = reader.GetString(reader.GetOrdinal("Age")),
                                Role = reader.GetString(reader.GetOrdinal("Role")),
                                TeamId = reader.GetInt32(reader.GetOrdinal("TeamId"))
                            };
                        }
                        reader.Close();
                        command.Parameters.Clear();
                    }
                    connection.Close();
                }
                scope.Complete();
            }
            return player;
        }
        /// <summary>
        /// Gets all players.
        /// </summary>
        /// <returns>List of Players</returns>
        public IEnumerable<Player> GetPlayers() {
            List<Player> players = new List<Player>();
            DBTeam dBTeam = new DBTeam();

            TransactionOptions to = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, to)) {
                using (SqlConnection connection = DBConnection.GetSqlConnection()) {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand()) {
                        Dictionary<Player, int> teamIdToPlayer = new Dictionary<Player, int>();
                        command.CommandText = "SELECT Id, NickName, Age, Role, TeamId FROM Player";
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read()) {
                            Player player = new Player {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                NickName = reader.GetString(reader.GetOrdinal("NickName")),
                                Age = reader.GetString(reader.GetOrdinal("Age")),
                                Role = reader.GetString(reader.GetOrdinal("Role")),
                                TeamId = reader.GetInt32(reader.GetOrdinal("TeamId"))
                            };
                            players.Add(player);
                        }
                        reader.Close();
                        command.Parameters.Clear();
                    }
                    connection.Close();
                }
                scope.Complete();
            }
            return players;
        }
        /// <summary>
        /// Gets the players on a given team.
        /// </summary>
        /// <param name="team">The team.</param>
        /// <returns>List of players</returns>
        public List<Player> GetPlayersByTeam(Team team) {
            List<Player> players = new List<Player>();
            TransactionOptions to = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, to)) {
                using (SqlConnection connection = DBConnection.GetSqlConnection()) {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand()) {
                        command.CommandText = "SELECT Id, NickName, Age, Role, TeamId FROM Player WHERE TeamId = @TeamId";
                        command.Parameters.AddWithValue("TeamId", team.Id);
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read()) {
                            Player player = new Player {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                NickName = reader.GetString(reader.GetOrdinal("NickName")),
                                Age = reader.GetString(reader.GetOrdinal("Age")),
                                Role = reader.GetString(reader.GetOrdinal("Role")),
                                TeamId = reader.GetInt32(reader.GetOrdinal("TeamId"))
                            };
                            players.Add(player);
                        }
                        reader.Close();
                    }
                    connection.Close();
                }
                scope.Complete();
            }
            return players;
        }
    }
}

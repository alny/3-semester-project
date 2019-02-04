using System;
using Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Transactions;
using DataAccess.Interfaces;

namespace DataAccess {
    public class DBTeam : IDBTeam {
        private DBPlayer _DbPlayer;
        /// <summary>
        /// Persists Team.
        /// </summary>
        /// <param name="team">The team.</param>
        /// <returns>TeamId given to the persisted team</returns>
        public int CreateTeam(Team team) {
            _DbPlayer = new DBPlayer();
            TransactionOptions to = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, to)) {
                using (SqlConnection connection = DBConnection.GetSqlConnection()) {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand()) {
                        command.CommandText = "INSERT INTO Team (Name) OUTPUT INSERTED.ID VALUES(@Name)";
                        command.Parameters.AddWithValue("Name", team.Name);
                        team.Id = (int)command.ExecuteScalar();

                        if (team != null) {
                            if (team.Players != null) {
                                foreach (Player p in team.Players) {
                                    _DbPlayer.EditPlayer(p);
                                }
                            }
                        }
                    }
                    connection.Close();
                }
                scope.Complete();
            }
            return team.Id;
        }
        /// <summary>
        /// Deletes the team. Players on the team will be listed as free agents.
        /// </summary>
        /// <param name="team">The team.</param>
        public void DeleteTeam(Team team) {
            int teamId = team.Id;
            if (team != null) {
                foreach (Player p in team.Players) {
                    p.TeamId = 1;
                    _DbPlayer.EditPlayer(p);
                }
            }
            TransactionOptions to = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, to)) {
                using (SqlConnection connection = DBConnection.GetSqlConnection()) {

                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand()) {
                        command.CommandText = "DELETE FROM Team WHERE Id = @id; DBCC CHECKIDENT (@tableName, RESEED);";
                        command.Parameters.AddWithValue("id", teamId);
                        command.Parameters.AddWithValue("tableName", "Team");
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
                scope.Complete();
            }   
        }
        /// <summary>
        /// Edits team specific information.
        /// </summary>
        /// <param name="team">The team.</param>
        public void EditTeam(Team team) {
            TransactionOptions to = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, to)) {
                using (SqlConnection connection = DBConnection.GetSqlConnection()) {

                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand()) {
                        command.CommandText = "UPDATE Team SET Name=@name WHERE Id=@id";
                        command.Parameters.AddWithValue("name", team.Name);
                        command.Parameters.AddWithValue("id", team.Id);
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
                scope.Complete();
            }
        }
        /// <summary>
        /// Gets the team via a unique identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Found team with players</returns>
        public Team GetTeam(int id) {
            Team team = null;
         _DbPlayer = new DBPlayer();

            TransactionOptions to = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, to)) {
                using (SqlConnection connection = DBConnection.GetSqlConnection()) {

                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand()) {
                        command.CommandText = "SELECT Id, Name FROM Team WHERE Id=@id";
                        command.Parameters.AddWithValue("Id", id);
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read()) {
                            team = new Team {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            };
                        }
                        reader.Close();

                        if (team != null) {
                            team.Players = _DbPlayer.GetPlayersByTeam(team);
                        }
                    }
                    connection.Close();
                }
                scope.Complete();
            }
            return team;
        }
        /// <summary>
        /// Gets all teams except free agents.
        /// </summary>
        /// <returns>List of Teams</returns>
        public IEnumerable<Team> GetTeams() {
            List<Team> teams = new List<Team>();
            _DbPlayer = new DBPlayer();
            TransactionOptions to = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, to)) {
                using (SqlConnection connection = DBConnection.GetSqlConnection()) {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand()) {
                        command.CommandText = "SELECT Id, Name FROM Team";
                        var reader = command.ExecuteReader();
                        while (reader.Read()) {
                            Team team = new Team {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            };
                            if (team.Id > 1) {
                                teams.Add(team);
                            }

                        }
                        reader.Close();
                        foreach (Team t in teams) {
                            if (t != null) {
                                t.Players = _DbPlayer.GetPlayersByTeam(t);
                                foreach (Player p in t.Players) {
                                    p.TeamId = t.Id;
                                }
                            }
                        }
                    }
                    connection.Close();
                }
                scope.Complete();
            }
            return teams;
        }
        /// <summary>
        /// Gets the teams in a given match.
        /// </summary>
        /// <param name="match">The match.</param>
        /// <returns>List of teams</returns>
        public List<Team> GetTeamsByMatch(Match match) {
            List<Team> teams = new List<Team>();
            _DbPlayer = new DBPlayer();
            TransactionOptions to = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, to)) {
                using (SqlConnection connection = DBConnection.GetSqlConnection()) {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand()) {
                        command.CommandText = "SELECT t.Id, t.Name FROM Team t INNER JOIN TeamsInMatch tim ON t.Id = tim.TeamId WHERE tim.MatchId = @MatchId";
                        command.Parameters.AddWithValue("MatchId", match.Id);
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read()) {
                            Team team = new Team {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            };
                            teams.Add(team);
                        }
                        reader.Close();

                        foreach (Team t in teams) {
                            if (t != null) {
                                t.Players = _DbPlayer.GetPlayersByTeam(t);
                                foreach (Player p in t.Players) {
                                    p.TeamId = t.Id;
                                }
                            }
                        }
                    }
                    connection.Close();
                }
                scope.Complete();
            }
            return teams;
        }
        /// <summary>
        /// Gets the winner team by match.
        /// </summary>
        /// <param name="match">The match.</param>
        /// <returns>Team</returns>
        public Team GetWinnerTeamByMatch(Match match) {
            _DbPlayer = new DBPlayer();
            Team team = null;
            TransactionOptions to = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, to)) {
                using (SqlConnection connection = DBConnection.GetSqlConnection()) {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand()) {
                        command.CommandText = "SELECT t.Id, t.Name FROM Team t INNER JOIN Match m ON t.Id = m.WinnerId WHERE m.Id = @MatchId";
                        command.Parameters.AddWithValue("MatchId", match.Id);
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read()) {
                            team = new Team {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            };
                        }
                        reader.Close();

                        if (team != null) {
                            team.Players = _DbPlayer.GetPlayersByTeam(team);
                            foreach (Player p in team.Players) {
                                p.TeamId = team.Id;
                            }
                        }
                    }
                    connection.Close();
                }
                scope.Complete();
            }
            return team;
        }
        /// <summary>
        /// Gets a team by a Player.
        /// </summary>
        /// <param name="player"></param>
        /// <returns>Team</returns>
        public Team GetTeamByPlayer(Player player) {
            Team team = null;
            TransactionOptions to = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, to)) {
                using (SqlConnection connection = DBConnection.GetSqlConnection()) {

                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand()) {
                        command.CommandText = "SELECT t.id, t.Name FROM Team t, Player p WHERE p.TeamId = t.Id AND p.Id = @playerId";
                        command.Parameters.AddWithValue("playerId", player.Id);
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read()) {
                            team = new Team {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            };
                        }
                        reader.Close();
                    }
                    connection.Close();
                }
                scope.Complete();
            }
            return team;
        }
    }
}


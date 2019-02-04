using DataAccess.Interfaces;
using Model;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Transactions;

namespace DataAccess {
    public class DBMatch : IDBMatch {
        /// <summary>
        /// Persists Match object
        /// </summary>
        /// <param name="match">The match.</param>
        /// <returns>MatchId given to the persisted match</returns>
        public int CreateMatch(Match match) {
            int matchId = 0;
            TransactionOptions to = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, to)) {
                using (SqlConnection connection = DBConnection.GetSqlConnection()) {

                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand()) {
                        command.CommandText = "INSERT INTO Match (Format, WinnerId, EventId) OUTPUT INSERTED.ID VALUES(@Format, @WinnerId, @EventId)";
                        if (match.Format == null) {
                            match.Format = "BO1";
                        }
                        command.Parameters.AddWithValue("Format", match.Format);
                        command.Parameters.AddWithValue("WinnerId", -1);
                        if (match.EventId == 0) {
                            match.EventId = 1;
                        }
                        command.Parameters.AddWithValue("EventId", match.EventId);
                        matchId = (int)command.ExecuteScalar();
                        match.Id = matchId;

                        if (match.Maps != null) {
                            foreach (Map map in match.Maps) {
                           
                                command.CommandText = "INSERT INTO MapsOnMatch (MatchId, MapId) VALUES(@MatchId, @MapId)";
                                command.Parameters.AddWithValue("MatchId", matchId);
                                command.Parameters.AddWithValue("MapId", map.Id);
                                command.ExecuteNonQuery();
                                command.Parameters.Clear();
                            }
                        }

                        if (match.Teams != null) {
                            command.CommandText = "INSERT INTO TeamsInMatch (TeamId, MatchId) VALUES (@TeamId, @MatchId)";
                            for (int i = 0; i < match.Teams.Count; i++) {
                                command.Parameters.AddWithValue("@TeamId", match.Teams[i].Id);
                                command.Parameters.AddWithValue("@MatchId", match.Id);
                                command.ExecuteNonQuery();
                                command.Parameters.Clear();
                            }
                        }
                    }
                    connection.Close();
                }
                scope.Complete();
            }
            return matchId;
        }
        /// <summary>
        /// Deletes a persisted match.
        /// </summary>
        /// <param name="match">The match.</param>
        public void DeleteMatch(Match match) {
            TransactionOptions to = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, to)) {
                using (SqlConnection connection = DBConnection.GetSqlConnection()) {

                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand()) {
                        command.CommandText = "DELETE FROM MapsOnMatch WHERE MatchId=@id; DBCC CHECKIDENT(@tableName, RESEED);";
                        command.Parameters.AddWithValue("id", match.Id);
                        command.Parameters.AddWithValue("tableName", "MapsOnMatch");
                        command.ExecuteNonQuery();
                        command.Parameters.Clear();

                        command.CommandText = "DELETE FROM TeamsInMatch WHERE MatchId=@id; DBCC CHECKIDENT(@tableName, RESEED)";
                        command.Parameters.AddWithValue("id", match.Id);
                        command.Parameters.AddWithValue("tableName", "TeamsInMatch");
                        command.ExecuteNonQuery();
                        command.Parameters.Clear();

                        command.CommandText = "DELETE FROM Match WHERE Id=@id; DBCC CHECKIDENT(@tableName, RESEED)";
                        command.Parameters.AddWithValue("id", match.Id);
                        command.Parameters.AddWithValue("tableName", "Match");
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
                scope.Complete();
            }
        }
        /// <summary>
        /// Edits information for the persisted match.
        /// </summary>
        /// <param name="match">The match.</param>
        public void EditMatch(Match match) {
            TransactionOptions to = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, to)) {
                using (SqlConnection connection = DBConnection.GetSqlConnection()) {

                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand()) {
                        command.CommandText = "UPDATE Match SET Format=@Format, WinnerId=@WinnerId, EventId=@EventId, Held=@Held WHERE Id=@id";
                        command.Parameters.AddWithValue("Format", match.Format);
                        if (match.Winner == null) {
                            command.Parameters.AddWithValue("WinnerId", -1);
                        } else {
                            command.Parameters.AddWithValue("WinnerId", match.Winner.Id);
                        }
                        if (match.Held){
                            command.Parameters.AddWithValue("Held", 1);
                        }
                        else{
                            command.Parameters.AddWithValue("Held", 0);
                        }

                        command.Parameters.AddWithValue("EventId", match.EventId);
                        command.Parameters.AddWithValue("id", match.Id);
                        command.ExecuteNonQuery();

                        foreach (Map map in match.Maps) {
                            command.CommandText = "UPDATE MapsOnMatch SET MatchId = @MatchId, MapId = @MapId WHERE MatchId = @MatchId";
                            command.Parameters.AddWithValue("MatchId", match.Id);
                            command.Parameters.AddWithValue("MapId", map.Id);
                            command.ExecuteNonQuery();
                            command.Parameters.Clear();
                        }

                        SqlDataReader reader;

                        command.CommandText = "SELECT Id FROM TeamsInMatch WHERE MatchId = @MatchId";
                        command.Parameters.AddWithValue("@MatchId", match.Id);
                        
                        reader = command.ExecuteReader();

                        int[] ids = new int[2];

                        int count = 0;

                        while (reader.Read()) {
                            ids[count] = reader.GetInt32(reader.GetOrdinal("Id"));
                            count++;
                        }
                        command.Parameters.Clear();
                        reader.Close();
                        for (int i = 0; i < match.Teams.Count; i++) {
                            command.CommandText = "UPDATE TeamsInMatch SET TeamId = @TeamId WHERE Id = @id";
                            command.Parameters.AddWithValue("@TeamId", match.Teams[i].Id);
                            command.Parameters.AddWithValue("@id", ids[i]);
                            command.ExecuteNonQuery();
                            command.Parameters.Clear();
                        }
                        match.GenerateName(match.Teams[0], match.Teams[1]);
                    }
                    connection.Close();
                }
                scope.Complete();
            }
        }
        /// <summary>
        /// Gets the match by unique identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The found match</returns>
        public Match GetMatch(int id) {
            DBTeam dBTeam = new DBTeam();
            DBEvent dBEvent = new DBEvent();
            Match match = null;
            int eventId = 0;
            List<Map> mapList = new List<Map>();

            TransactionOptions to = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, to)) {
                using (SqlConnection connection = DBConnection.GetSqlConnection()) {

                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand()) {

                        command.CommandText = "SELECT Id, Format, WinnerId, EventId FROM Match WHERE Id=@id";
                        command.Parameters.AddWithValue("Id", id);

                        int winnerId = 0;
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read()) {


                            match = new Match {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Format = reader.GetString(reader.GetOrdinal("Format")),
                                EventId = reader.GetInt32(reader.GetOrdinal("EventId"))
                            };

                            winnerId = reader.GetInt32(reader.GetOrdinal("WinnerId"));

                            eventId = reader.GetInt32(reader.GetOrdinal("EventId"));

                        }
                        command.Parameters.Clear();
                        reader.Close();

                        if (match != null) {
                            GetMapsByMatch(match);
                            match.Teams = dBTeam.GetTeamsByMatch(match);
                            match.Winner = dBTeam.GetTeam(winnerId);
                            match.GenerateName(match.Teams[0], match.Teams[1]);
                        }

                    }
                    connection.Close();
                }
                scope.Complete();
            }
            return match;
        }
        /// <summary>
        /// Persists Map object
        /// </summary>
        /// <param name="map">The map.</param>
        /// <returns>MapId given to the persisted map</returns>
        public int CreateMap(Map map) {
            int mapId;
            TransactionOptions to = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, to)) {
                using (SqlConnection connection = DBConnection.GetSqlConnection()) {

                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand()) {
                        command.CommandText = "INSERT INTO Map (Name, IsActive) OUTPUT INSERTED.ID VALUES(@Name, @IsActive)";
                        command.Parameters.AddWithValue("@Name", map.Name);
                        command.Parameters.AddWithValue("@IsActive", map.IsActive);
                        mapId = (int)command.ExecuteScalar();
                    }
                    connection.Close();
                }
                scope.Complete();
            }
            return mapId;
        }
        /// <summary>
        /// Gets all matches.
        /// </summary>
        /// <returns>List of matches</returns>
        public IEnumerable<Match> GetMatches() {
            List<Match> matches = new List<Match>();
            Match match = null;
            DBTeam dBTeam = new DBTeam();
            SqlDataReader reader;

            TransactionOptions to = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, to)) {
                using (SqlConnection connection = DBConnection.GetSqlConnection()) {

                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand()) {

                        command.CommandText = "SELECT Id, Format, WinnerId, EventId FROM Match";
                        reader = command.ExecuteReader();
                        while (reader.Read()) {
                            match = new Match {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Format = reader.GetString(reader.GetOrdinal("Format")),
                                EventId = reader.GetInt32(reader.GetOrdinal("EventId"))
                            };
                            matches.Add(match);
                        }
                        reader.Close();
                        command.Parameters.Clear();
                        if (matches != null) {
                            foreach (Match m in matches) {
                                if (m != null) {
                                    GetMapsByMatch(m);
                                    m.Teams = dBTeam.GetTeamsByMatch(m);
                                    m.Winner = dBTeam.GetWinnerTeamByMatch(m);
                                    if (m.Teams.Count != 0) {
                                        m.GenerateName(m.Teams[0], m.Teams[1]);
                                    }
                                }
                            }
                        }
                    }
                    connection.Close();
                }
                scope.Complete();
            }
            return matches;
        }
        /// <summary>
        /// Gets the matches by event.
        /// </summary>
        /// <param name="eventt">The eventt.</param>
        /// <returns>List of matches</returns>
        public List<Match> GetMatchesByEvent(Event eventt) {
            List<Match> matches = new List<Match>();
            Match match = null;
            DBTeam dBTeam = new DBTeam();
            SqlDataReader reader;

            TransactionOptions to = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, to)) {
                using (SqlConnection connection = DBConnection.GetSqlConnection()) {

                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand()) {

                        command.CommandText = "SELECT Id, Format, WinnerId, EventId FROM Match WHERE EventId = @EventId";
                        command.Parameters.AddWithValue("EventId", eventt.Id);
                        reader = command.ExecuteReader();
                        while (reader.Read()) {
                            match = new Match {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Format = reader.GetString(reader.GetOrdinal("Format")),
                                EventId = reader.GetInt32(reader.GetOrdinal("EventId"))
                            };

                            matches.Add(match);

                        }
                        reader.Close();
                        command.Parameters.Clear();

                        foreach (Match m in matches) {
                            if (m != null) {
                                GetMapsByMatch(m);
                                m.Teams = dBTeam.GetTeamsByMatch(m);
                                m.Winner = dBTeam.GetWinnerTeamByMatch(m);
                                if (m.Teams.Count != 0) {
                                    m.GenerateName(m.Teams[0], m.Teams[1]);
                                }
                            }
                        }

                    }
                    connection.Close();
                }
                scope.Complete();
            }
            return matches;
        }
        /// <summary>
        /// Gets the maps by match and adds them to the match given in the parameters.
        /// </summary>
        /// <param name="match">The match.</param>
        private void GetMapsByMatch(Match match) {
            TransactionOptions to = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, to)) {
                using (SqlConnection connection = DBConnection.GetSqlConnection()) {

                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand()) {
                        if (match != null) {
                            command.CommandText = "SELECT mat.Id AS MatchId, mat.Format, mat.WinnerId, m.Id AS MapId, m.Name, m.IsActive AS IsActive FROM Match mat INNER JOIN MapsOnMatch mom ON mat.Id = mom.MatchId INNER JOIN Map m ON m.Id = mom.MapId WHERE mat.Id = @Id";
                            command.Parameters.AddWithValue("Id", match.Id);
                            SqlDataReader reader = command.ExecuteReader();
                            while (reader.Read()) {
                                Map map = new Map {
                                    Id = reader.GetInt32(reader.GetOrdinal("MapId")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))

                                };
                                match.Maps.Add(map);
                            }
                            command.Parameters.Clear();
                            reader.Close();
                        }

                    }
                    connection.Close();
                }
                scope.Complete();
            }
        }
        /// <summary>
        /// Gets the maps.
        /// </summary>
        /// <returns>List of Maps</returns>
        public IEnumerable<Map> GetMaps() {
            List<Map> maps = new List<Map>();
            Map map = null;
            SqlDataReader reader;

            TransactionOptions to = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, to)) {
                using (SqlConnection connection = DBConnection.GetSqlConnection()) {

                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand()) {

                        command.CommandText = "SELECT Id, Name, IsActive FROM Map";
                        reader = command.ExecuteReader();
                        while (reader.Read()) {
                            map = new Map {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                            };
                            maps.Add(map);
                        }
                        reader.Close();
                        command.Parameters.Clear();
                    }
                    connection.Close();
                }
                scope.Complete();
            }
            return maps;
        }
        /// <summary>
        /// Edits information about the map.
        /// </summary>
        /// <param name="map">The map.</param>
        public void EditMap(Map map) {
            TransactionOptions to = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, to)) {
                using (SqlConnection connection = DBConnection.GetSqlConnection()) {

                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand()) {
                        command.CommandText = "UPDATE Map SET Name = @Name, IsActive = @IsActive WHERE Id = @Id";
                        command.Parameters.AddWithValue("@Name", map.Name);
                        command.Parameters.AddWithValue("@IsActive", map.IsActive);
                        command.Parameters.AddWithValue("@Id", map.Id);
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
                scope.Complete();
            }
        }
        /// <summary>
        /// Deletes the persisted map.
        /// </summary>
        /// <param name="map">The map.</param>
        public void DeleteMap(Map map) {
            TransactionOptions to = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, to)) {
                using (SqlConnection connection = DBConnection.GetSqlConnection()) {

                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand()) {
                        command.CommandText = "DELETE FROM Map WHERE Id=@id; DBCC CHECKIDENT (@tableName, RESEED);";
                        command.Parameters.AddWithValue("tableName", "Map");
                        command.Parameters.AddWithValue("id", map.Id);
                        command.ExecuteNonQuery();
                        command.Parameters.Clear();
                    }
                    connection.Close();
                }
                scope.Complete();
            }
        }

    }
}
using DataAccess.Interfaces;
using Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace DataAccess {
    public class DBEvent : IDBEvent {
        DBMatch _DbMatch;
        /// <summary>
        /// Persists Event object
        /// </summary>
        /// <param name="eventToCreate">The event to create.</param>
        /// <returns>EventId given to the persisted event</returns>
        public int CreateEvent(Event eventToCreate) {
            _DbMatch = new DBMatch();
            int eventId = 0;
            TransactionOptions to = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, to)) {
                using (SqlConnection connection = DBConnection.GetSqlConnection()) {

                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand()) {
                        command.CommandText = "INSERT INTO Event (Name, GameName, Type) OUTPUT INSERTED.ID VALUES(@Name, @GameName, @Type)";
                        command.Parameters.AddWithValue("Name", eventToCreate.Name);
                        command.Parameters.AddWithValue("GameName", eventToCreate.GameName);
                        command.Parameters.AddWithValue("Type", eventToCreate.Type);
                        eventId = (int)command.ExecuteScalar();
                        eventToCreate.Id = eventId;

                        if (eventToCreate != null) {
                            if (eventToCreate.Matches != null) {
                              foreach (Match m in eventToCreate.Matches) {
                                m.Id = eventToCreate.Id;
                                _DbMatch.CreateMatch(m);
                              }
                            }
                        }
                    }
                    connection.Close();
                }
                scope.Complete();
            }
            return eventId;
        }
        /// <summary>
        /// Deletes the event given by the parameter.
        /// </summary>
        /// <param name="eventToDelete">The event to delete.</param>
        public void DeleteEvent(Event eventToDelete) {
            _DbMatch = new DBMatch();
            TransactionOptions to = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, to)) {
                using (SqlConnection connection = DBConnection.GetSqlConnection()) {

                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand()) {
                        command.CommandText = "DELETE FROM Event WHERE Id=@id; DBCC CHECKIDENT (Event, RESEED);";
                        command.Parameters.AddWithValue("id", eventToDelete.Id);
                        command.ExecuteNonQuery();

                        if (eventToDelete != null) {
                            foreach (Match m in eventToDelete.Matches) {
                                _DbMatch.DeleteMatch(m);
                            }
                        }
                    }
                    connection.Close();
                }
                scope.Complete();
            }
        }
        /// <summary>
        /// Edits information about the event.
        /// </summary>
        /// <param name="eventToUpdate">The event to update.</param>
        public void EditEvent(Event eventToUpdate) {
            _DbMatch = new DBMatch();
            TransactionOptions to = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, to)) {
                using (SqlConnection connection = DBConnection.GetSqlConnection()) {

                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand()) {
                        command.CommandText = "UPDATE Event SET Name = @Name, GameName = @GameName, Type = @Type WHERE id = @id";
                        command.Parameters.AddWithValue("id", eventToUpdate.Id);
                        command.Parameters.AddWithValue("Name", eventToUpdate.Name);
                        command.Parameters.AddWithValue("GameName", eventToUpdate.GameName);
                        command.Parameters.AddWithValue("Type", eventToUpdate.Type);
                        command.ExecuteNonQuery();

                        if (eventToUpdate != null) {
                            foreach (Match m in eventToUpdate.Matches) {
                                _DbMatch.EditMatch(m);
                            }
                        }
                    }
                    connection.Close();
                }
                scope.Complete();
            }
        }
        /// <summary>
        /// Gets an event by unique identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The found event</returns>
        public Event GetEvent(int id) {
            _DbMatch = new DBMatch();
            Event foundEvent = null;

            TransactionOptions to = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, to)) {
                using (SqlConnection connection = DBConnection.GetSqlConnection()) {

                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand()) {

                        command.CommandText = "SELECT Id, Name, GameName, Type FROM Event WHERE Id=@id";
                        command.Parameters.AddWithValue("Id", id);
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read()) {
                            foundEvent = new Event {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                GameName = reader.GetString(reader.GetOrdinal("GameName")),
                                Type = reader.GetString(reader.GetOrdinal("Type"))
                            };
                        }
                        reader.Close();

                        if (foundEvent != null) {
                            foundEvent.Matches = _DbMatch.GetMatchesByEvent(foundEvent);
                        }
                    }
                    connection.Close();
                }
                scope.Complete();
            }
            return foundEvent;
        }
        /// <summary>
        /// Gets All events.
        /// </summary>
        /// <returns>List of Events</returns>
        public IEnumerable<Event> GetEvents() {
            List<Event> events = new List<Event>();
            List<int> matchIdList = new List<int>();
            _DbMatch = new DBMatch();
            Event eventToAdd = null;
            TransactionOptions to = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, to)) {
                using (SqlConnection connection = DBConnection.GetSqlConnection()) {

                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand()) {

                        command.CommandText = "SELECT Id, Name, GameName, Type FROM Event";
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read()) {
                            eventToAdd = new Event {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                GameName = reader.GetString(reader.GetOrdinal("GameName")),
                                Type = reader.GetString(reader.GetOrdinal("Type"))
                            };
                            events.Add(eventToAdd);
                        }
                        reader.Close();
                        command.Parameters.Clear();

                        foreach (Event e in events) {
                            if (e != null) {
                                e.Matches = _DbMatch.GetMatchesByEvent(e);
                            }
                        }
                    }
                    connection.Close();
                }
                scope.Complete();
            }
            return events;
        }
    }
}

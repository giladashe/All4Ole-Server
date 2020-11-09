using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace All4Ole_Server.Model
{
    public interface IManager
    {
        // Gets flight plan from DB by id.
        User GetUser(string key);

        // Inserts new Flight plan to DB.
        string InsertUser(User user);

       /* // Deletes flight (plan) from DB.
        string DeleteFlight(string id);

        // Get all flights from DB.
        Task<IEnumerable<Flight>> GetAllFlights(string dateTime, bool isExternal);

        // Get all Server urls from DB.
        IEnumerable<Server> GetAllServers();

        // Insert new Server to DB.
        string InsertServer(Server server);

        // Deletes Server from DB by id.
        string DeleteServer(string id);*/
    }
}

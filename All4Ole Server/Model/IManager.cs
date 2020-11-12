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

        string Login(string userName, string password);

        // Inserts new Flight plan to DB.
        string InsertUser(User user);

        string SetHelp(string userName, int help);

        List<User> PeopleLikeMe(string userName);

        List<User> FindFriendsForHobbies(string userName, int hobbies);

    }
}

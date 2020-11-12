using System.Collections.Generic;

namespace All4Ole_Server.Model
{
    public interface IManager
    {
        
        string Login(string userName, string password);

        // Inserts new Flight plan to DB.
        string InsertUser(User user);

        string SetHelp(string userName, int help);

        List<User> PeopleLikeMe(string userName);

        List<User> FindFriendsForHobbies(string userName, int hobbies);

        List<User> LookForHelp(string userName, int help);

    }
}

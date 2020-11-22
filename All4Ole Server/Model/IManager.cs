using System.Collections.Generic;

namespace All4Ole_Server.Model
{
    public interface IManager
    {

        User Login(string userName, string password);

        // Inserts new user to the db
        string InsertUser(User user);

        // Sets help for user
        string SetHelp(string userName, int help);

        // Searches for people who are similar to the user
        List<User> PeopleLikeMe(string userName);

        // Finds users for hobbies
        List<User> FindFriendsForHobbies(string userName, int hobbies);

        // Look for users who can help the user
        List<User> LookForHelp(string userName, int help);

    }
}

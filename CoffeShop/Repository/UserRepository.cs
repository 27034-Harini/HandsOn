using CoffeeShop.Model;

namespace CoffeeShop.Repository
{
    internal class UserRepository
    {
        private List<UserInfo> _users = new List<UserInfo>();
        public void AddUser(UserInfo user)
        {
            _users.Add(user);
        }

        public List<UserInfo> GetAllUsers()
        {
            return _users;
        }

    }
}

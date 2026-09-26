using System.Text.Json;
using System.Text.Json.Nodes;
using CoffeeShop.Model;

namespace CoffeeShop.Repository
{
    internal class UserRepository
    {
        private readonly string _userFilePath;

        public UserRepository(string userFilePath)
        {
            this._userFilePath = userFilePath;
        }

        public void AddUser(UserInfo user)
        {
            List<UserInfo> users = this.LoadUsers();
            users.Add(user);
            this.SaveUsers(users);
        }

        public List<UserInfo> GetAllUsers()
        {
            return LoadUsers();
        }

        private List<UserInfo> LoadUsers()
        {
            if (!File.Exists(_userFilePath))
            {
                return new List<UserInfo>();
            }

            string data = File.ReadAllText(_userFilePath);
            if (string.IsNullOrWhiteSpace(data))
            {
                return new List<UserInfo>();
            }
            return JsonSerializer.Deserialize<List<UserInfo>>(data) ?? new List<UserInfo>();
        }

        private void SaveUsers(List<UserInfo> users)
        {
            string data = JsonSerializer.Serialize(
                users,
                new JsonSerializerOptions
                {
                    WriteIndented = true,
                });
            File.WriteAllText(this._userFilePath, data);
        }
    }
}

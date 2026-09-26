using CoffeeShop.Repository;
using CoffeeShop.Helper;
using CoffeeShop.Model;
using CoffeeShop.Utils;

namespace CoffeeShop.Service
{
    internal class UserService
    {
        private UserRepository _userRepository;
        private Validator _validator;
        public UserService(Validator validator, UserRepository userRepository)
        {
            _validator = validator;
            _userRepository = userRepository;
        }

        internal void SignUp(string username, string email, string phoneNumber, string password)
        {
            this._userRepository.AddUser(new UserInfo(Guid.NewGuid(), username, email, phoneNumber, password));
        }

        internal bool IsEmailAlreadyExists(string email)
        {
            List<UserInfo> users = this._userRepository.GetAllUsers();
            UserInfo? foundUser = users.FirstOrDefault(user => user.Email == email);
            if (foundUser != null)
            {
                return true;
            }
            return false;
        }

        internal bool IsPhoneNumberAlreadyExists(string phoneNumber)
        {
            List<UserInfo> users = this._userRepository.GetAllUsers();
            UserInfo? foundUser = users.FirstOrDefault(user => user.PhoneNumber == phoneNumber);
            if (foundUser != null)
            {
                return true;
            }
            return false;
        }

        internal bool IsUserExists(string email, string password)
        {
            List<UserInfo> users = this._userRepository.GetAllUsers();
            UserInfo? foundUser = users.FirstOrDefault(user => user.Email == email);
            if (foundUser != null && foundUser.Password == password)
            {
                return true;
            }
            return false;
        }

        internal bool ValidateEmail(string email)
        {
            return this._validator.ValidateEmail(email);
        }

        internal bool ValidateName(string userName)
        {
            return this._validator.ValidateName(userName);
        }

        internal bool ValidatePassword(string password)
        {
            return this._validator.ValidatePassword(password);
        }

        internal bool Login(string email, string password)
        {
            List<UserInfo> users = this._userRepository.GetAllUsers();
            UserInfo? foundUser = users.FirstOrDefault(user => user.Email == email && user.Password == password);
            if (CurrentSession.IsLoggedIn || foundUser == null)
            {
                return false;
            }
            CurrentSession.StartSession(foundUser);
            return true;
        }

        internal bool Logout()
        {
            if (CurrentSession.IsLoggedIn)
            {
                CurrentSession.ClearSession();
                return true;
            }
            return false;
        }

        internal bool ValidatePhoneNumber(string phoneNumber)
        {
            return this._validator.ValidatePhoneNumber(phoneNumber);
        }
    }
}
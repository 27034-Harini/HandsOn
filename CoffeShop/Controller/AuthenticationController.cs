using CoffeeShop.Constants;
using CoffeeShop.Enums;
using CoffeeShop.Service;
using CoffeeShop.Views;

namespace CoffeeShop.Controller
{
    internal class AuthenticationController
    {

        private int _maxNoOfTries = 3;
        private View _view;
        private UserService _userService;
        private CoffeeController _coffeeController;

        public AuthenticationController(View view, UserService userService, CoffeeController coffeeController)
        {
            this._view = view;
            this._userService = userService;
            this._coffeeController = coffeeController;
        }

        public void UserAccessOption()
        {
            bool shallBreak = false;
            while (!shallBreak)
            {
                string option = this._view.DisplayAndGetChoice<UserAccess>(ConsoleMessages.ChooseOption);
                switch (option)
                {
                    case "1":
                        this.SignUp();
                        break;
                    case "2":
                        this.Login();
                        break;
                    case "3":
                        shallBreak = true;
                        break;
                    default:
                        this._view.DisplayErrorMessage(ConsoleMessages.InvalidInput);
                        break;
                }
            }
        }

        private void Logout()
        {
            bool IsLoggedOut = this._userService.Logout();
            if (!IsLoggedOut)
            {
                this._view.DisplayErrorMessage(ConsoleMessages.LogoutFailed);
                return;
            }
            this._view.DisplayErrorMessage(ConsoleMessages.LogoutSucceed);
        }

        private void Login()
        {
            string? email = null;
            string? password = null;
            bool gotEmail = this.GetEmail(out email);
            if (!gotEmail)
            {
                return;
            }
            bool gotPassword = this.GetPassword(out password);
            if (!gotPassword)
            {
                return;
            }
            if (!this._userService.IsUserExists(email, password))
            {
                this._view.DisplayErrorMessage(ConsoleMessages.InvalidEmailOrPassword);
                return;
            }
            bool IsLoggedIn = this._userService.Login(email, password);
            if (!IsLoggedIn)
            {
                this._view.DisplayErrorMessage(ConsoleMessages.LoginFailed);
                return;
            }
            this._view.DisplaySuccessMessage(ConsoleMessages.LoginSucceed);
            this._coffeeController.GetMenuChoice();
            this.Logout();
        }

        private void SignUp()
        {
            string? username = null;
            string? email = null;
            string? phoneNumber = null;
            string? password = null;
            bool gotUserName = this.GetUserName(out username);
            if (!gotUserName)
            {
                return;
            }
            bool gotEmail = this.GetEmail(out email);
            if (!gotEmail)
            {
                return;
            }
            if (this._userService.IsEmailAlreadyExists(email))
            {
                this._view.DisplayErrorMessage(ConsoleMessages.EmailAlreadyExist);
                return;
            }
            bool gotPhoneNumber = this.GetPhoneNumber(out phoneNumber);
            if (!gotPhoneNumber)
            {
                return;
            }
            if (this._userService.IsPhoneNumberAlreadyExists(phoneNumber))
            {
                this._view.DisplayErrorMessage(ConsoleMessages.PhoneNumberAlreadyExist);
                return;
            }
            bool gotPassword = this.GetPassword(out password);
            if (!gotPassword)
            {
                return;
            }
            this._userService.SignUp(username, email, phoneNumber, password);
            this._view.DisplaySuccessMessage(ConsoleMessages.SignUpSucceed);

        }

        private bool GetPassword(out string password)
        {
            password = string.Empty;
            int passwordCount = this._maxNoOfTries;
            while (passwordCount > 0)
            {
                password = this._view.GetUserDetail(ConsoleMessages.GetPassword);
                if (!this._userService.ValidatePassword(password))
                {
                    this._view.DisplayErrorMessage(ConsoleMessages.InvalidInput);
                    this._view.DisplayAttemptsLeft(--passwordCount);
                    continue;
                }

                return true;
            }

            this._view.DisplayErrorMessage(ConsoleMessages.MaxAttemptsReached);
            return false;
        }

        private bool GetPhoneNumber(out string phoneNumber)
        {
            phoneNumber = string.Empty;
            int phoneNumberCount = this._maxNoOfTries;
            while (phoneNumberCount > 0)
            {
                phoneNumber = this._view.GetUserDetail(ConsoleMessages.GetPhoneNumber);
                if (!this._userService.ValidatePhoneNumber(phoneNumber))
                {
                    this._view.DisplayErrorMessage(ConsoleMessages.InvalidInput);
                    this._view.DisplayAttemptsLeft(--phoneNumberCount);
                    continue;
                }

                return true;
            }

            this._view.DisplayErrorMessage(ConsoleMessages.MaxAttemptsReached);
            return false;
        }

        private bool GetEmail(out string email)
        {
            email = string.Empty;
            int emailCount = this._maxNoOfTries;
            while (emailCount > 0)
            {
                email = this._view.GetUserDetail(ConsoleMessages.GetEmail);
                if (!this._userService.ValidateEmail(email))
                {
                    this._view.DisplayErrorMessage(ConsoleMessages.InvalidInput);
                    this._view.DisplayAttemptsLeft(--emailCount);
                    continue;
                }

                return true;
            }

            this._view.DisplayErrorMessage(ConsoleMessages.MaxAttemptsReached);
            return false;
        }

        private bool GetUserName(out string userName)
        {
            userName = string.Empty;
            int nameCount = this._maxNoOfTries;
            while (nameCount > 0)
            {
                userName = this._view.GetUserDetail(ConsoleMessages.GetName);
                if (!this._userService.ValidateName(userName))
                {
                    this._view.DisplayErrorMessage(ConsoleMessages.InvalidInput);
                    this._view.DisplayAttemptsLeft(--nameCount);
                    continue;
                }

                return true;
            }

            this._view.DisplayErrorMessage(ConsoleMessages.MaxAttemptsReached);
            return false;
        }
    }
}

namespace CoffeeShop.Model
{
    internal class UserInfo
    {
        public UserInfo(Guid id, string username, string email, string phoneNumber, string password)
        {
            this.UserId = id;
            this.UserName = username;
            this.Email = email;
            this.PhoneNumber = phoneNumber;
            this.Password = password;
        }

        public Guid UserId { get; init; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }

    }
}

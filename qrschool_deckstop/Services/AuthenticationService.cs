using qrschool_deckstop.DataAccess;
using qrschool_deckstop.Models;
using qrschool_deckstop.Helpers;
using System;
using System.Diagnostics;

namespace qrschool_deckstop.Services
{
    public class AuthenticationService
    {
        private readonly UserRepository _userRepository;
        public User CurrentUser { get; private set; }

        public AuthenticationService()
        {
            _userRepository = new UserRepository();
        }

        public bool Login(string username, string password)
        {
            var user = _userRepository.GetByUsername(username);
            if (user == null) 
            {
                return false;
            }

            // Debug info
            Debug.WriteLine($"[Auth] username={username}");
            Debug.WriteLine($"[Auth] dbHash='{user?.PasswordHash}'");
            Debug.WriteLine($"[Auth] dbPlain='{user?.PasswordPlain}'");

            // Используем новый метод репозитория: сначала проверка plain, затем hash
            if (!_userRepository.ValidatePassword(user, password))
            {
                return false;
            }

            CurrentUser = user;
            _userRepository.UpdateLastLogin(user.Id);
            return true;
        }

        public void Logout()
        {
            CurrentUser = null;
        }

        public bool IsAccountant => CurrentUser?.Role == "accountant";
        public bool IsTech => CurrentUser?.Role == "tech";
    }
}

using qrschool_deckstop.DataAccess;
using qrschool_deckstop.Models;

namespace qrschool_deckstop.Services
{
    public class UserService
    {
        private readonly UserRepository _repo;

        public UserService()
        {
            _repo = new UserRepository();
        }

        public User GetByUsername(string username) => _repo.GetByUsername(username);

        public int Create(User user) => _repo.Create(user);
    }
}
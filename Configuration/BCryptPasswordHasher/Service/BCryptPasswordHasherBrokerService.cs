using BCryptPasswordHasher.Interface;
using Common.Interface;

namespace BCryptPasswordHasher.Service
{
    public class BCryptPasswordHasherBrokerService : IBCryptPasswordHasherBrokerService, IScopedService
    {
        public string Hash(string password) => BCrypt.Net.BCrypt.HashPassword(password);

        public bool Verify(string password, string hash) => BCrypt.Net.BCrypt.Verify(password, hash);
    }
}
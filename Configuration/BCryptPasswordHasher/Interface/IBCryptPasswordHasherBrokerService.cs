namespace BCryptPasswordHasher.Interface
{
    public interface IBCryptPasswordHasherBrokerService
    {
        string Hash(string password);
        bool Verify(string password, string hash);
    }
}
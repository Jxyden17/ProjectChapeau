namespace ProjectChapeau.Services.Interfaces
{
    public interface IPasswordService
    {
        string GenerateSalt();
        string InterleaveSalt(string password, string salt);
        string HashPassword(string input);
    }
}

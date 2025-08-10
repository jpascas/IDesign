namespace IDesign.Access.Identity
{
    public interface IPasswordHasher
    {
        string Hash(string password);
        bool VerifyHashedPassword(string hashedPassword, string providedPassword);
    }
}

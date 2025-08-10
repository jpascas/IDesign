using IDesign.Access.Entities;

namespace IDesign.Access.Identity.JWT
{
    public interface IJwtProvider
    {
        string Generate(User user);
    }
}

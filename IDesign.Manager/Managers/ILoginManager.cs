using IDesign.Manager.Models;

namespace IDesign.Manager
{
    public interface ILoginManager
    {
        Task<OperationResult<string>> HandleLoginAsync(LoginUserDto dto);
    }
}
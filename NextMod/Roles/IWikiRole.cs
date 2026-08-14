using MiraAPI.Roles;

namespace NEXT.Roles
{
    public interface IWikiRole : ICustomRole
    {
        string WikiDescription { get; }
    }
}
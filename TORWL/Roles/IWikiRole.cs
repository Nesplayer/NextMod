using MiraAPI.Roles;

namespace TORWL.Roles
{
    public interface IWikiRole : ICustomRole
    {
        string WikiDescription { get; }
    }
}
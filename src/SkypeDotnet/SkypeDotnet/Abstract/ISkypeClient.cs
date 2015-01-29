using SkypeDotnet.Model;

namespace SkypeDotnet.Abstract
{
    public interface ISkypeClient
    {
        SkypeSelfProfile GetSelfProfile();
    }
}
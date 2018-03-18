using ServerCore.Models;

namespace ServerCore
{
    public interface IAddNodePlugin
    {
        void AfterNodeAdded(Node node);
        bool Validate(Node node);
    }
}
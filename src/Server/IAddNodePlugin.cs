using Server.Models;

namespace Server
{
    public interface IAddNodePlugin
    {
        void AfterNodeAdded(Node node);
        bool Validate(Node node);
    }
}
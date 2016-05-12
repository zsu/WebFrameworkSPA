using System.Collections.Generic;

namespace App.Common.SessionMessage
{
    public interface ISessionMessageProvider
    {
        void SetMessage(SessionMessage message);
        //void SetMessage(MessageType messageType, MessageBehaviors behavior, string message);
        //void SetMessage(MessageType messageType, MessageBehaviors behavior, string message,string key);
        //void SetMessage(MessageType messageType, MessageBehaviors behavior, string message, MessageButton? messageButtons);
        //void SetMessage(MessageType messageType, MessageBehaviors behavior, string message, string key,string caption, MessageButton? messageButtons, MessageIcon? messageIcon);
        List<SessionMessage> GetMessage();
        void Clear();
    }
}

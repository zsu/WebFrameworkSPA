/// Author: Zhicheng Su
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace App.Common.SessionMessage
{
    public class SessionStateSessionMessageProvider:ISessionMessageProvider
    {
        private const string SessionMessageKey = "SessionMessage";
        #region ISessionMessageProvider Members

        public void SetMessage(SessionMessage message)
        {
            if (message == null)
                return;
            List<SessionMessage> messages = GetMessage();
            if (messages == null)
                messages = new List<SessionMessage>();
            if (!string.IsNullOrEmpty(message.Key) && messages.Exists(x => x.Key == message.Key && x.Behavior == message.Behavior))
                return;
            messages.Add(message);
            HttpContext.Current.Session[SessionMessageKey] = messages;
        }
        //public void SetMessage(MessageType messageType, MessageBehaviors behavior, string message)
        //{
        //    SetMessage(messageType, behavior, message, null,null, null, null);
        //}
        //public void SetMessage(MessageType messageType, MessageBehaviors behavior, string message,string key)
        //{
        //    SetMessage(messageType, behavior, message, key,null, null, null);
        //}
        //public void SetMessage(MessageType messageType, MessageBehaviors behavior, string message, MessageButton? messageButtons)
        //{
        //    SetMessage(messageType, behavior, message, null,null, messageButtons, null);
        //}
        //public void SetMessage(MessageType messageType, MessageBehaviors behavior, string message,string key, string caption, MessageButton? messageButtons, MessageIcon? messageIcon)
        //{
        //    if (caption == null || caption.Trim() == string.Empty)
        //        caption = messageType.ToString();
        //    if (!messageButtons.HasValue)
        //        messageButtons = MessageButton.Ok;
        //    if (!messageIcon.HasValue)
        //    {
        //        switch (messageType)
        //        {
        //            case MessageType.Error:
        //                messageIcon = MessageIcon.Error;
        //                break;
        //            case MessageType.Warning:
        //                messageIcon = MessageIcon.Warning;
        //                break;
        //            case MessageType.Info:
        //                messageIcon = MessageIcon.Information;
        //                break;
        //        }
        //    }
        //    SessionMessage sessionMessage = new SessionMessage(messageType, behavior, message,key, caption, messageButtons, messageIcon);
        //    SetMessage(sessionMessage);
        //}
        public List<SessionMessage> GetMessage()
        {
            List<SessionMessage> sessionMessages = HttpContext.Current.Session[SessionMessageKey] as List<SessionMessage>;
            return sessionMessages;
        }

        public void Clear()
        {
            HttpContext.Current.Session[SessionMessageKey] = null;
        }

        #endregion
    }
}

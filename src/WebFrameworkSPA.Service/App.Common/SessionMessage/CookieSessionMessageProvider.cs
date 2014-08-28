/// Author: Zhicheng Su
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Runtime.Serialization.Json;
using System.IO;

namespace App.Common.SessionMessage
{
    public class CookieSessionMessageProvider:ISessionMessageProvider
    {
        private const string SessionMessageKey = "SessionMessage";
        #region ISessionMessageProvider Members

        public void SetMessage(SessionMessage message)
        {
            if (message == null)
                return;
            string json = null;
            List<SessionMessage> messages = GetMessage();
            if (messages == null)
                messages = new List<SessionMessage>();
            if (!string.IsNullOrEmpty(message.Key) && messages.Exists(x => x.Key == message.Key && x.Behavior==message.Behavior))
                return;
            messages.Add(message);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(List<SessionMessage>));
            using (MemoryStream ms = new MemoryStream())
            {
                ser.WriteObject(ms, messages);
                json = Encoding.Default.GetString(ms.ToArray());
                ms.Close();
            }
            HttpContext.Current.Response.Cookies[SessionMessageKey].Value=json;
        }
        //public void SetMessage(MessageType messageType, MessageBehaviors behavior, string message)
        //{
        //    SetMessage(messageType, behavior, message, null, null, null, null);
        //}
        //public void SetMessage(MessageType messageType, MessageBehaviors behavior, string message,string key)
        //{
        //    SetMessage(messageType, behavior, message, key,null, null, null);
        //}
        //public void SetMessage(MessageType messageType, MessageBehaviors behavior, string message, MessageButton? messageButtons)
        //{
        //    SetMessage(messageType, behavior, message, null,null, messageButtons, null);
        //}
        //public void SetMessage(MessageType messageType, MessageBehaviors behavior, string message, string key,string caption, MessageButton? messageButtons, MessageIcon? messageIcon)
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
        //    SessionMessage sessionMessage = new SessionMessage(messageType, behavior, message, key, caption, messageButtons, messageIcon);
        //    SetMessage(sessionMessage);
        //}
        public List<SessionMessage> GetMessage()
        {
            List<SessionMessage> message = null;
            HttpCookie cookie = HttpContext.Current.Request.Cookies[SessionMessageKey];
            if (cookie != null)
            {
                string cookieValue = cookie.Value;
                using (MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(cookieValue)))
                {
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(List<SessionMessage>));
                    message = ser.ReadObject(ms) as List<SessionMessage>;
                    ms.Close();
                }
            }
            return message;
        }

        public void Clear()
        {
            HttpCookie cookie = HttpContext.Current.Response.Cookies[SessionMessageKey];
            if (cookie != null)
                cookie.Expires=DateTime.Now.AddMinutes(-30); ;
        }

        #endregion
    }
}

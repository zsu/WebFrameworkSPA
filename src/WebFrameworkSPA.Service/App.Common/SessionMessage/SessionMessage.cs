/// Author: Zhicheng Su
using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Runtime.Serialization;
using System.Collections.Generic;
using App.Common.InversionOfControl;
namespace App.Common.SessionMessage
{
    public class SessionMessageManager
    {
        private const string SessionMessageKey = "SessionMessage";

        private SessionMessageManager() { }
        public static List<SessionMessage> GetMessage()
        {
            ISessionMessageProvider provider = IoC.GetService<ISessionMessageProvider>();
            return provider.GetMessage();
        }
        public static void SetMessage(MessageType messageType, MessageBehaviors behavior, string message)
        {
            SetMessage(messageType, behavior, message, null,null,null,null);
        }
        /// <summary>
        /// Set message. message with key only display once not matter how many ajax calls on the same page.
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="behavior"></param>
        /// <param name="message"></param>
        /// <param name="key"></param>
        public static void SetMessage(MessageType messageType, MessageBehaviors behavior, string message,string key)
        {
            SetMessage(messageType, behavior, message, key, null, null, null);
        }
        public static void SetMessage(MessageType messageType, MessageBehaviors behavior, string message, MessageButton? messageButtons)
        {
            SetMessage(messageType, behavior, message,null,null, messageButtons, null);
        }
        public static void SetMessage(MessageType messageType, MessageBehaviors behavior, string message, string key,string caption, MessageButton? messageButtons, MessageIcon? messageIcon)
        {
            if (caption == null || caption.Trim() == string.Empty)
                caption = messageType.ToString();
            if (!messageButtons.HasValue)
                messageButtons = MessageButton.Ok;
            if (!messageIcon.HasValue)
            {
                switch (messageType)
                {
                    case MessageType.Error:
                        messageIcon = MessageIcon.Error;
                        break;
                    case MessageType.Warning:
                        messageIcon = MessageIcon.Warning;
                        break;
                    case MessageType.Info:
                        messageIcon = MessageIcon.Information;
                        break;
                }
            }
            SessionMessage sessionMessage = new SessionMessage(messageType, behavior, message, key,caption, messageButtons, messageIcon);
            ISessionMessageProvider provider = IoC.GetService<ISessionMessageProvider>();
            provider.SetMessage(sessionMessage);
        }
        public static void Clear()
        {
            ISessionMessageProvider provider = IoC.GetService<ISessionMessageProvider>();
            provider.Clear();
        }
    }
    /// <summary>
    /// Summary description for SessionMessage
    /// </summary>
    [DataContract]
    public class SessionMessage
    {
        [DataMember]
        public string Caption
        {
            get;set;
        }
        [DataMember]
        public string Message
        {
            get;set;
        }
        [DataMember]
        public MessageType Type
        {
            get;set;
        }
        [DataMember]
        public MessageBehaviors Behavior
        {
            get;set;
        }
        [DataMember]
        public MessageButton? Buttons
        {
            get;set;
        }
        [DataMember]
        public MessageIcon? Icon
        {
            get;set;
        }
        [DataMember]
        public string Key { get; set; }
        public SessionMessage(MessageType messageType, MessageBehaviors behavior, string message)
            : this(messageType, behavior, message, null,null, null, null)
        {
        }
        public SessionMessage(MessageType messageType, MessageBehaviors behavior, string message, string key,string caption, MessageButton? messageButtons, MessageIcon? messageIcon)
        {
            if (behavior == MessageBehaviors.Modal && (!messageButtons.HasValue || !messageIcon.HasValue))
            {
                messageButtons = messageButtons ?? MessageButton.Ok;
                if(!messageIcon.HasValue)
                {
                    switch(messageType)
                    {
                        case MessageType.Error:
                            messageIcon = MessageIcon.Error;
                            break;
                        case MessageType.Info:
                            messageIcon = MessageIcon.Information;
                            break;
                        case MessageType.Success:
                            messageIcon = MessageIcon.Success;
                            break;
                        case MessageType.Warning:
                            messageIcon = MessageIcon.Warning;
                            break;
                        default:
                            messageIcon = MessageIcon.Information;
                            break;
                    }
                }
            }
            Key = key;
            Message = message;
            Caption = caption;
            Type = messageType;
            Behavior = behavior;
            Buttons = messageButtons;
            Icon = messageIcon;
        }
    }

    public enum MessageButton
    {
        Ok = 0
        //OKCancel = 1,
        //AbortRetryIgnore = 2,
        //YesNoCancel = 3,
        //YesNo = 4,
        //RetryCancel = 5
    }
    public enum MessageIcon
    {
		None = 0,
		Error = 1,
		Hand = 2,
		Stop = 3,
		Lock = 4,
		Question = 5,
		Exclamation = 6,
		Warning = 7,
		Asterisk = 8,
		Information = 9,
		Success = 10
    }

    [Flags]
    public enum MessageBehaviors
    {
        StatusBar = 1,
        Modal = 2
    }
    public enum MessageType
    {
        Error = 1,
        Warning = 2,
        Info = 3,
		Success = 4
    }
}



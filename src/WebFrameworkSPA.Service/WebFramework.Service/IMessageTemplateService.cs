using System;
using System.Collections.Generic;
using System.Linq;
using WebFramework.Data.Domain;
namespace Service
{
    public interface IMessageTemplateService:IService
    {
        IQueryable<MessageTemplate> Query();
        bool Exists(string name);
        void Add(MessageTemplate item);
        bool Delete(Guid id);
        bool Delete(MessageTemplate item);
        List<MessageTemplate> GetAll();
        MessageTemplate GetById(Guid id);
        MessageTemplate GetByName(string name);
        void Update(MessageTemplate item);
    }
}

namespace BrockAllen.MembershipReboot.Nh.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class NhGroupRepository<TGroup> : QueryableGroupRepository<TGroup>
        where TGroup : NhGroup
    {
        private readonly App.Common.Data.IRepository<TGroup,Guid> groupRepository;

        public NhGroupRepository(App.Common.Data.IRepository<TGroup,Guid> groupRepository)
        {
            this.groupRepository = groupRepository;
        }

        protected override IQueryable<TGroup> Queryable
        {
            get
            {
                return this.groupRepository.Query;//.FindAll();
            }
        }

        public override TGroup Create()
        {
            var group = Activator.CreateInstance<TGroup>();
            return group;
        }

        public override void Add(TGroup item)
        {
            this.groupRepository.Add(item);//.Save(item);
        }

        public override void Remove(TGroup item)
        {
            this.groupRepository.Delete(item);

        }

        public override void Update(TGroup item)
        {

            this.groupRepository.Update(item);

        }

        public override IEnumerable<TGroup> GetByChildID(Guid childGroupID)
        {
            var query =
                from g in this.groupRepository.Query//.FindAll()
                from c in g.ChildrenCollection
                where c.ChildGroupID == childGroupID
                select g;
            return query;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using EzBilling.Models;

namespace EzBilling.Database
{
    public sealed class ClientRepository : Repository<Client>
    {
        public ClientRepository(DbContext model) : base(model)
        {
        }

        public override void InsertOrUpdate(Client entity)
        {
            if (entity.ClientId == default(int))
            {
                Model.Set<Client>().Add(entity);
            }
            else
            {
                Model.Entry(entity).State = EntityState.Modified;
            }
        }
    }
}

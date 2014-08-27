using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using EzBilling.Models;

namespace EzBilling.Database
{
    public sealed class BillRepository : Repository<Bill>
    {
        public BillRepository(DbContext model) : base(model)
        {
        }

        public override void InsertOrUpdate(Bill entity)
        {
            if (entity.BillId == default(int))
            {
                Model.Set<Bill>().Add(entity);
            }
            else
            {
                Model.Entry(entity).State = EntityState.Modified;
            }
        }
    }
}

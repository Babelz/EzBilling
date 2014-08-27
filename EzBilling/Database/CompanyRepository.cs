using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using EzBilling.Models;

namespace EzBilling.Database
{
    public sealed class CompanyRepository : Repository<Company>
    {

        
        public CompanyRepository(DbContext model) : base(model)
        {
        }

        public override void InsertOrUpdate(Company entity)
        {
            // if there isn't company
            if (!Find(c => c.CompanyId == entity.CompanyId).Any())
            {
                Model.Set<Company>().Add(entity);
            }
            else
            {
                Model.Entry(entity).State = EntityState.Modified;
            }
        }
    }
}

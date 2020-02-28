using ESU.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESU.Data
{
    public class HostProvider
    {
        private readonly Data.ESUContext context;

        public HostProvider(Data.ESUContext context)
        {
            this.context = context;
        }

        public async Task<List<Host>> LoadHostAsync(HostFiltringParameters filtringParameters = null)
        {
            var query = this.GetHostQuery(filtringParameters);
            return await query.ToListAsync();
        }

        public List<Host> LoadHost(HostFiltringParameters filtringParameters = null)
        {
            return this.LoadHostAsync(filtringParameters).Result;
        }

        private IQueryable<Host> GetHostQuery(HostFiltringParameters filtringParameters = null)
        {
            var query = this.context.Hosts.AsQueryable();
            if (filtringParameters != null)
            {
                if (filtringParameters.MinDate > DateTime.MinValue)
                {
                    query = query.Where(x => x.SubscriptionDate >= filtringParameters.MinDate);
                }

                if (!string.IsNullOrEmpty(filtringParameters.Name))
                {
                    query = query.Where(x => x.Name.StartsWith(filtringParameters.Name));
                }

                if (!string.IsNullOrEmpty(filtringParameters.Mail))
                {
                    query = query.Where(x => x.Mail.StartsWith(filtringParameters.Mail));
                }
                
                if(filtringParameters.Offset > 0)
                {
                    query = query.Skip(filtringParameters.Offset);
                }

                if (filtringParameters.Limit > 0)
                {
                    query = query.Take(filtringParameters.Limit);
                }
            }

            return query
                .Include(x => x.ProcessingStatus)
                .Include(x => x.Licenses)
                .ThenInclude(l => l.Confirmations);
        }
    }
}

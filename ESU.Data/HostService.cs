using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESU.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ESU.Data
{
    public class HostService
    {
        private readonly Data.ESUContext context;

        public HostService(Data.ESUContext context)
        {
            this.context = context;
        }

        public async Task<List<Host>> LoadHostAsync(HostFilteringParameters filtringParameters = null)
        {
            var query = this.GetHostQuery(filtringParameters);
            return await query.ToListAsync();
        }

        public List<Host> LoadHost(HostFilteringParameters filtringParameters = null)
        {
            return this.LoadHostAsync(filtringParameters).Result;
        }

        private IQueryable<Host> GetHostQuery(HostFilteringParameters filtringParameters = null)
        {
            var query = this.context.Hosts.AsQueryable();
            if (filtringParameters == null)
            {
                return query
               .Include(x => x.ProcessingStatus)
               .Include(x => x.Licenses)
               .ThenInclude(l => l.Confirmations);
            }

            if (filtringParameters.MinDate > DateTime.MinValue)
            {
                query = query.Where(x => x.SubscriptionDate >= filtringParameters.MinDate);
                if (filtringParameters.MaxDate >= filtringParameters.MinDate)
                {
                    query = query.Where(x => x.SubscriptionDate <= filtringParameters.MaxDate);
                }
            }

            if (!string.IsNullOrEmpty(filtringParameters.Name))
            {
                query = query.Where(x => x.Name.StartsWith(filtringParameters.Name));
            }

            if (!string.IsNullOrEmpty(filtringParameters.Mail))
            {
                query = query.Where(x => x.Mail.StartsWith(filtringParameters.Mail));
            }

            if (!string.IsNullOrEmpty(filtringParameters.Site))
            {
                query = query.Where(x => x.Site.StartsWith(filtringParameters.Site));
            }

            if (!string.IsNullOrEmpty(filtringParameters.Entity))
            {
                query = query.Where(x => x.Site.StartsWith(filtringParameters.Entity));
            }

            if (!string.IsNullOrEmpty(filtringParameters.Network))
            {
                query = query.Where(x => x.Network.StartsWith(filtringParameters.Network));
            }

            if (filtringParameters.Offset > 0)
            {
                query = query.Skip(filtringParameters.Offset);
            }

            if (filtringParameters.Limit > 0)
            {
                query = query.Take(filtringParameters.Limit);
            }

            if (filtringParameters.WithStatus)
            {
                query = query.Include(x => x.ProcessingStatus);
            }

            if (filtringParameters.WithLicenses)
            {
                var temp = query.Include(x => x.Licenses).ThenInclude(x => x.Activation);
                if (filtringParameters.WithConfirmations)
                {
                    query = temp.Include(x => x.Licenses).ThenInclude(l => l.Confirmations);
                }
                else
                {
                    query = temp;
                }
            }



            return query;
        }

        public int Count()
        {
            return this.context.Hosts.Count();
        }

        public Task<int> CountAsync()
        {
            return this.context.Hosts.CountAsync();
        }

        public Host Find(int id)
        {
            return this.context.Hosts.Find(id);
        }

        public Task<Host> FindAsync(int id)
        {
            return this.context.Hosts
                .Include(x => x.Licenses)
                .ThenInclude(x => x.Confirmations)
                .Include(x=>x.Licenses)
                .ThenInclude(x=>x.Activation)
                .Include(x => x.ProcessingStatus)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}

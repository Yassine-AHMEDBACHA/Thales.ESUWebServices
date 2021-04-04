﻿using ESU.DashbordWS.Extensions;
using ESU.DashbordWS.Infrastructures;
using ESU.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESU.DashbordWS.Core
{
    public class HostService
    {
        public HostService(ESUContext context)
        {
            this.Context = context;
        }

        internal ESUContext Context { get; }

        internal async Task<IEnumerable<Host>> GetAsync(HostFilteringParameters parameters)
        {
            var query = this.BuildQuery(parameters);
            return await query.ToListAsync();
        }

        internal async Task<int> GetCountAsync(HostFilteringParameters parameters)
        {
            var query = this.BuildQuery(parameters);
            return await query.CountAsync();
        }

        private IQueryable<Host> BuildQuery(HostFilteringParameters parameters)
        {
            var query = this.Context.Hosts.AsQueryable();
            if(parameters == null)
            {
                return query;
            }

            if (parameters.ProductKeyId > 0)
            {
                query = query.Where(x => x.ProductKeyId.Equals(parameters.ProductKeyId));
            }

            if (parameters.Name.IsValid())
            {
                query = query.Where(x => x.Name.StartsWith(parameters.Name));
            }

            if(parameters.Network.IsValid())
            {
                query = query.Where(x => x.Network.StartsWith(parameters.Network));
            }

            if (parameters.Site.IsValid())
            {
                query = query.Where(x => x.Site.StartsWith(parameters.Site));
            }

            if (parameters.Entity.IsValid())
            {
                query = query.Where(x => x.Identity.StartsWith(parameters.Entity));
            }

            if (parameters.Mail.IsValid())
            {
                query = query.Where(x => x.Mail.StartsWith(parameters.Mail));
            }

            if (parameters.Offset > 0)
            {
                query = query.Skip(parameters.Offset);
            }

            if (parameters.Limit > 0)
            {
                query = query.Take(parameters.Limit);
            }

            return query;
        }

        

    }
}

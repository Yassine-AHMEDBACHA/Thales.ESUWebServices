using ESU.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace ESU.Monitoring.Core
{
    public class HostAnalyzer
    {
        public IEnumerable<string> GetHostTrace(Host host)
        {
            yield return "Host found on the server with id " + host.Id;
            if (host.Licenses?.Count > 0)
            {
                yield return "Product Key installed on the host";
                foreach (var license in host.Licenses)
                {
                    var confirmation = license?.Confirmations.FirstOrDefault(x => x.Status == Status.Success);
                    if (confirmation != null)
                    {
                        yield return "Confirmation key retreived";
                        var status = host.ProcessingStatus.FirstOrDefault(x => x.Message.ToLowerInvariant().Contains("activated"));
                        if (status != null)
                        {
                            yield return "Host activated";
                        }
                        else
                        {
                            yield return "Wainting Confirmation key installation";
                        }
                    }
                    else
                    {
                        yield return "Confirmation key not available";
                    }
                }
            }
            else
            {
                if (host.ProcessingStatus?.Count < 1)
                {
                    yield return "No more informations are available.";
                }
                else
                {
                    foreach (var message in host.ProcessingStatus.GroupBy(x => x.Message).Select(g => g.Key))
                    {
                        yield return message;
                    }
                }
            }
        }
    }
}

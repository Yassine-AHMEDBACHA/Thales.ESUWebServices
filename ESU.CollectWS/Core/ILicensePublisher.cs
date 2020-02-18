using ESU.Data.Models;
using System.Threading.Tasks;

namespace ESU.CollectWS.Core
{
    public interface ILicensePublisher
    {
        void Publish(License license);
    }
}
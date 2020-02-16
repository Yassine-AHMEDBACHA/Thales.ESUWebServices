using ESU.Data.Models;

namespace ESU.CollectWS.Core
{
    public interface ILicensePublisher
    {
        void Publish(License license);
    }
}
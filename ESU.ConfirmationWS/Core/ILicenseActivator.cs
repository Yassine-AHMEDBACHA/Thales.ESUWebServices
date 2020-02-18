using ESU.Data.Models;

namespace ESU.ConfirmationWS.Core
{
    public interface ILicenseActivator
    {
        void Append(License license);
    }
}
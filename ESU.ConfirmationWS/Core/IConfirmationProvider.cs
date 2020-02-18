using ESU.Data.Models;

namespace ESU.ConfirmationWS.Core
{
    public interface IConfirmationProvider
    {
        Confirmation GetConfirmation(string installationId, string extendedProductId);
    }
}
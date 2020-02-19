namespace ESU.ActivationWS.Core
{
    public interface IActivationHelper
    {
        string RequestConfirmationKey(string installationId, string extendedProductId);
    }
}
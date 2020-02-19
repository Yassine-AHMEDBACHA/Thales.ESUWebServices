using ESU.Data.Models;
using System;

namespace ESU.ConfirmationWS.Core
{
    public interface ILicenseActivator
    {
        DateTime LastRun { get; }

        void Append(License license);
    }
}
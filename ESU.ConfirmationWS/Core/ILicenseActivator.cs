using ESU.Data.Models;
using System;

namespace ESU.ConfirmationWS.Core
{
    public interface ILicenseActivator
    {
        DateTime LastRun { get; }

        DateTime FirstRun { get; }

        void Append(License license);
    }
}
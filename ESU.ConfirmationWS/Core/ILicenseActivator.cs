using ESU.Data.Models;
using System;

namespace ESU.ConfirmationWS.Core
{
    public interface ILicenseActivator
    {
        DateTime LastRun { get; }

        DateTime FirstRun { get; }

        int LastCount { get; }

        int Total { get; }

        string LastKey { get; }

        string Step { get; }

        void Append(License license);


    }
}
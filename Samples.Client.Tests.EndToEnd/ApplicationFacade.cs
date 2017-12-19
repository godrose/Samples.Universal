﻿using FlaUI.Core;
using LogoFX.Client.Testing.Contracts;

namespace Samples.Client.Tests.EndToEnd
{
    public class ApplicationFacade : IApplicationFacade
    {
        /// <summary>Starts the application.</summary>
        /// <param name="startupPath">The startup path.</param>
        public void Start(string startupPath)
        {
            ApplicationContext.Application = Application.Launch(startupPath);
            ApplicationContext.Application.WaitWhileBusy();
        }

        public void Stop()
        {
            throw new System.NotImplementedException();
        }
    }
}
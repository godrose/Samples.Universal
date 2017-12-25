﻿using System.IO;
using LogoFX.Client.Testing.Contracts;
using Samples.Client.Tests.Contracts;

namespace Samples.Client.Tests.EndToEnd
{
    class StartClientApplicationService : IStartClientApplicationService
    {        
        private readonly IStartApplicationService _startApplicationService;
        private readonly IExecutableContainer _executableContainer;

        public StartClientApplicationService(
            IStartApplicationService startApplicationService,
            IExecutableContainer executableContainer)
        {
            _startApplicationService = startApplicationService;
            _executableContainer = executableContainer;
        }

        public void StartApplication()
        {
            var testDirectory = Directory.GetCurrentDirectory();
            var applicationDirectory = Directory.GetParent(testDirectory).FullName;
            var applicationPath = Path.Combine(applicationDirectory, _executableContainer.Path);
            Directory.SetCurrentDirectory(applicationDirectory);
            //TODO: replace with app Id
            _startApplicationService.StartApplication(applicationPath);
            Directory.SetCurrentDirectory(testDirectory);
        }
    }    
}

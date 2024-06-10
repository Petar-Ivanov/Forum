﻿using Microsoft.Extensions.Logging;

namespace ApplicationServices.Implementations
{
    public class BaseManagementService
    {
        protected readonly ILogger _logger;

        public BaseManagementService(ILogger logger)
        {
            _logger = logger;
        }
    }
}

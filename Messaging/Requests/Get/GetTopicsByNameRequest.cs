﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Requests.Get
{
    public class GetTopicsByNameRequest : TermSearchRequestBase
    {
        public GetTopicsByNameRequest(string searchTerm) : base(searchTerm)
        {
        }
    }
}
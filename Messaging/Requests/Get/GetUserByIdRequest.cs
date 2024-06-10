﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Requests.Get
{
    public class GetUserByIdRequest: IntegerServiceRequestBase
    {
        public GetUserByIdRequest(int id) : base(id)
        {
        }
    }
}

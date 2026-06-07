using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Common.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string msg) : base(msg)
        {
            
        }
    }
}

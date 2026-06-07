using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Common.Exceptions
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException(string msg) : base(msg)
        {
            
        }
    }
}

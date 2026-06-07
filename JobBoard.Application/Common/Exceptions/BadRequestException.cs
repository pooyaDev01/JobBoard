using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Common.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string msg) : base(msg)
        {
            
        }
    }
}

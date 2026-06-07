using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string msg) : base(msg)
        {
            
        }
    }
}

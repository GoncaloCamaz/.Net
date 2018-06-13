using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portelinha.Exceptions
{
    public class DadosInvalidosException : Exception
    {
        public DadosInvalidosException()
        {

        }

        public DadosInvalidosException(string message)
            : base(message)
        {
   
        }
    }
}
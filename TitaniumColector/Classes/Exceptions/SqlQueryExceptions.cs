using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace TitaniumColector.Classes.Exceptions
{
    class SqlQueryExceptions :Exception 
    {

        public SqlQueryExceptions()
        {
        }

        public SqlQueryExceptions(string message)
            : base(message)
        {

        }

        public SqlQueryExceptions(string message, Exception inner)
            : base(message, inner)
        {

        }

    }

    class QuantidadeInvalidaException : Exception 
    {
        public QuantidadeInvalidaException()
        {

        }

        public QuantidadeInvalidaException(string message)
            : base(message)
        {

        }

        public QuantidadeInvalidaException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }

    class NoNewPropostaException : Exception
    {
        public NoNewPropostaException()
        {

        }

        public NoNewPropostaException(string message)
            : base(message)
        {

        }

        public NoNewPropostaException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }

}

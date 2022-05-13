using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Exception {
    public class OrderingDomainException : SystemException {
        public OrderingDomainException() { }

        public OrderingDomainException(string message)
            : base(message) { }

        public OrderingDomainException(string message, SystemException innerException)
            : base(message, innerException) { }
    }
}

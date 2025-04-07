using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Application.DTOs
{
    public class AddressDto
    {
        public string Street { get; init; }
        public string City { get; init; }
        public string PostalCode { get; init; }
        public string Country { get; init; }
        public string Phone { get; init; }
    }
}

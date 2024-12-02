using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Application.Dtos
{
    public class MiscDto
    {
        public BaseDto User { get; set; }
        public BaseDto Business { get; set; }
        public BaseDto Vehicle { get; set; }

    }

    public class BaseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedKernel
{
    public abstract class BaseEntity
    {
        public int Id { get; protected set; }
        public DateTime CreatedDate { get; private set; }
        public DateTime? ModifiedDate { get; private set; }

        public BaseEntity()
        {
            CreatedDate = DateTime.UtcNow;
        }

        public void SetModifiedDate()
        {
            ModifiedDate = DateTime.UtcNow;
        }
    }
}

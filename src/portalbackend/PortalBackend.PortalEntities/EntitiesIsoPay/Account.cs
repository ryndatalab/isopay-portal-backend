using Org.Eclipse.TractusX.Portal.Backend.PortalBackend.PortalEntities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  PortalBackend.PortalEntities.EntitiesIsoPay
{
    public class Account : IBaseEntity
    {
        public Guid Id { get; set; }
         
        [StringLength(36)]
        public string AccNumber { get; set; }

        public decimal Balance { get; set; }
        public virtual Currency Currency { get; set; }
        public virtual AccountHead AccountHead { get; set; }

    }
}

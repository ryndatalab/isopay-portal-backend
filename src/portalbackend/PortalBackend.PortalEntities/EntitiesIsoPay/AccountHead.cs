using System.ComponentModel.DataAnnotations; 

namespace PortalBackend.PortalEntities.EntitiesIsoPay
{
    public class AccountHead
    {
        private AccountHead()
        {
            Label = null!;
        }

        public AccountHead(Guid accountHeadId) : this()
        {
            Id = accountHeadId;
            Label = accountHeadId.ToString();
        }

        public Guid Id { get; private set; }

        [MaxLength(255)]
        public string Label { get; private set; }

    }
}


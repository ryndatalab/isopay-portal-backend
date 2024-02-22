using System.ComponentModel.DataAnnotations; 

namespace PortalBackend.PortalEntities.EntitiesIsoPay
{
    public class Currency
    {
        private Currency()
        {
            Label = null!;
        }

        public Currency(Guid currencyId) : this()
        {
            Id = currencyId;
            Label = currencyId.ToString();
        }

        public Guid Id { get; private set; }

        [MaxLength(255)]
        public string Label { get; private set; }

    }
}


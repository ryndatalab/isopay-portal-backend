using PortalBackend.PortalEntities.Enums.IsoPay;
using System.ComponentModel.DataAnnotations; 

namespace PortalBackend.PortalEntities.EntitiesIsoPay
{
    public class TransactionType
    {
        private TransactionType()
        {
            Label = null!;
        }

        public TransactionType(TransactionTypeId transactionTypeId) : this()
        {
            Id = transactionTypeId;
            Label = transactionTypeId.ToString();
        }

        public TransactionTypeId Id { get; private set; }

        [MaxLength(255)]
        public string Label { get; private set; }

    }
}


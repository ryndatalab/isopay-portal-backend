using System.ComponentModel.DataAnnotations; 

namespace PortalBackend.PortalEntities.EntitiesIsoPay
{
    public class Transaction
    {
        private Transaction()
        {
            //Console.WriteLine($"Started acc:{acc} amt: {amt} bal: {amtBal}{currency} trasaction: {transactionType}");
            //
        }

        public Transaction(Guid currencyId) : this()
        {
            Id = currencyId; 
        }

        public Guid Id { get; private set; }

        [MaxLength(255)]
        public DateTimeOffset OnDate { get; private set; }
        public  virtual Account AccCredit { get; private set; }
        public virtual Account AccDebit { get; private set; }
        public virtual Currency Currency { get; private set; }
        public decimal Amount { get; private set; }
        public virtual TransactionType TransactionType { get; private set; }
    }
}


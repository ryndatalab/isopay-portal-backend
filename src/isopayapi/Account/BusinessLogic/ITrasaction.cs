using System.ComponentModel.DataAnnotations;
using System.Transactions;

namespace Account.Service.BusinessLogic
{

    public record TransferDto(string SenderAcc, string RecieverAcc, string Currency, decimal Amt);

    //public record TransferDto (string senderAcc, string recieverAcc, decimal amt, string currency);
    public enum TransactionType
    {
        Transfer = 0, Credit = 1, Debit = 2, Adjust = 3
    }
    public interface ITransaction
    {
        Task Transfer(TransferDto transferDto);
    }
    public class Transaction : ITransaction
    {
        public async Task Transfer(TransferDto transferDto)
        {

            string senderAcc = transferDto.SenderAcc;
            string recieverAcc = transferDto.RecieverAcc;
            decimal amt = transferDto.Amt;
            string currency = transferDto.Currency;

            await Transact(senderAcc, amt, currency, TransactionType.Debit);
            await Transact(recieverAcc, amt, currency, TransactionType.Credit);
        }
        private async Task Transfer(string senderAcc, string recieverAcc, decimal amt, string currency)
        {
            await Transact(senderAcc, amt, currency, TransactionType.Debit);
            await Transact(recieverAcc, amt, currency, TransactionType.Credit);
        }
        private async Task Transact(string acc, decimal amt, string currency, TransactionType transactionType)
        {
            decimal amtBal = 100;
            Console.WriteLine($"Started acc:{acc} amt: {amt} bal: {amtBal}{currency} trasaction: {transactionType}");
            // transact
            switch (transactionType)
            {
                case TransactionType.Credit:
                    {
                        amtBal += amt;
                    }
                    break;
                case TransactionType.Debit:
                    amtBal -= amt;
                    break;
                case TransactionType.Adjust:
                case TransactionType.Transfer:
                default:
                    break;
            }
            Console.WriteLine($"Done acc:{acc} amt: {amt} bal: {amtBal}{currency}trasaction: {transactionType}");
        }
    }


}

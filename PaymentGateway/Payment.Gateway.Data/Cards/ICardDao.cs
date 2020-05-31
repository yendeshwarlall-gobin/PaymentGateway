namespace Payment.Gateway.Data.Cards
{
    public interface ICardDao
    {
        Card GetCardBasedOnNumberAndCVV(string cardNumber, string cardCvv);
        void InsertCardInfo(Card newCard);
        Card GetCardById(int cardId);
    }
}

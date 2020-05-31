using Payment.Gateway.Data.Cards;

namespace Payment.Gateway.Services.CardServices
{
    public interface ICardService
    {
        Card GetCardBasedOnNumberAndCVV(string cardNumber, string cardCvv);
        void InsertCardInfo(Card newCard);
    }
}

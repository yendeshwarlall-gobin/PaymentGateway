using Payment.Gateway.Data.Cards;

namespace Payment.Gateway.Services.CardServices
{
    public class CardService:ICardService
    {
        private readonly ICardDao _cardDao;

        public CardService(ICardDao cardDao)
        {
            this._cardDao = cardDao;
        }

        public Card GetCardBasedOnNumberAndCVV(string cardNumber, string cardCvv)
        {
            return _cardDao.GetCardBasedOnNumberAndCVV(cardNumber, cardCvv);
        }

        public void InsertCardInfo(Card newCard)
        {
            _cardDao.InsertCardInfo(newCard);
        }
    }
}

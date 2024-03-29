﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.BLL.Exceptions;
using MTCG.DAL;
using MTCG.Models;

namespace MTCG.BLL.Managers
{
    public class CardManager : ICardManager
    {

        private readonly ICardDao _cardDao;

        public CardManager(ICardDao cardDao)
        {
            _cardDao = cardDao;
        }

        public void CreateCard(Card card)
        {
            if (_cardDao.InsertCard(card) == false)
            {
                throw new DuplicateCardException();
            }
        }

        public void AquirePackage(User user, Package package)
        {
            if (user.Coins < 5)
            {
                throw new NotEnoughCoinsException();
            }
            foreach (Card card in package.Cards)
            {
                card.UId = user.Id;
                if (_cardDao.UpdateCardUId(card) == false)
                {
                    throw new CardNotFoundException();
                }
            }
            user.Coins -= 5;
        }

        public void FillCardsInPackage(Package package)
        {
            List<Card> _cards = new List<Card>();
            foreach (Card card in package.Cards)
            {
                Card? tmp = GetCardById(card.Id);
                if (tmp == null)
                {
                    throw new CardNotFoundException();
                }
                _cards.Add(tmp);
            }
            package.Cards = _cards.ToArray();
        }

        public List<Card> GetAllUsersCards(string uid)
        {
            return _cardDao.GetAllCardsByUId(uid);
        }

        public Card? GetCardById(string id)
        {
            return _cardDao.GetCardById(id) ?? throw new CardNotFoundException();
        }

        public void UpdateCardUId(Card card)
        {
            _cardDao.UpdateCardUId(card);
        }

        public bool DeleteCardById(string id) {
            return _cardDao.DeleteCardById(id);
        }
    }
}

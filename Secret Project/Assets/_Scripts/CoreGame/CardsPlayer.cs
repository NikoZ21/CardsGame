using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.CoreGame
{
    public class CardsPlayer : MonoBehaviour
    {
        [SerializeField] private Card[] playerCards = new Card[9];

        public void DealCards(List<Card> deckCards)
        {
            for (int i = 0; i < 9; i++)
            {
                playerCards[i] = deckCards[i];
                deckCards.RemoveAt(i);

                Debug.Log($"{playerCards[i].Suit}/{playerCards[i].CardR}");
            }
        }
    }
}
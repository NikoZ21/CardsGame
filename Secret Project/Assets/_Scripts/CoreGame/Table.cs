using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.CoreGame
{
    public class Table : MonoBehaviour
    {
        [SerializeField] private CardsPlayer[] players;
        [SerializeField] private Card cardPrefab;
        private string cardsFolderPath = "FrontOfCards";
        [SerializeField] private List<Card> deckOfCards = new List<Card>();
        private System.Random randomCard = new System.Random();

        private void Start()
        {
            SetUpCards();

            ShuffleDeck();

            foreach (var p in players)
            {
                p.DealCards(deckOfCards);
            }
        }

        private void SetUpCards()
        {
            var cardsFrontTextures = Resources.LoadAll<Texture2D>(cardsFolderPath);

            foreach (var frontT in cardsFrontTextures)
            {
                var suit = frontT.name.Split("_")[0];
                var rank = frontT.name.Split("_")[1];


                if (int.TryParse(rank, out int rankInt))
                {
                    if (rankInt < 6) continue;
                }

                var card = Instantiate(cardPrefab);

                card.CardR = (CardRank)rankInt;

                if (Enum.TryParse(suit, out CardSuit cS)) card.Suit = cS;

                Sprite temp = Sprite.Create(frontT, new Rect(0, 0, frontT.width, frontT.height),
                    new Vector2(0.5f, 0.5f));

                card.FrontSprite = temp;

                deckOfCards.Add(card);
            }
        }

        private void ShuffleDeck()
        {
            int n = deckOfCards.Count;

            for (int i = n - 1; i > 0; i--)
            {
                int j = randomCard.Next(0, i + 1);
                Card temp = deckOfCards[i];
                deckOfCards[i] = deckOfCards[j];
                deckOfCards[j] = temp;
            }
        }
    }
}
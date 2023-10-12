using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.CoreGame
{
    public class Card : MonoBehaviour
    {
        private CardSuit cardSuit = CardSuit.NotSet;
        public CardSuit Suit
        {
            get { return cardSuit; }
            set
            {
                if (cardSuit != value)
                {
                    cardSuit = value;

                    cardColor = cardSuit switch
                    {
                        CardSuit.Heart or CardSuit.Diamond => CardColor.Red,
                        CardSuit.Club or CardSuit.Spade => CardColor.Black,
                        _ => CardColor.NotSet
                    };
                }
            }
        }

        private CardColor cardColor;

        public CardRank CardR = CardRank.NotSet;

        private Sprite frontSprite;
        public Sprite FrontSprite
        {
            get { return frontSprite; }
            set
            {
                if (frontSprite != value || frontSprite == null)
                {
                    frontSprite = value;
                    if (frontSprite)
                    {
                        Debug.Log("It ain't null");
                        HandleSpriteChanged(frontSprite);
                    }


                    Debug.Log("Property running");
                }
            }
        }

        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void HandleSpriteChanged(Sprite sprite)
        {
            if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

            spriteRenderer.sprite = sprite;
        }
    }

    public enum CardColor
    {
        NotSet,
        Red,
        Black,
    }

    public enum CardRank
    {
        NotSet,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Jack = 11,
        Queen = 12,
        King = 13,
        Ace = 14
    }

    public enum CardSuit
    {
        NotSet,
        Club,
        Diamond,
        Heart,
        Spade
    }
}
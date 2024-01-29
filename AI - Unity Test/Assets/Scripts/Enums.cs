using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardValue
{
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Ten,
    Jack,
    Queen,
    King,
    Ace
}

public enum CardSuit
{
    Club,
    Diamond,
    Heart,
    Spade
}

public enum PokerGameState
{
    Deal,
    Draw
}

public enum GameMode
{
    Poker,
    BlackJack
}

public enum PokerSubGame
{
    JackOrBetter,
    BonusPoker,
    BonusPokerDeluxe,
    DoubleBonusPoker,
    DoubleDoubleBonusPoker,
    TripleDoubleBonusPoker,
    DeucesWildPoker,
    DeucesWildBonusPoker,
    JokerPoker
}

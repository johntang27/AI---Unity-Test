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
    Jack_Or_Better,
    Bonus_Poker,
    Bonus_Poker_Deluxe,
    Double_Bonus_Poker,
    Double_Double_Bonus_Poker,
    Triple_Double_Bonus_Poker,
    Deuces_Wild_Poker,
    Deuces_Wild_Bonus_Poker,
    Joker_Poker
}

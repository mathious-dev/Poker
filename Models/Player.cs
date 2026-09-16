namespace Poker.Models;

public class Player
{
    public string Name{get;set;}
    public  int Coin{get;set;}=1000;
    public Card[]? Deck{get;set;}=new Card[2];
    public void Bet(ref int mainPot,int AmountBet)
    {
        mainPot+=AmountBet;
        Coin-=AmountBet;
    }
    
}

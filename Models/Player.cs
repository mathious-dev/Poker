namespace Poker.Models;

public class Player
{
    public string Name{get;set;}
    public  int Coin{get;set;}=1000;
    public Card[]? Deck{get;set;}=new Card[2];
    public void Bet(ref int mainPot,int AmountBet)
    {
        mainPot+=AmountBet;
        Console.WriteLine($"\n Le joueur {this.Name} a misé {AmountBet}");
        Coin-=AmountBet;
    }
    public void PlayerSleep(List<Player>allPlayers)
    {
        allPlayers.Remove(this);
        Console.WriteLine($"\nLe joueur {this.Name} s'est couché");
    }
    
}

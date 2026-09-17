namespace Poker.Models;

public class Player
{
    public string Name{get;set;}
    public int Coin{get;set;}=1000;
    public int BetOfTheRound{get;set;}=0;
    public Card[]? Deck{get;set;}=new Card[2];
    public void Bet(ref int mainPot,int AmountBet)
    {
        mainPot+=AmountBet;
        BetOfTheRound+=AmountBet;
        Console.WriteLine($"\n Le joueur {this.Name} a misé {AmountBet}");
        Coin-=AmountBet;
    }
    public void PlayerSleep(List<Player>allPlayers)
    {
        allPlayers.Remove(this);
        Console.WriteLine($"\nLe joueur {this.Name} s'est couché");
    }
    public void UserCheckCard()
    {
        Console.WriteLine($"\nVos cartes sont ");
        foreach(Card card in this.Deck)
        {
            if(card.Number>10)
                Console.WriteLine($"\n{(FaceCard)card.Number} de {card.Type}");
            else
                Console.WriteLine($"\n{card.Number} de {card.Type}");
        }
    }
    public void EmptyTemporaryBet()
    {
        this.BetOfTheRound=0;
    }
    public void CheckBet()
    {
        Console.WriteLine($"\nVotre mise : {this.BetOfTheRound}");
    }
}

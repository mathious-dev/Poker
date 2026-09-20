namespace Poker.Models;

public class Player
{
    public string Name{get;set;}
    public int Coin{get;set;}=1000;
    public int BetOfTheRound{get;set;}=0;
    public Card[]? Deck{get;set;}=new Card[2];
    public bool allIn{get;set;}=false;
    public int Bet(int AmountBet)
    {
        BetOfTheRound+=AmountBet;
        Console.WriteLine($"\n Le joueur {this.Name} a misé {AmountBet}");
        if(AmountBet==Coin)
        {
            this.allIn=true;
            UserCheckCard();
        }
            
        Coin-=AmountBet;
        return AmountBet;
    }
    public void PlayerSleep(List<Player>allPlayers)
    {
        allPlayers.Remove(this);
        Console.WriteLine($"\nLe joueur {this.Name} s'est couché");
    }
    public void UserCheckCard()
    {
        Console.WriteLine($"\nles cartes du joueur {this.Name} sont ");
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
    public static List<Player> RemoveLoserPlayer(List<Player> players)
    {
        var newListPlayer=new List<Player>();
        foreach(Player player in players)
        {
            if(player.Coin>0)
                newListPlayer.Add(player);
            else
               Console.WriteLine($"\nLe joueur {player.Name} est éliminé"); 
        }
        return newListPlayer;
    }
    public virtual int PlayerAllIn()
    {
        int amountBet=Bet(this.Coin);
        Console.WriteLine("\nVous avez tout miser !" );
        this.allIn=true;
        UserCheckCard();
        return amountBet;
    }
    public void PlayerWin(int amountWin,string? combination,List<Card>mainCards)
    {
        var allCards=new List<Card>();
        allCards=Rule.GroupCards(this.Deck,mainCards);
        this.Coin+=amountWin;
        Console.WriteLine($"\nLe joueur {this.Name} a gagné avec {amountWin}");
        if(combination!=null)
            Console.WriteLine($"et avec la combinaison {combination}");
        
        Card.ShowCards(mainCards);
        Card.ShowCards(this.Deck);
        
    }
    public static void MultiplePLayersWin(int mainPot,List<Player>winners,string? combination,List<Card>mainCards)
    {
        int countPlayer=winners.Count();
        int amount=mainPot/countPlayer;
        foreach(Player winner in winners )
        {
            winner.PlayerWin(amount,combination,mainCards);
        }
    }
    public void FollowWithNoBet()
    {
        Console.WriteLine($"\nLe joueur {this.Name} suit");
    }
    public static void EveryPlayerInGameShowCard(List<Player> players)
    {
        foreach(Player player in players)
        {
            player.UserCheckCard();
        }
    }
    // public void FollowBet(int amount)
    // {
    //     Bet(amount);
    // }
}

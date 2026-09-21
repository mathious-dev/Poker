namespace Poker.Models;
using System;
public class Player
{
    public event Action<int> OnMoneyBet;
    public string Name{get;set;}
    public int Coin{get;set;}=1000;
    public int BetOfTheRound{get;set;}=0;
    public Card[]? Deck{get;set;}=new Card[2];
    public bool AllIn{get;set;}=false;
    public void Bet(int AmountBet)
    {
        BetOfTheRound+=AmountBet;
        Console.WriteLine($"\n Le joueur {this.Name} a misé {AmountBet}");
        if(AmountBet>=Coin)
            this.AllIn=true;
        Coin-=AmountBet;
        //événement
        OnMoneyBet?.Invoke(AmountBet);
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
    public virtual void PlayerAllIn()
    {
        Bet(this.Coin);
        Console.WriteLine("\nVous avez tout miser !" );
        this.AllIn=true;
    }
    public void PlayerWin(int amountWin,string? combination,List<Card>mainCards)
    {
        this.Coin+=amountWin;
        Console.WriteLine($"\nLe joueur {this.Name} a gagné avec {amountWin}");
        if(combination!=null)
        {
            Console.WriteLine($"et avec la combinaison {combination}");
            Card.ShowCards(mainCards);
            Card.ShowCards(this.Deck);
        }  
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
    /*
    *méthode si le joueur suit*
    *On prend en compte le fait qu'il faut soustraire ce qu'il doit miser sinon il va miser en trop*
    */
    public int FollowBet(int minBet)
    {
        int amountToCall=minBet-this.BetOfTheRound;
        if(amountToCall>=this.Coin)
            this.PlayerAllIn();
        else
        {
            Console.WriteLine($"\nLe joueur {this.Name} suit");
            Bet(amountToCall);
        }
        return amountToCall;
    }
    public static void EveryPlayerInGameShowCard(List<Player> players)
    {
        foreach(Player player in players)
        {
            player.UserCheckCard();
        }
    }
    /*
    *Si le joueur décide de suivre en n'augmentant pas la mise*
    *Il faut aussi prendre en compte que le minimum à miser peut-être supérieur à tout ce que le joueur est capable de miser si jamais un autre joueur a beaucoup plus de jetons*
    */
    public static bool IsPlayerFollowingWithNoMoreBet(Player player,int minBet)
    {
        if(player.BetOfTheRound==minBet||player.AllIn==true)
        {
            return true;
        }
        return false;
    }
    
}

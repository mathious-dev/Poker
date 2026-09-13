using Microsoft.VisualBasic;
using Poker.Models;
namespace Poker;

public class Rule
{
    public enum RuleEnum
    {
        HighCard,
        Pair,
        DoublePair,
        ThreeSameKind,
        Follow,
        Color,
        Full,
        Square,
        FollowFlush,
        RoyalFlush
    }
    public RuleEnum combination{get;set;}
    public static (List<Player>,Player) RuleHightCard(List<Player> players)
    {
        var winners=new List<Player>();
        int higherCard=2;
        var winner=new Player();
        foreach(var player in players)
        {
            foreach(Card card in player.Deck)
            {
                if(card.Number>higherCard)
                {
                    higherCard=card.Number;   
                    winner=player;      
                    winners.Clear();
                }
                else if(card.Number==higherCard&& player!=winner)
                {
                    winners.Add(player);
                }
            }

        }
        return (winners,winner);
    }
    public static (List<Player>,Player) RulePair(List<Player>players,List<Card> cardPlace)
    {
        var winners=new List<Player>();
        var winner=new Player();
        int highestPair=0;
        foreach(var player in players)
        {
            var allCards=GroupCards(player.Deck,cardPlace);
            
            var Pair=SearchPairOrThreeSame(allCards,2);
            if(Pair.Any())
            {
                if(Pair.First()>highestPair)
                {
                    highestPair=Pair.First();
                    winner=player;
                    winners.Clear();
                }
                else if(Pair.First()==highestPair)
                {
                    winners.Add(player);
                }
            }     
        }
        return (winners,winner);
    }
    public static (List<Player>,Player) RuleThreeSameKind(List<Player>players,List<Card> cardPlace)
    {
        var winners=new List<Player>();
        var winner=new Player();
        int highestThree=0;
        foreach(var player in players)
        {
            var allCards=GroupCards(player.Deck,cardPlace);
            var Three=SearchPairOrThreeSame(allCards,3);
            if(Three.Any())
            {
                if(Three.First()>highestThree)
                {
                    highestThree=Three.First();
                    winner=player;
                    winners.Clear();
                }
                else if(Three.First()==highestThree)
                {
                    winners.Add(player);
                }
            }
        }
        return (winners,winner);
    }
    public static List<int> SearchPairOrThreeSame(List<Card> allCards,int whereNumber)
    {
        var Pair=allCards.GroupBy(c=>c.Number)
                        .Where(g=>g.Count()>=whereNumber)
                        .Select(g => g.Key)
                        .OrderByDescending(n=>n)
                        .ToList();
        return Pair;
    }
    public static List<Card> GroupCards(Card[] cardsPlayer,List<Card> cardPlace)
    {
        var allCards=new List<Card>();
        allCards.AddRange(cardsPlayer);
        allCards.AddRange(cardPlace);
        return allCards;
    }
}

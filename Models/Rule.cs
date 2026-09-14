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
        (var winners,var winner)=GroupRuleSameKind(players,cardPlace,2);
        return (winners,winner);
    }
    public static (List<Player>,Player) RuleThreeSameKind(List<Player>players,List<Card> cardPlace)
    {
        (var winners,var winner)=GroupRuleSameKind(players,cardPlace,3);
        return (winners,winner);
    }
    public static (List<Player>,Player) RuleSquare(List<Player>players,List<Card> cardPlace)
    {
        (var winners,var winner)=GroupRuleSameKind(players,cardPlace,4);
        return (winners,winner);
    }
    public static (List<Player>,Player) GroupRuleSameKind(List<Player>players,List<Card> cardPlace,int sameKindNumber)
    {
        var winners=new List<Player>();
        var winner=new Player();
        int highestValue=0;
        foreach(var player in players)
        {
            var allCards=GroupCards(player.Deck,cardPlace);
            var listSameKind=SearchPairOrThreeSameOrFourSame(allCards,sameKindNumber);
            if(listSameKind.Any())
            {
                if(listSameKind.First()>highestValue)
                {
                    highestValue=listSameKind.First();
                    winner=player;
                    winners.Clear();
                }
                else if(listSameKind.First()==highestValue)
                {
                    winners.Add(player);
                }
            }
        }
         return (winners,winner);
    }
    public static List<int> SearchPairOrThreeSameOrFourSame(List<Card> allCards,int whereNumber)
    {
        var SameKind=allCards.GroupBy(c=>c.Number)
                        .Where(g=>g.Count()>=whereNumber)
                        .Select(g => g.Key)
                        .OrderByDescending(n=>n)
                        .ToList();
        return SameKind;
    }
    public static List<Card> GroupCards(Card[] cardsPlayer,List<Card> cardPlace)
    {
        var allCards=new List<Card>();
        allCards.AddRange(cardsPlayer);
        allCards.AddRange(cardPlace);
        return allCards;
    }
}

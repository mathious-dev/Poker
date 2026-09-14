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
    //utiliser LINQ avec max
    public static (List<Player>,Player) RuleHightCard(List<Player> players)
    {
        var winners=new List<Player>();
        int higherCard=0;
        var winner=new Player();
        foreach(var player in players)
        {
            var higherCardPlayer=player.Deck.Max(c=>c.Number);
            if(higherCardPlayer>higherCard)
            {
                higherCard=higherCardPlayer;   
                winner=player;      
                winners.Clear();
            }
            else if(higherCardPlayer==higherCard&& player!=winner)
            {
                winners.Add(player);
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
    public static (List<Player>,Player) RuleDoublePair(List<Player>players,List<Card> cardPlace,int sameKindNumber)
    {
        //Modifier pour le cas d'une troisième paire
        var winners=new List<Player>();
        var winner=new Player();
        int highestValueFirstPair=0;
        int highestValueSecondPair=0;
        foreach(var player in players)
        {
            var allCards=GroupCards(player.Deck,cardPlace);
            var listSameKind=SearchPairOrThreeSameOrFourSame(allCards,sameKindNumber);
            if(listSameKind.Any())
            {
                if(listSameKind.Count()==2)
                {
                    if(listSameKind.First()>highestValueFirstPair)
                    {
                        highestValueFirstPair=listSameKind.First();
                        highestValueSecondPair= listSameKind.Last();
                        winner=player;
                        winners.Clear();
                    }
                    else if(listSameKind.First()==highestValueFirstPair)
                    {
                        if(listSameKind.Last()>highestValueSecondPair)
                        {
                            highestValueSecondPair=listSameKind.Last();
                            winner=player;
                            winners.Clear();
                        }
                        else if(listSameKind.Last()==highestValueSecondPair)
                        {
                            winners.Add(player);
                        }
                    }      
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

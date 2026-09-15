using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
using Poker.Models;
namespace Poker;

public class Rule
{
    public enum RuleEnum
    {
        HighCard,//fait
        Pair,//fait
        DoublePair,//fait
        ThreeSameKind,//fait
        Follow,
        Color,//fait
        Full,
        Square,//fait
        FollowFlush,
        RoyalFlush
    }
    public RuleEnum combination{get;set;}
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
            ConditionHighestValueAndWinners(listSameKind,ref highestValue,winners,ref winner,player);
        }
         return (winners,winner);
    }
    public static void ConditionHighestValueAndWinners(List<int> listSameKind,ref int highestValue,List<Player>winners,ref Player winner,Player player)
    {
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
    public static (List<Player>,Player) RuleSameCardColor(List<Player>players,List<Card> cardPlace)
    {
        var winners=new List<Player>();
        var winner=new Player();
        int highestValue=0;
        foreach(var player in players)
        {
            var allCards=GroupCards(player.Deck,cardPlace);
            var listSameColor=GroupCardByColor(allCards);
            ConditionHighestValueAndWinners(listSameColor,ref highestValue,winners,ref winner,player);
        }
         return (winners,winner);
    }
    //ne pas oublier règle de l'As
    public static (List<Player>,Player) RuleFollow(List<Player>players,List<Card> cardPlace)
    {
        var winners=new List<Player>();
        var winner=new Player();
        int highestValue=0;
        foreach(var player in players)
        {
            var allCards=GroupCards(player.Deck,cardPlace);
            var listFollow=GroupCardByFollow(allCards);
            ConditionHighestValueAndWinners(listFollow,ref highestValue,winners,ref winner,player);
        }
         return (winners,winner);
    }
    public static List<int> GroupCardByColor(List<Card> cards)
    {
        var listCardSameColor=cards.GroupBy(g=>g.Type)
                                .Where(c=>c.Count()==5)
                                .SelectMany(c=>c)//va ouvrir la boite "coeur" par exemple avec toutes les cartes
                                .Select(c=>c.Number)
                                .OrderByDescending(c=>c)
                                .Take(5)
                                .ToList();
        return listCardSameColor;
    }
    public static List<int> GroupCardByFollow(List<Card> cards)
    {
        //faire la vérification de l'As qui peut être avec 2 ou un roi
        var listCardFollow=cards.Select(c=>c.Number)
                                .Distinct()
                                .OrderByDescending(c=>c)
                                .ToList();
        var count=1;
        var countSinceStart=2;//on le commence à 2 car si count est à 5 il y a  un break, on ne va donc jamais atteindre le countSinceStart++ et la soustraction de skip sera faussée
        var previousCard=0;
        var listEmpty=new List<int>();
        for(int i=0;i<listCardFollow.Count()-1;i++)//-1 car on va toujours comparer avec le chiffre d'après
        {
            previousCard=listCardFollow[i];
            if(listCardFollow[i+1]==previousCard-1)
            {
                count++;
                
                if(count==5)
                {
                    var skip=countSinceStart-count;
                    listCardFollow=listCardFollow.Skip(skip)//il faut modifier la liste
                                .Take(5)
                                .ToList();
                    break;
                }
                    
            }
            else
            {
                count=1;
            }
            countSinceStart++;
        }
        if(count==5)
            return listCardFollow;
        else
            return listEmpty;
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

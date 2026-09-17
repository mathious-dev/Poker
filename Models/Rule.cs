using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
using Poker.Models;
namespace Poker;

public class Rule
{
    public enum RuleEnum
    {
        HighCard=1,//fait
        Pair,//fait
        DoublePair,//fait
        ThreeSameKind,//fait
        Follow,//fait
        Color,//fait
        Full,//fait
        Square,//fait
        FollowFlush,//fait
        RoyalFlush//fait
    }
    public static (List<Player>,Player,string) RuleHightCard(List<Player> players)
    {
        int numCombination=1;
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
        return (winners,winner,((Rule.RuleEnum)numCombination).ToString());
    }
    public static (List<Player>,Player,string) RulePair(List<Player>players,List<Card> cardPlace)
    {
        int numCombination=2;
        (var winners,var winner)=GroupRuleSameNumber(players,cardPlace,2);
        return (winners,winner,((Rule.RuleEnum)numCombination).ToString());
    }
    public static (List<Player>,Player,string) RuleDoublePair(List<Player>players,List<Card> cardPlace)
    {
        //Modifier pour le cas d'une troisième paire
        int numCombination=3;
        var winners=new List<Player>();
        var winner=new Player();
        int highestValueFirstPair=0;
        int highestValueSecondPair=0;
        foreach(var player in players)
        {
            var allCards=GroupCards(player.Deck,cardPlace);
            var listSameKind=SearchPairOrThreeSameOrFourSame(allCards,2);
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
        return (winners,winner,((Rule.RuleEnum)numCombination).ToString());
    }
    public static (List<Player>,Player,string) RuleThreeSameKind(List<Player>players,List<Card> cardPlace)
    {
        int numCombination=4;
        (var winners,var winner)=GroupRuleSameNumber(players,cardPlace,3);
        return (winners,winner,((Rule.RuleEnum)numCombination).ToString());
    }
    public static (List<Player>,Player,string) RuleSquare(List<Player>players,List<Card> cardPlace)
    {
        int numCombination=8;
        (var winners,var winner)=GroupRuleSameNumber(players,cardPlace,4);
        return (winners,winner,((Rule.RuleEnum)numCombination).ToString());
    }
    public static (List<Player>,Player,string) RuleSameCardColor(List<Player>players,List<Card> cardPlace)
    {
        int numCombination=6;
        var winners=new List<Player>();
        var winner=new Player();
        int highestValue=0;
        foreach(var player in players)
        {
            var allCards=GroupCards(player.Deck,cardPlace);
            var listSameColor=GroupCardByColor(allCards);
            if(listSameColor.Any())
            {
                listSameColor=listSameColor.Take(5).ToList();
                ConditionHighestValueAndWinners(listSameColor,ref highestValue,winners,ref winner,player);
            }
        }
        return (winners,winner,((Rule.RuleEnum)numCombination).ToString());
    }
    public static (List<Player>,Player,string) RuleFollow(List<Player>players,List<Card> cardPlace)
    {
        int numCombination=5;
        var winners=new List<Player>();
        var winner=new Player();
        int highestValue=0;
        foreach(var player in players)
        {
            var allCards=GroupCards(player.Deck,cardPlace);
            var listFollow=GroupCardByFollow(allCards);
            ConditionHighestValueAndWinners(listFollow,ref highestValue,winners,ref winner,player);
        }
        return (winners,winner,((Rule.RuleEnum)numCombination).ToString());
    }
    public static (List<Player>,Player,string) RuleFull(List<Player>players,List<Card> cardPlace)
    {
        int numCombination=7;
        var winners=new List<Player>();
        var winner=new Player();
        int highestValueTriple=0;
        int highestValuePair=0;
        foreach(var player in players)
        {
            var allCards=GroupCards(player.Deck,cardPlace);
            var listFullPair=SearchPairOrThreeSameOrFourSame(allCards,2);
            var listFullTriple=SearchPairOrThreeSameOrFourSame(allCards,3);
            if(listFullPair.Any()&&listFullTriple.Any())
            {
                if(listFullTriple.First()>highestValueTriple)
                {
                    highestValueTriple=listFullTriple.First();
                    highestValuePair=listFullPair.First();
                    winner=player;
                    winners.Clear();
                }
                else if(listFullTriple.First()==highestValueTriple)
                {
                    if(listFullPair.First()>highestValuePair)
                    {
                        highestValuePair=listFullPair.First();
                        winner=player;
                        winners.Clear();
                    }
                    else if(listFullPair.First()==highestValuePair)
                        winners.Add(player);
                }
            }
                
        }
        return (winners,winner,((Rule.RuleEnum)numCombination).ToString());
    }
    public static (List<Player>,Player,string) RuleFollowFlush(List<Player>players,List<Card> cardPlace)
    {
        int numCombination=9;
        var winners=new List<Player>();
        var winner=new Player();
        int highestValue=0;
        foreach(var player in players)
        {
            var allCards=GroupCards(player.Deck,cardPlace);
            var listFollowFlush=GroupCardByFollowFlush(allCards);
            ConditionHighestValueAndWinners(listFollowFlush,ref highestValue,winners,ref winner,player);
        }
        return (winners,winner,((Rule.RuleEnum)numCombination).ToString());
    }
    public static (List<Player>,Player,string) RuleFollowRoyalFlush(List<Player>players,List<Card> cardPlace)
    {
        int numCombination=10;
        var winners=new List<Player>();
        var winner=new Player();
        int highestValue=0;
        foreach(var player in players)
        {
            var allCards=GroupCards(player.Deck,cardPlace);
            var listFollowRoyalFlush=GroupCardByFollowRoyalFlush(allCards);
            ConditionHighestValueAndWinners(listFollowRoyalFlush,ref highestValue,winners,ref winner,player);
        }
        return (winners,winner,((Rule.RuleEnum)numCombination).ToString());
    }

    public static (List<Player>,Player) GroupRuleSameNumber(List<Player>players,List<Card> cardPlace,int sameKindNumber)
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
    public static List<int> GroupCardByColor(List<Card> cards)
    {
        //on va prendre toutes les cartes afin de pouvoir utiliser la méthode pour la suite flush
        var listCardSameColor=cards.GroupBy(g=>g.Type)
                                .Where(c=>c.Count()>=5)
                                .SelectMany(c=>c)//va ouvrir la boite "coeur" par exemple avec toutes les cartes
                                .Select(c=>c.Number)
                                .OrderByDescending(c=>c)
                                .ToList();
        return listCardSameColor;
    }
    public static List<int> GroupCardByFollow(List<Card> cards)
    {

        var listCardFollow=cards.Select(c=>c.Number)
                                .Distinct()
                                .OrderByDescending(c=>c)
                                .ToList();
        listCardFollow=IsFollow(listCardFollow);
        return listCardFollow;
    }
    public static List<int> GroupCardByFollowFlush(List<Card> cards)
    {

        var listCardFollowFlush=GroupCardByColor(cards);
        if(listCardFollowFlush.Any())
            listCardFollowFlush=IsFollow(listCardFollowFlush);
        return listCardFollowFlush;
    }
    public static List<int> GroupCardByFollowRoyalFlush(List<Card> cards)
    {
        var listRoyal=GroupCardByFollowFlush(cards);
        if(listRoyal.Any()&&listRoyal.First()==14)
            return listRoyal;
        return new List<int>();
    }
    public static List<Card> GroupCards(Card[] cardsPlayer,List<Card> cardPlace)
    {
        var allCards=new List<Card>();
        allCards.AddRange(cardsPlayer);
        allCards.AddRange(cardPlace);
        return allCards;
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
    public static List<int> IsFollow(List<int> listCardFollow)
    {
        var count=1;
        var countSinceStart=2;//on le commence à 2 car si count est à 5 il y a  un break, on ne va donc jamais atteindre le countSinceStart++ et la soustraction de skip sera faussée
        var previousCard=0;
        var listEmpty=new List<int>();
        if(listCardFollow.Contains(14))
            listCardFollow.Add(1);
            
        
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
                count=1;
            countSinceStart++;
        }
        if(count==5)
            return listCardFollow;
        else
            return listEmpty;
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
    
}

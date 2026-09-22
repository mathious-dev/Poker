using System.Globalization;
using System.Security.Cryptography.X509Certificates;
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
        var othersWinners=new List<Player>();
        int higherCard=0;
        var winner=new Player();
        foreach(var player in players)
        {
            var higherCardPlayer=player.Deck.Max(c=>c.Number);
            if(higherCardPlayer>higherCard)
            {
                higherCard=higherCardPlayer;   
                winner=player;      
                othersWinners.Clear();
            }
            else if(higherCardPlayer==higherCard&& player!=winner)
            {
                othersWinners.Add(player);
            }
        }
        return (othersWinners,winner,((Rule.RuleEnum)numCombination).ToString());
    }
    public static (List<Player>,Player,string) RulePair(List<Player>players,List<Card> cardPlace)
    {
        int numCombination=2;
        (var othersWinners,var winner,var highestValue)=GroupRuleSameNumber(players,cardPlace,2);
        if(othersWinners.Any())
            CompareLastCards(othersWinners,cardPlace,highestValue,ref winner);
        return (othersWinners,winner,((Rule.RuleEnum)numCombination).ToString());
    }
    public static (List<Player>,Player,string) RuleDoublePair(List<Player>players,List<Card> cardPlace)
    {
        int numCombination=3;
        var othersWinners=new List<Player>();
        var winner=new Player();
        int highestValueFirstPair=0;
        int highestValueSecondPair=0;
        int highestValueLastCardForWinner=0;
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
                        othersWinners.Clear();
                    }
                    else if(listSameKind.First()==highestValueFirstPair)
                    {
                        if(listSameKind.Last()>highestValueSecondPair)
                        {
                            highestValueSecondPair=listSameKind.Last();
                            winner=player;
                            othersWinners.Clear();
                        }
                        else if(listSameKind.Last()==highestValueSecondPair)
                        {
                            othersWinners.Add(player);
                        }
                    }      
                }     
            }
        }
        if(othersWinners.Any())
        {
            var allWinners=new List<Player>();
            allWinners.Add(winner);
            allWinners.AddRange(othersWinners);
            othersWinners.Clear();
            foreach(Player player in allWinners)
            {
                var allCardsForWinners=GroupCards(player.Deck,cardPlace);
                var cardWithoutSameNumber=allCardsForWinners.Select(c=>c.Number)
                        .Where(c=>c!=highestValueFirstPair&&c!=highestValueSecondPair)
                        .OrderByDescending(n => n)  
                        .SkipLast(2)
                        .First();
                if(highestValueLastCardForWinner==0)
                    highestValueLastCardForWinner=cardWithoutSameNumber;
                if(cardWithoutSameNumber>highestValueLastCardForWinner)
                {
                    highestValueLastCardForWinner=cardWithoutSameNumber;
                    othersWinners.Clear();
                    winner=player;
                }
                else if(cardWithoutSameNumber==highestValueLastCardForWinner)
                {
                    othersWinners.Add(player);
                }
                    
            }
        }
        return (othersWinners,winner,((Rule.RuleEnum)numCombination).ToString());
    }
    public static (List<Player>,Player,string) RuleThreeSameKind(List<Player>players,List<Card> cardPlace)
    {
        int numCombination=4;
        (var othersWinners,var winner,var highestValue)=GroupRuleSameNumber(players,cardPlace,3);
        if(othersWinners.Any())
            CompareLastCards(othersWinners,cardPlace,highestValue,ref winner);
        return (othersWinners,winner,((Rule.RuleEnum)numCombination).ToString());
    }
    public static (List<Player>,Player,string) RuleSquare(List<Player>players,List<Card> cardPlace)
    {
        int numCombination=8;
        (var othersWinners,var winner,var highestValue)=GroupRuleSameNumber(players,cardPlace,4);
        if(othersWinners.Any())
            CompareLastCards(othersWinners,cardPlace,highestValue,ref winner);
        return (othersWinners,winner,((Rule.RuleEnum)numCombination).ToString());
    }
    public static (List<Player>,Player,string) RuleSameCardColor(List<Player>players,List<Card> cardPlace)
    {
        int numCombination=6;
        var othersWinners=new List<Player>();
        var winner=new Player();
        int highestValue=0;
        foreach(var player in players)
        {
            var allCards=GroupCards(player.Deck,cardPlace);
            var listSameColor=GroupCardByColor(allCards);
            if(listSameColor.Any())
            {
                listSameColor=listSameColor.Take(5).ToList();
                ConditionHighestValueAndothersWinners(listSameColor,ref highestValue,othersWinners,ref winner,player);
            }
        }
        return (othersWinners,winner,((Rule.RuleEnum)numCombination).ToString());
    }
    public static (List<Player>,Player,string) RuleFollow(List<Player>players,List<Card> cardPlace)
    {
        int numCombination=5;
        var othersWinners=new List<Player>();
        var winner=new Player();
        int highestValue=0;
        foreach(var player in players)
        {
            var allCards=GroupCards(player.Deck,cardPlace);
            var listFollow=GroupCardByFollow(allCards);
            ConditionHighestValueAndothersWinners(listFollow,ref highestValue,othersWinners,ref winner,player);
        }
        return (othersWinners,winner,((Rule.RuleEnum)numCombination).ToString());
    }
    public static (List<Player>,Player,string) RuleFull(List<Player>players,List<Card> cardPlace)
    {
        int numCombination=7;
        var othersWinners=new List<Player>();
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
                    othersWinners.Clear();
                }
                else if(listFullTriple.First()==highestValueTriple)
                {
                    if(listFullPair.First()>highestValuePair)
                    {
                        highestValuePair=listFullPair.First();
                        winner=player;
                        othersWinners.Clear();
                    }
                    else if(listFullPair.First()==highestValuePair)
                        othersWinners.Add(player);
                }
            }
                
        }
        return (othersWinners,winner,((Rule.RuleEnum)numCombination).ToString());
    }
    public static (List<Player>,Player,string) RuleFollowFlush(List<Player>players,List<Card> cardPlace)
    {
        int numCombination=9;
        var othersWinners=new List<Player>();
        var winner=new Player();
        int highestValue=0;
        foreach(var player in players)
        {
            var allCards=GroupCards(player.Deck,cardPlace);
            var listFollowFlush=GroupCardByFollowFlush(allCards);
            ConditionHighestValueAndothersWinners(listFollowFlush,ref highestValue,othersWinners,ref winner,player);
        }
        return (othersWinners,winner,((Rule.RuleEnum)numCombination).ToString());
    }
    public static (List<Player>,Player,string) RuleFollowRoyalFlush(List<Player>players,List<Card> cardPlace)
    {
        int numCombination=10;
        var othersWinners=new List<Player>();
        var winner=new Player();
        int highestValue=0;
        foreach(var player in players)
        {
            var allCards=GroupCards(player.Deck,cardPlace);
            var listFollowRoyalFlush=GroupCardByFollowRoyalFlush(allCards);
            ConditionHighestValueAndothersWinners(listFollowRoyalFlush,ref highestValue,othersWinners,ref winner,player);
        }
        return (othersWinners,winner,((Rule.RuleEnum)numCombination).ToString());
    }

    public static (List<Player>,Player,int) GroupRuleSameNumber(List<Player>players,List<Card> cardPlace,int sameKindNumber)
    {
        var othersWinners=new List<Player>();
        var winner=new Player();
        int highestValue=0;
        foreach(var player in players)
        {
            var allCards=GroupCards(player.Deck,cardPlace);
            var listSameKind=SearchPairOrThreeSameOrFourSame(allCards,sameKindNumber);
            ConditionHighestValueAndothersWinners(listSameKind,ref highestValue,othersWinners,ref winner,player);
        }
         return (othersWinners,winner,highestValue);
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
    public static void ConditionHighestValueAndothersWinners(List<int> listSameKind,ref int highestValue,List<Player>othersWinners,ref Player winner,Player player)
    {
        if(listSameKind.Any())
            {
                if(listSameKind.First()>highestValue)
                {
                    highestValue=listSameKind.First();
                    winner=player;
                    othersWinners.Clear();
                }
                else if(listSameKind.First()==highestValue)
                {
                    othersWinners.Add(player);
                }
            }
    }
    public static void CompareLastCards(List<Player>othersWinners,List<Card>cardPlace,int highestValue,ref Player winner)
    {
        var allWinners= new List<Player>();
        List<int> bestLastCards = null;//stocker les meilleurs cartes d'un joueur à battre
        allWinners.AddRange(othersWinners.ToList());
        allWinners.Add(winner);
        othersWinners.Clear();
        foreach(Player player in allWinners)
        {
            var allCards=GroupCards(player.Deck,cardPlace);
            var allCardsWithoutSameNumber=allCards.Select(c=>c.Number)
                    .Where(c=>c!=highestValue)
                    .OrderByDescending(n => n)  
                    .SkipLast(2)
                    .ToList();
            if(bestLastCards==null)
            {
                bestLastCards=allCardsWithoutSameNumber;
                winner=player;
            }
            else
            {
                bool isBetter = false;
                bool isEqual = true;
                for(int i=0;i<allCardsWithoutSameNumber.Count();i++)
                {
                    if(allCardsWithoutSameNumber[i]>bestLastCards[i])
                    {
                        winner=player;
                        bestLastCards=allCardsWithoutSameNumber;
                        othersWinners.Clear();
                        isBetter = true;
                        isEqual = false;
                        break;
                    }
                    else if(allCardsWithoutSameNumber[i]<bestLastCards[i])
                    {
                        isEqual=false;
                        break;
                    }
                }
                if(isEqual)
                    othersWinners.Add(player); 
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

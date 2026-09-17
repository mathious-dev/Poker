namespace Poker.Models;

public class GameEngine
{
    List<Card>listCardsStart=Card.GeneralDeckCard();
    int countRound=1;
    public void Init(List<Bot>bots, List<Player> players)
    {
        Random randomLevel=new Random();
        Random randomCard=new Random();
        listCardsStart=listCardsStart.OrderBy(c=>randomCard.Next()).ToList();
        (var FirstCard,var SecondCard)=GiveCards(listCardsStart);
        Bot bot1=new Bot(randomLevel.Next(1,4))
        {
            Name="bot1",
            Deck = [
                FirstCard,
                SecondCard
            ]
        };
        (FirstCard,SecondCard)=GiveCards(listCardsStart);
        Bot bot2=new Bot(randomLevel.Next(1,4))
        {
            Name="bot2",
            Deck = [
                FirstCard,
                SecondCard
            ]
        };
        (FirstCard,SecondCard)=GiveCards(listCardsStart);
        Bot bot3=new Bot(randomLevel.Next(1,4))
        {
            Name="bot3",
            Deck = [
                FirstCard,
                SecondCard
            ]
        };
        
        foreach(Player player in players)
        {
            (FirstCard,SecondCard)=GiveCards(listCardsStart);
            player.Deck = [
                FirstCard,
                SecondCard
            ];
        }
        bots.Add(bot1);
        bots.Add(bot2);
        bots.Add(bot3);
        Console.WriteLine("\nCommencement de la partie.");
        foreach(Bot bot in bots)
        {
            Console.WriteLine($"\nLe bot {bot.Name}a rejoint la partie");
        }
    }
    public void Round(List<Bot>bots,List<Player> players,int minBet,int mainPot)
    {
        
        foreach(Bot bot in bots)
        {
            bot.BotAction(bot.Level,);//appliquer avant au bot les combinaisons pour qu'il sache quoi faire
        }
    }
    public (List<Player>?,Player,string) WhoWin(List<Bot>bots,List<Player> players,List<Card> mainCards)
    {
        string combination;
        var allPlayers=new List<Player>();
        allPlayers.AddRange(bots);
        allPlayers.AddRange(players);
        var winner = new Player();
        var winners=new List<Player>();
        (winners,winner,combination)=Rule.RuleFollowRoyalFlush(allPlayers,mainCards);
        if(winner.Name!=null)
            return (winners,winner,combination);

        (winners,winner,combination)=Rule.RuleFollowFlush(allPlayers,mainCards);
        if(winner.Name!=null)
            return (winners,winner,combination);

        (winners,winner,combination)=Rule.RuleSquare(allPlayers,mainCards);
        if(winner.Name!=null)
            return (winners,winner,combination);

        (winners,winner,combination)=Rule.RuleFull(allPlayers,mainCards);
        if(winner.Name!=null)
            return (winners,winner,combination);

        (winners,winner,combination)=Rule.RuleSameCardColor(allPlayers,mainCards);
        if(winner.Name!=null)
            return (winners,winner,combination);

        (winners,winner,combination)=Rule.RuleFollow(allPlayers,mainCards);
        if(winner.Name!=null)
            return (winners,winner,combination);

        (winners,winner,combination)=Rule.RuleThreeSameKind(allPlayers,mainCards);
        if(winner.Name!=null)
            return (winners,winner,combination);

        (winners,winner,combination)=Rule.RuleDoublePair(allPlayers,mainCards);
        if(winner.Name!=null)
            return (winners,winner,combination);

        (winners,winner,combination)=Rule.RulePair(allPlayers,mainCards);
        if(winner.Name!=null)
            return (winners,winner,combination);

        (winners,winner,combination)=Rule.RuleHightCard(allPlayers);
        return (winners,winner,combination);
    }
    public (Card,Card) GiveCards(List<Card> listCards)
    {
        var FirstCard=listCards[0];
        listCards.RemoveAt(0);
        var SecondCard=listCards[0];
        listCards.RemoveAt(0);
        return (FirstCard,SecondCard);
    }
}

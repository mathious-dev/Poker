namespace Poker.Models;

public class GameEngine
{
    List<Card>listCardsStart=Card.GeneralDeckCard();
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
    public (Card,Card) GiveCards(List<Card> listCards)
    {
        var FirstCard=listCards[0];
        listCards.RemoveAt(0);
        var SecondCard=listCards[0];
        listCards.RemoveAt(0);
        return (FirstCard,SecondCard);
    }
}

using System.Diagnostics;

namespace Poker.Models;

public class GameEngine
{
    
    public void Init(List<Bot>bots)
    {
        Random randomLevel=new Random();
        Bot bot1=new Bot(randomLevel.Next(1,4))
        {Name="bot1",};
        Bot bot2=new Bot(randomLevel.Next(1,4))
        {Name="bot2",  };
        Bot bot3=new Bot(randomLevel.Next(1,4))
        { Name="bot3",};
        bots.Add(bot1);
        bots.Add(bot2);
        bots.Add(bot3);
        Console.WriteLine("\nCommencement de la partie.");
        foreach(Bot bot in bots)
        {
            Console.WriteLine($"\nLe bot {bot.Name}a rejoint la partie");
        }
    }
    public void Round(List<Bot>bots,Player humanPlayer,int minBet,List<Card>?cardsOnTable)
    {
        Random randomCard=new Random();
        var listCardsStart=new List<Card>();
        int handTour=1;
        int mainPot=0;
        var allPlayers=new List<Player>();
        allPlayers.Add(humanPlayer);
        allPlayers.AddRange(bots);
        listCardsStart=Card.GeneralDeckCard();
        listCardsStart=listCardsStart.OrderBy(c=>randomCard.Next()).ToList();
        foreach(Player player in allPlayers)
        {
            var (FirstCard,SecondCard)=GiveCards(listCardsStart);
            player.Deck=[FirstCard,SecondCard];
        }
        while(handTour<5 ||allPlayers.Count()>1)
        {
            foreach(Player player in allPlayers)
            {
                if(player is Bot bot)
                {
                    int betFromBot=bot.BotAction(ref mainPot,ref minBet,cardsOnTable);
                    if(betFromBot<minBet||betFromBot==0)
                        bot.PlayerSleep(allPlayers);   
                }
                
            }
            Console.Write("\n Que voulez-vous faire?");
            ChoiceUser(ref minBet,ref mainPot,allPlayers,humanPlayer);
        }
        
    }
    public void ChoiceUser(ref int minBet,ref int mainPot,List<Player>allPlayersInGame,Player humanPlayer)
    {
        int choice=0;
        string[] tab={"Miser","Se coucher","Regarder vos cartes","Consulter votre pot","Arrêter le jeu"};
        while(choice!=1&&choice!=2&&choice!=4)
        {
            foreach(string sentenceChoice in tab)
            {
                int i=1;
                Console.WriteLine($"\n{i}.{sentenceChoice}");
                i++;
            }
            choice=Gestion.IntEnter();
            switch(choice)
            {
                case 1:UserBet(ref minBet,ref mainPot,humanPlayer);break;
                case 2:humanPlayer.PlayerSleep(allPlayersInGame);break;
                case 3:UserCheckCard();break;
                case 4:Console.WriteLine($"\n{humanPlayer.Coin}");break;
                case 5:Console.WriteLine("\nFin du jeu");;break;
            }
        }
    }
    public void UserBet(ref int minBet,ref int mainPot,Player humanPlayer)
    {
        bool finish=false;
        int montant;
        int stop=0;
        Console.WriteLine("\nEntrez le montant a miser");
        while(!finish)
        {
            montant=Gestion.IntEnter();
            if(montant<minBet)
            {
                Console.WriteLine($"\nVous devez misez plus ou égal que {minBet} !");
                while(stop!=1&&stop!=2)
                {
                    Console.WriteLine("\nAnnuler?\n 1.OUI\n2.NON");
                    stop=Gestion.IntEnter();
                }
            }
            else
            {
                humanPlayer.Bet(ref mainPot,montant);
                Console.WriteLine($"\nVotre pot est de {humanPlayer.Coin}");
                finish=true;
            }
        }
    }
    public void UserCheckCard()
    {
        
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

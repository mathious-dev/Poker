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
    public void Round(List<Player>allPlayers,int minBet)
    {
        Random randomCard=new Random();
        var listCards=new List<Card>();
        var listCardsOnTable=new List<Card>();
        int handTour=1;
        int mainPot=0;
        var humanPlayer=new Player();
        listCards=Card.GeneralDeckCard();
        listCards=listCards.OrderBy(c=>randomCard.Next()).ToList();
        foreach(Player player in allPlayers)
        {
            var (FirstCard,SecondCard)=GiveCardsStart(listCards);
            player.Deck=[FirstCard,SecondCard];
            if(player is not Bot)
            {
                humanPlayer=player;
            }
        }
        while(handTour<5 ||allPlayers.Count()>1)
        {
            Console.WriteLine($"\nTour {handTour}");
            if(handTour==2)
            {
                for(int i=0;i<3;i++)
                {
                    var card=GiveCard(listCards);
                    listCardsOnTable.Add(card);
                }
            }
            else if(handTour>2)
            {
                var card=GiveCard(listCards);
                listCardsOnTable.Add(card);
            }
            foreach(Player player in allPlayers)
            {
                if(player is Bot bot)
                {
                    int betFromBot=bot.BotAction(ref mainPot,ref minBet,null);
                    if(betFromBot<minBet||betFromBot==0)
                        bot.PlayerSleep(allPlayers);   
                }
                
            }
            Console.Write("\n Que voulez-vous faire?");
            ChoiceUser(ref minBet,ref mainPot,allPlayers,humanPlayer);
            handTour++;
        }
        
    }
    public void ChoiceUser(ref int minBet,ref int mainPot,List<Player>allPlayersInGame,Player humanPlayer)
    {
        bool playerHasBetOrFinish=false;
        int choice=0;
        string[] tab={"Miser","Se coucher","Consulter vos informations","Voir les informations principales"};
        while(!playerHasBetOrFinish)
        {
            int i=1;
            foreach(string sentenceChoice in tab)
            {
                Console.WriteLine($"\n{i}.{sentenceChoice}");
                i++;
            }
            choice=Gestion.IntEnter();
            switch(choice)
            {
                case 1:UserBet(ref minBet,ref mainPot,humanPlayer,allPlayersInGame,ref playerHasBetOrFinish);break;
                case 2:humanPlayer.PlayerSleep(allPlayersInGame);playerHasBetOrFinish = true;break;
                case 3:humanPlayer.UserCheckCard();humanPlayer.CheckBet();break;
                case 4:
                foreach(Player player in allPlayersInGame)
                {
                    if(player.BetOfTheRound>0)
                        Console.WriteLine($"\n joueur {player.Name} a misé {player.BetOfTheRound}");
                    else
                        Console.WriteLine($"\n joueur {player.Name} est couché");
                }
                Console.WriteLine($"\nLe pot principal est de {mainPot} et la mise minimal est de {minBet}");
                break;
            }
        }
    }
    //possibilité de refactoriser
    public void UserBet(ref int minBet,ref int mainPot,Player humanPlayer,List<Player>allPlayersInGame,ref bool hasBetOrFinish)
    {
        bool finish=false;
        int montant;
        Console.WriteLine($"\nVous avez {humanPlayer.Coin} jetons");
        while(!finish)
        {
            int stop=0;
            int allInChoice=0;
            if(minBet>humanPlayer.Coin)
            {
                Console.WriteLine($"\nLe minimum a miser est de {minBet}, ce qui est supérieur à votre pot {humanPlayer.Coin} Voulez-vous all-in? \n1.OUI\n2.NON, vous vous couchez");
                while(allInChoice!=1&&allInChoice!=2)
                {
                    allInChoice=Gestion.IntEnter();
                    if(allInChoice==1)
                    {
                        humanPlayer.PlayerAllIn(ref mainPot);
                        hasBetOrFinish=true;
                        humanPlayer.CheckBet();
                        finish=true;
                    }
                        
                    else if(allInChoice==2)
                    {
                        humanPlayer.PlayerSleep(allPlayersInGame);
                        hasBetOrFinish=true;
                        finish = true;
                    }
                    else
                        Console.WriteLine("\nVous devez choisir une option !");
                }
                continue; // Force à repartir au début du while
            }
            Console.WriteLine("\nEntrez le montant a miser");
            montant=Gestion.IntEnter();
            if(montant<minBet)
            {
                Console.WriteLine($"\nVous devez misez plus ou égal que {minBet} !");
                while(stop!=1&&stop!=2)
                {
                    Console.WriteLine("\nAnnuler?\n 1.OUI\n2.NON");
                    stop=Gestion.IntEnter();
                    if(stop==1)
                    {
                        hasBetOrFinish=false;
                        Console.WriteLine("\nAnnulation...");
                        finish=true;
                    }
                    else if(stop != 1 && stop != 2)
                    {
                        Console.WriteLine("\nVous devez choisir une option !");
                    } 
                }
            }
            else
            {
                humanPlayer.Bet(ref mainPot,montant);
                hasBetOrFinish=true;
                Console.WriteLine($"\nVotre pot est de {humanPlayer.Coin}");
                Console.WriteLine($"\nVotre mise totale est de {humanPlayer.BetOfTheRound}");
                finish=true;
            }
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
    public (Card,Card) GiveCardsStart(List<Card> listCards)
    {
        var FirstCard=listCards[0];
        listCards.RemoveAt(0);
        var SecondCard=listCards[0];
        listCards.RemoveAt(0);
        return (FirstCard,SecondCard);
    }
    public Card GiveCard(List<Card> listCards)
    {
        var card=listCards[0];
        listCards.RemoveAt(0);
        return card;
    }
}

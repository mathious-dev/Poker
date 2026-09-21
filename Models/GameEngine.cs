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
    public void AddBetOnMainPot(ref int mainPot,int AmountBet)
    {
        mainPot+=AmountBet;
    }
    public void MinBetChange(ref int minBet,int AmountBet)
    {
        minBet=AmountBet;
    }
    public Player SetSettingStartWithCardsAndCheckHumanPlayer(List<Card>listCards,List<Player>players,Player human)
    {
        foreach(Player player in players)
        {
            var (FirstCard,SecondCard)=GiveCardsStart(listCards);
            player.Deck=[FirstCard,SecondCard];
            if(player is not Bot)
            {
                human=player;
            }
        }
        return human;
    }
    public void GiveCardOnTable(int handTour,List<Card> generalDeckCards,List<Card>cardsOnTable)
    {
        if(handTour==2)
            {
                for(int i=0;i<3;i++)
                {
                    var card=GiveCard(generalDeckCards);
                    cardsOnTable.Add(card);
                }
                Card.ShowCards(cardsOnTable);
            }
            else if(handTour>2&&handTour<5)
            {
                var card=GiveCard(generalDeckCards);
                cardsOnTable.Add(card);
                Card.ShowCards(cardsOnTable);
            }
    }
    public bool VerifNumberPlayer(List<Player> players,int countPlayer)
    {
        return (countPlayer >= players.Count() || players.Count() == 1);
    }
    public bool VerifPLayerAllIn(Player player)
    {
        return (player.AllIn);
    }
    public void ShowMainPot(int mainPot)
    {
        Console.WriteLine($"\nle pot est de : {mainPot} jetons");
    }
    //Corriger la logique des mises car à chaque tour on est obligé de miser le minimum
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
        humanPlayer=SetSettingStartWithCardsAndCheckHumanPlayer(listCards,allPlayers,humanPlayer);
        while(handTour<5 &&allPlayers.Count()>1)
        {
            int countPlayerPlayed=0;
            Console.WriteLine($"\nTour {handTour}");
            humanPlayer.UserCheckCard();
            ShowMainPot(mainPot);
            GiveCardOnTable(handTour,listCards,listCardsOnTable);
            if (allPlayers.Count(p => !p.AllIn) > 1)
            {
                while(allPlayers.Count()>countPlayerPlayed)
                {
                    Console.WriteLine($"\nLa mise minimal est de : {minBet}");
                    foreach(Player player in allPlayers.ToList())//on crée une copie de la liste pour éviter une erreur
                    {
                        if (VerifNumberPlayer(allPlayers,countPlayerPlayed))
                            break;
                        if(VerifPLayerAllIn(player))
                        {
                            countPlayerPlayed++;
                            continue;// On passe directement au joueur suivant
                        }
                        if(player is Bot bot)
                        {
                            int betFromBot=bot.BotAction(minBet,listCardsOnTable);
                            if(!bot.AllIn&&(betFromBot<minBet||betFromBot==0))
                                bot.PlayerSleep(allPlayers);   
                            else if(betFromBot>minBet)
                            {
                                MinBetChange(ref minBet,betFromBot);
                                countPlayerPlayed=1;
                            }
                            else
                                countPlayerPlayed++;
                            AddBetOnMainPot(ref mainPot,betFromBot);
                        }
                        else
                        {
                            Console.Write("\n Que voulez-vous faire?");
                            int amountBet=ChoiceUser(minBet,mainPot,allPlayers,humanPlayer);
                            if(!humanPlayer.AllIn&&(amountBet<minBet||amountBet==0))
                                humanPlayer.PlayerSleep(allPlayers);   
                            else if(amountBet>minBet)
                            {
                                MinBetChange(ref minBet,amountBet);
                                countPlayerPlayed=1;
                            }
                            else
                                countPlayerPlayed++;
                            AddBetOnMainPot(ref mainPot,amountBet);
                        }
                    }
                }
            }
            if(handTour==4||allPlayers.All(p=>p.AllIn))
                Player.EveryPlayerInGameShowCard(allPlayers);
            handTour++;
            ShowMainPot(mainPot);
        }
        if(allPlayers.Count()==1)
        {
            var winner= new Player();
            winner=allPlayers.First();
            winner.PlayerWin(mainPot,null,listCardsOnTable);
        }
        else
            WhoWin(allPlayers,listCardsOnTable,mainPot);
    }
    public int ChoiceUser(int minBet,int mainPot,List<Player>allPlayersInGame,Player humanPlayer)
    {
        bool playerHasBetOrFinish=false;
        int choice=0;
        int amountBetFromHuman=0;
        string[] tab={"Miser","Suivre","Se coucher","Consulter vos informations","Voir les informations principales"};
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
                case 1:amountBetFromHuman=UserBet(minBet,humanPlayer,allPlayersInGame,ref playerHasBetOrFinish);break;
                case 2:amountBetFromHuman=humanPlayer.FollowBet(minBet);playerHasBetOrFinish = true;break;
                case 3:amountBetFromHuman=0;playerHasBetOrFinish = true;break;
                case 4:humanPlayer.UserCheckCard();humanPlayer.CheckBet();break;
                case 5:
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
        return amountBetFromHuman;
    }
    //possibilité de refactoriser
    public int UserBet(int minBet,Player humanPlayer,List<Player>allPlayersInGame,ref bool hasBetOrFinish)
    {
        bool finish=false;
        int amountBet=0;
        Console.WriteLine($"\nVous avez {humanPlayer.Coin} jetons");
        while(!finish)
        {
            int stop=0;
            int AllInChoice=0;
            if(minBet>humanPlayer.Coin)
            {
                Console.WriteLine($"\nLe minimum a miser est de {minBet}, ce qui est supérieur à votre pot {humanPlayer.Coin} Voulez-vous all-in? \n1.OUI\n2.NON, vous vous couchez");
                while(AllInChoice!=1&&AllInChoice!=2)
                {
                    AllInChoice=Gestion.IntEnter();
                    if(AllInChoice==1)
                    {
                        amountBet=humanPlayer.PlayerAllIn();
                        hasBetOrFinish=true;
                        humanPlayer.CheckBet();
                        finish=true;
                    }
                        
                    else if(AllInChoice==2)
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
            amountBet=Gestion.IntEnter();
            if(amountBet<minBet)
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
                amountBet=humanPlayer.Bet(amountBet);
                hasBetOrFinish=true;
                Console.WriteLine($"\nVotre pot est de {humanPlayer.Coin}");
                Console.WriteLine($"\nVotre mise totale est de {humanPlayer.BetOfTheRound}");
                finish=true;
            }
        }
        return amountBet;
    }
    public void WhoWin(List<Player> allPlayers,List<Card> mainCards,int mainPot)
    {
        string combination;
        var winner = new Player();
        var winners=new List<Player>();
        (winners,winner,combination)=Rule.RuleFollowRoyalFlush(allPlayers,mainCards);
        if(CheckWinner(winners,winner,combination,mainCards,mainPot))return;
    
        (winners,winner,combination)=Rule.RuleFollowFlush(allPlayers,mainCards);
        if(CheckWinner(winners,winner,combination,mainCards,mainPot))return;

        (winners,winner,combination)=Rule.RuleSquare(allPlayers,mainCards);
        if(CheckWinner(winners,winner,combination,mainCards,mainPot))return;

        (winners,winner,combination)=Rule.RuleFull(allPlayers,mainCards);
        if(CheckWinner(winners,winner,combination,mainCards,mainPot))return;

        (winners,winner,combination)=Rule.RuleSameCardColor(allPlayers,mainCards);
        if(CheckWinner(winners,winner,combination,mainCards,mainPot))return;

        (winners,winner,combination)=Rule.RuleFollow(allPlayers,mainCards);
        if(CheckWinner(winners,winner,combination,mainCards,mainPot))return;

        (winners,winner,combination)=Rule.RuleThreeSameKind(allPlayers,mainCards);
        if(CheckWinner(winners,winner,combination,mainCards,mainPot))return;

        (winners,winner,combination)=Rule.RuleDoublePair(allPlayers,mainCards);
        if(CheckWinner(winners,winner,combination,mainCards,mainPot))return;

        (winners,winner,combination)=Rule.RulePair(allPlayers,mainCards);
        if(CheckWinner(winners,winner,combination,mainCards,mainPot))return;

        (winners,winner,combination)=Rule.RuleHightCard(allPlayers);
        if(CheckWinner(winners,winner,combination,mainCards,mainPot))return;
    }
    public void OneWinnerOrMore(List<Player>winners,Player winner,string combination,List<Card>cardsOnTable,int mainPot)
    {
        if(winners.Any())
            Player.MultiplePLayersWin(mainPot,winners,combination,cardsOnTable);
        else
            winner.PlayerWin(mainPot,combination,cardsOnTable);
    }
    public bool CheckWinner(List<Player>winners,Player winner,string combination,List<Card>cardsOnTable,int mainPot)
    {
        if(winner.Name!=null)
        {
            OneWinnerOrMore(winners,winner,combination,cardsOnTable,mainPot);
            return true;
        }
        return false;
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

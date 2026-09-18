namespace Poker;

using System.ComponentModel.DataAnnotations;
using Poker.Models;
public class Gestion
{
    Player player=new Player();
    List<Bot> bots=new List<Bot>();
    
    public Gestion()
    {
        Menu();
    }
    public void Menu()
    {
        int choice=1;
        
        while(choice!=3)
        {
            int i=1;
            string[] options={"Jouer","Tester","Quitter"};
            foreach(string option in options)
            {
                Console.WriteLine($"{i}.{option}");
                i++;
            }
            choice=IntEnter();
            switch(choice)
            {
                case 1: StartGame();break;
                case 2: Test();break;
                case 3: Console.WriteLine("Fin du jeu");break;
            }
        }
        
    }
    public void StartGame()
    {
        int minBet=50;
        int countRound=1;
        var newPlayer=new Player();
        var allPlayers=new List<Player>();
        var allPlayersForRound=new List<Player>();
        allPlayers.AddRange(bots);
        allPlayers.Add(player);
        string name=null;
        while(string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("\nQuelle est votre nom? ");
            name=Console.ReadLine();
        }
        newPlayer.Name=name;
        var gameEngine=new GameEngine();
        gameEngine.Init(bots);
        allPlayersForRound=allPlayers;
        while(allPlayers.Count()!=1)
        {
            gameEngine.Round(allPlayersForRound,minBet);//quand tous les tours sont finis
            countRound++;
            
            allPlayers=Player.RemoveLoserPlayer(allPlayers);
            double newBetMin=countRound/10;
            if(newBetMin<1)
                minBet=50;
            else
                minBet=50*((int)newBetMin+1);
            allPlayersForRound=allPlayers;
                
        }
    }
    public void Game()
    {
        
    }
    public void Test()
    {
        string combination;
        var listGagnant=new List<Player>();
        var listPlayers=new List<Player>();
        var winner=new Player();
        var player1=new Player
        {
            Name="Math",
            Deck=[
                new (11) {  Type = TypeCard.Coeur },
                new (2) { Type = TypeCard.Coeur }
            ]

        };
        var player2=new Player()
        {
            Name="François",
            Deck=new Card[]
            {
                new Card(14) {  Type = TypeCard.Coeur },
                new Card(2) { Type = TypeCard.Coeur }
            }
        };
        var player3=new Player()
        {
            Name="Michel",
            Deck=new Card[]
            {
                new Card(10) {  Type = TypeCard.Coeur },
                new Card(10) {  Type = TypeCard.Pique }
            }
        };
        listPlayers.Add(player1);
        listPlayers.Add(player2);
        listPlayers.Add(player3);
        var listCard=new List<Card>()
        {
            new Card(10) {  Type = TypeCard.Pique },
            new Card(3) { Type = TypeCard.Trefle },
            new Card(4) {  Type = TypeCard.Coeur },
            new Card(5) {  Type = TypeCard.Coeur },
            new Card(10) { Type = TypeCard.Coeur }
        };
        // (listGagnant,winner,combination)=Rule.RulePair(listPlayers,listCard);
        // (listGagnant,winner,combination)=Rule.RuleThreeSameKind(listPlayers,listCard);
        (listGagnant,winner,combination)=Rule.RuleFollow(listPlayers,listCard);
        // (listGagnant,winner,combination)=Rule.RuleFollowFlush(listPlayers,listCard);
        // (listGagnant,winner,combination)=Rule.RuleFollowRoyalFlush(listPlayers,listCard);
        // (listGagnant,winner,combination)=Rule.RuleFull(listPlayers,listCard);
        Console.WriteLine($"le gagnant est : {winner.Name}");
        if(listGagnant !=null)
        {
            Console.WriteLine($"Les autres gagnants sont : ");
            foreach(Player gagnant in listGagnant)
            {
                Console.WriteLine($"{gagnant.Name}");
            }
        }
        player1.UserCheckCard();
    }
    public static int IntEnter()
    {
        int intChoice;
        string choice=Console.ReadLine();
        int.TryParse(choice,out intChoice);
        return intChoice;
        
    }
}

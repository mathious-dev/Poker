namespace Poker;

using System.ComponentModel.DataAnnotations;
using Poker.Models;
public class Gestion
{
    List<Player> players=new List<Player>();
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
        var player=new Player();
        players.Add(player);
        string name=null;
        // while(players.Count()!=0)//ou si on clique sur une certaines touches pour terminer, ajouter plus tard
        // {
            while(string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("\nQuelle est votre nom? ");
                name=Console.ReadLine();
            }
            player.Name=name;
            var gameEngine=new GameEngine();
            gameEngine.Init(bots,players);
            Card.showCards(player.Deck);
        // }
    }
    public void Test()
    {
        var listGagnant=new List<Player>();
        var listPlayers=new List<Player>();
        var winner=new Player();
        var player1=new Player
        {
            Name="Math",
            Deck=[
                new (6) {  Type = TypeCard.Coeur },
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
        // (listGagnant,winner)=Rule.RulePair(listPlayers,listCard);
        // (listGagnant,winner)=Rule.RuleThreeSameKind(listPlayers,listCard);
        (listGagnant,winner)=Rule.RuleFollow(listPlayers,listCard);
        // (listGagnant,winner)=Rule.RuleFollowFlush(listPlayers,listCard);
        // (listGagnant,winner)=Rule.RuleFollowRoyalFlush(listPlayers,listCard);
        // (listGagnant,winner)=Rule.RuleFull(listPlayers,listCard);
        Console.WriteLine($"le gagnant est : {winner.Name}");
        if(listGagnant !=null)
        {
            Console.WriteLine($"Les autres gagnants sont : ");
            foreach(Player gagnant in listGagnant)
            {
                Console.WriteLine($"{gagnant.Name}");
            }
        }
    }
    public int IntEnter()
    {
        int intChoice;
        string choice=Console.ReadLine();
        int.TryParse(choice,out intChoice);
        return intChoice;
        
    }
}

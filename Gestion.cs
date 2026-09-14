namespace Poker;

using System.ComponentModel.DataAnnotations;
using Poker.Models;
public class Gestion
{
    public Gestion()
    {
        Init();
        Menu();
    }
    public void Init()
    {
        
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
        
    }
    public void Test()
    {
        var listGagnant=new List<Player>();
        var listPlayers=new List<Player>();
        var winner=new Player();
        var player1=new Player
        {
            Name="Math",
            Deck=new Card[]
            {
                new Card { Number = 3, Type = TypeCard.Coeur },
                new Card { Number = 10, Type = TypeCard.Pique }
            }

        };
        var player2=new Player()
        {
            Name="François",
            Deck=new Card[]
            {
                new Card { Number = 10, Type = TypeCard.Coeur },
                new Card { Number = 10, Type = TypeCard.Pique }
            }
        };
        var player3=new Player()
        {
            Name="Michel",
            Deck=new Card[]
            {
                new Card { Number = 10, Type = TypeCard.Coeur },
                new Card { Number = 10, Type = TypeCard.Pique }
            }
        };
        listPlayers.Add(player1);
        listPlayers.Add(player2);
        listPlayers.Add(player3);
        var listCard=new List<Card>()
        {
            new Card { Number = 8, Type = TypeCard.Carreau },
            new Card { Number = 2, Type = TypeCard.Pique },
            new Card { Number = 10, Type = TypeCard.Pique }
        };
        // (listGagnant,winner)=Rule.RulePair(listPlayers,listCard);
        (listGagnant,winner)=Rule.RuleThreeSameKind(listPlayers,listCard);
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

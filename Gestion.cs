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
        string[] options;
        int choice=1;
        int countGame=1;
        while(choice!=3)
        {
            int i=1;
            if(countGame>1)
                options=["Rejouer","Voir les combinaisons possibles","Configuration de jeu","Quitter"];
            else
                options=["Jouer","Voir les combinaisons possibles","Configuration de jeu","Quitter"];
            foreach(string option in options)
            {
                Console.WriteLine($"\n{i}.{option}");
                i++;
            }
            choice=IntEnter();
            switch(choice)
            {
                case 1: StartGame();countGame++;break;
                case 2: Combinations();break;
                case 3 :EditSettings();break;
                case 4: Console.WriteLine("Fin du jeu");break;
            }
        }
    }
    public void EditSettings()
    {
        string[] options={"Jetons de base","Mise de départ","Nombre de bots","Quitter"};
        int choice=0;
        while(choice!=4)
        {
            int i=1;
            foreach(string option in options)
            {
                Console.WriteLine($"\n{i}.{option}");
                i++;
            }
            Console.WriteLine("\nQue voulez-vous modifier?");
            choice=IntEnter();
            switch(choice)
            {
                case 1:MethodForEditCoinsAndAmountBet(true,false);break;
                case 2:MethodForEditCoinsAndAmountBet(false,true);break;
                case 3:;break;
                case 4:Console.WriteLine("\nFin des modifications...");Menu();break;
            }
        }
    }
    public void MethodForEditCoinsAndAmountBet(bool coins,bool amountBet)
    {
        int amount=0;
        bool valid=false;
        while(!valid)
        {
            Console.WriteLine("\nNouveau montant : ");
            amount=IntEnter();
            if(coins)
                EditCoinsGestion(ref valid,amount);
            else if(amountBet)
                EditAmountBetGestion(ref valid,amount);
        }
    }
    public void EditCoinsGestion(ref bool valid,int amount)
    {
        valid=GameConfig.EditCoins(amount);
    }
    public void EditAmountBetGestion(ref bool valid,int amount)
    {
        valid=GameConfig.EditAmountBet(amount);
    }
    public void StartGame()
    {
        int indexPlayerBigBind=0;
        int minBet=GameConfig.StartAmountBet;
        int countRound=1;
        var newPlayer=new Player();
        var allPlayers=new List<Player>();
        string name=null;
        Random randomBigBinder=new Random();

        while(string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("\nQuelle est votre nom? ");
            name=Console.ReadLine();
        }
        newPlayer.Name=name;
        player=newPlayer;
        var gameEngine=new GameEngine();
        gameEngine.Init(bots);
        newPlayer.OnMoneyBet+=gameEngine.AddBetOnMainPot;
        allPlayers.AddRange(bots);
        allPlayers.Add(player);
        Game(allPlayers,player,indexPlayerBigBind,minBet,countRound,randomBigBinder,gameEngine);
        bots.Clear();
        countRound=1;
    }
    public void Game(List<Player>allPlayers,Player player,int indexPlayerBigBind,int minBet,int countRound,Random randomBigBinder,GameEngine gameEngine)
    {
        while(allPlayers.Count(p=>p.Coin>0)>1&&allPlayers.Contains(player))
        {
            if(countRound==1)
                indexPlayerBigBind=randomBigBinder.Next(0,allPlayers.Count());
            Console.ForegroundColor=ConsoleColor.DarkBlue;    
            Console.WriteLine($"\n|||||||||||||||||||||||||||||||||||||||||||\nRound : {countRound}");
            Console.ResetColor();
            gameEngine.Round(allPlayers,minBet,indexPlayerBigBind,player);//quand tous les tours sont finis
            if(indexPlayerBigBind+1<allPlayers.Count()-1)
                indexPlayerBigBind++;
            else
                indexPlayerBigBind=0;
            bool findBigBindPlayer=false;
            while(!findBigBindPlayer)
            {
                if(allPlayers[indexPlayerBigBind].Coin<=0)
                {
                    if(indexPlayerBigBind>=allPlayers.Count()-1)
                        indexPlayerBigBind=0;
                    else
                        indexPlayerBigBind++;
                }
                else
                    findBigBindPlayer=true;
            }
            countRound++;
            allPlayers=Player.RemoveLoserPlayer(allPlayers);
            double newBetMin=countRound/10;
            if(newBetMin<1)
                minBet=50;
            else
                minBet=50*((int)newBetMin+1);
            foreach(Player p in allPlayers)
            {
                p.DefaultFields();
            }
        }
        if(!allPlayers.Contains(player))
        {
            Console.ForegroundColor=ConsoleColor.Red;
            Console.WriteLine("Game Over. Vous avez perdu");
            Console.ResetColor();
        }
            
        else
        {
            Console.ForegroundColor=ConsoleColor.Cyan;
            Console.WriteLine("Félicitations. Vous avez gagné");
            Console.ResetColor();
        }
    }
    public void Combinations()
    {
        Console.WriteLine($"\n1.Flush Royale\nUne suite de chiffre de même couleur allant de l'As jusqu'au valet.\nExemple : ");
        Console.WriteLine($"\n2.Suite Flush\nUne suite de chiffre allant de l'As jusqu'au valet.\nExemple : ");
        Console.WriteLine($"\n3.Carré\n4 cartes du même chiffre.\nExemple : ");
        Console.WriteLine($"\n4.Full\n3 cartes du même chiffre avec une paire.\nExemple : ");
        Console.WriteLine($"\n5.Couleur\nToutes les cartes ont la même couleur.\nExemple : ");
        Console.WriteLine($"\n6.Suite\nUne suite de chiffre.\nExemple : ");
        Console.WriteLine($"\n7.Brelan\n3 cartes du même chiffre.\nExemple : ");
        Console.WriteLine($"\n8.Double paire\n2 paires donc 2 fois des cartes du même chiffre.\nExemple : ");
        Console.WriteLine($"\n9.Paire\n2 cartes du même chiffre.\nExemple : ");
        Console.WriteLine($"\n10.Carte Haute\nLa carte la plus haute de votre deck.\nExemple : ");

    }
    public static int IntEnter()
    {
        int intChoice;
        string choice=Console.ReadLine();
        int.TryParse(choice,out intChoice);
        return intChoice;
        
    }
}

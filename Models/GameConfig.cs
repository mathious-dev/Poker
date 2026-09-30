namespace Poker.Models;

public static class GameConfig
{
    public static int StartCoins{get;set;}=1000;
    public static int StartAmountBet{get;set;}=50;
    public static void ShowInformations()
    {
        Console.WriteLine($"\nLes jetons de base pour les joueurs est de {StartCoins}\nLa mise de base est de {StartAmountBet}");
    }
    public static bool EditCoins(int newAmount)
    {
        if(newAmount<500||newAmount>20000||newAmount%2!=0)
        {
            Console.WriteLine("Erreur, le montant doit être entre 500 et 20000 jetons et un chiffre paire");
            return false;
        }
        else
        {
            StartCoins=newAmount;
            Console.WriteLine($"\nNouveau montant de jetons : {StartCoins}");
            return true;
        }
    }
    public static bool EditAmountBet(int newAmountBet)
    {
        if(newAmountBet<50||newAmountBet>StartCoins||newAmountBet>10000||newAmountBet%2!=0)
        {
            Console.WriteLine("Erreur, le montant de mise doit être entre 50 et 10000 ,un chiffre paire et surtout inférieur au pot de base du joueur");
            return false;
        }
        else
        {
            StartAmountBet=newAmountBet;
            Console.WriteLine($"\nNouvelle mise de base : {StartCoins}");
            return true;
        }
    }
}

namespace Poker.Models;

public static class GameConfig
{
    public static int StartCoins{get;set;}=1000;
    public static int StartAmountBet{get;set;}=50;
    public static int NumberBots{get;set;}=3;
    public static void ShowInformations()
    {
        Console.WriteLine($"\nLes jetons de base pour les joueurs est de {StartCoins}\nLa mise de base est de {StartAmountBet}");
    }
    public static bool EditCoins(int newAmount)
    {
        if(newAmount<500||newAmount>20000||newAmount%2!=0)
        {
            Console.WriteLine("\nLe montant doit être entre 500 et 20000 jetons et un chiffre paire");
            return false;
        }
        else
        {
            StartCoins=newAmount;
            return true;
        }
    }
    public static bool EditAmountBet(int newAmountBet)
    {
        if(newAmountBet<50||newAmountBet>StartCoins||newAmountBet>10000||newAmountBet%2!=0)
        {
            Console.WriteLine("\nLe montant de mise doit être entre 50 et 10000 ,un chiffre paire et surtout inférieur au pot de base du joueur");
            return false;
        }
        else
        {
            StartAmountBet=newAmountBet;
            return true;
        }
    }
    public static bool EditNumberBots(int newAmountBots)
    {
        if(newAmountBots<2||newAmountBots>6)
        {
            Console.WriteLine("\nLe nombre de bots doit être minimum de 2 bots et maximum de 6 bots");
            return false;
        }
        else
        {
            NumberBots=newAmountBots;
            return true;
        }
    }
}

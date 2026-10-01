namespace Poker.Models.Bots;

public class BotEasy : Bot
{
    public override void ProbabilityBet(int highestValue,int combinationInt,int maxCoin,ref int betMin,ref int betMax,int minBetOnTable)
    {
        switch (combinationInt)
        {
            case 1: // Carte Haute
                if (highestValue >= 12) // Dame, Roi, As (Grosse carte haute)
                {
                    if (minBetOnTable > maxCoin * 0.30) { betMin = 0; betMax = 0; } // Se couche si il n'a rien
                    else { betMin = minBetOnTable; betMax = Math.Max(minBetOnTable, (int)(maxCoin * 0.20)); } 
                }
                else // Petite carte haute (Rien du tout)
                {
                    if (minBetOnTable > maxCoin * 0.10) { betMin = 0; betMax = 0; }
                    else { betMin = minBetOnTable; betMax = minBetOnTable; }
                }
                break;

            case 2: // Paire
                if (highestValue >= 10) // Paire de 10 ou mieux
                {
                    betMin = minBetOnTable;
                    betMax = Math.Max(minBetOnTable, (int)(maxCoin * 0.50)); // S'emballe vite
                }
                else // Petite paire (2 à 9)
                {
                    betMin = minBetOnTable;
                    betMax = Math.Max(minBetOnTable, (int)(maxCoin * 0.25));
                }
                break;

            case 3: // Double Paire
                if (highestValue >= 11) // Avec au moins un Valet
                {
                    betMin = Math.Max(minBetOnTable, (int)(maxCoin * 0.20));
                    betMax = Math.Max(minBetOnTable, (int)(maxCoin * 0.80)); // Presque tapis
                }
                else // Petites doubles paires
                {
                    betMin = minBetOnTable;
                    betMax = Math.Max(minBetOnTable, (int)(maxCoin * 0.40));
                }
                break;

            default: // Brelan et plus (>= 4)
                betMin = Math.Max(minBetOnTable, (int)(maxCoin * 0.30));
                betMax = maxCoin; // All-in facile
                break;
        }
    }
}

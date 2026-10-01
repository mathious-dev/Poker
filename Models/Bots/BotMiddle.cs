namespace Poker.Models.Bots;

public class BotMiddle: Bot
{
    public override void ProbabilityBet(int highestValue,int combinationInt,int maxCoin,ref int betMin,ref int betMax,int minBetOnTable)
    {
        switch (combinationInt)
        {
            case 1: // Carte Haute
                if (highestValue >= 13) // Roi ou As (Peut valoir le coup de voir le flop)
                {
                    if (minBetOnTable > maxCoin * 0.10) { betMin = 0; betMax = 0; } // Mais pas trop cher
                    else { betMin = minBetOnTable; betMax = Math.Max(minBetOnTable, (int)(maxCoin * 0.05)); }
                }
                else // Moins qu'un Roi sans combinaison -> Poubelle face à une mise
                {
                    if (minBetOnTable > 0) { betMin = 0; betMax = 0; }
                    else { betMin = 0; betMax = 0; } 
                }
                break;

            case 2: // Paire
                if (highestValue >= 11) // Top Paire (Valet, Dame, Roi, As)
                {
                    if (minBetOnTable > maxCoin * 0.30) { betMin = 0; betMax = 0; } // Lâche si on lui demande plus de 30%
                    else { betMin = minBetOnTable; betMax = Math.Max(minBetOnTable, (int)(maxCoin * 0.25)); }
                }
                else // Petite Paire (2 à 10)
                {
                    if (minBetOnTable > maxCoin * 0.15) { betMin = 0; betMax = 0; } // Lâche plus vite
                    else { betMin = minBetOnTable; betMax = Math.Max(minBetOnTable, (int)(maxCoin * 0.15)); }
                }
                break;

            case 3: // Double Paire
                if (highestValue >= 11) // Grosse double paire
                {
                    betMin = Math.Max(minBetOnTable, (int)(maxCoin * 0.15));
                    betMax = Math.Max(minBetOnTable, (int)(maxCoin * 0.40));
                }
                else // Petite double paire
                {
                    betMin = minBetOnTable;
                    betMax = Math.Max(minBetOnTable, (int)(maxCoin * 0.25));
                }
                break;

            default: // Brelan, Suite, Couleur... (>= 4)
                if (highestValue >= 10) // Brelan de 10+ ou grosse suite
                {
                    betMin = Math.Max(minBetOnTable, (int)(maxCoin * 0.40));
                    betMax = maxCoin; 
                }
                else // Petit brelan
                {
                    betMin = Math.Max(minBetOnTable, (int)(maxCoin * 0.20));
                    betMax = Math.Max(minBetOnTable, (int)(maxCoin * 0.60));
                }
                break;
        }
    }
}

namespace Poker.Models.Bots;
public class BotHard: Bot
{
    public override void ProbabilityBet(int highestValue,int combinationInt,int maxCoin,ref int betMin,ref int betMax,int minBetOnTable)
    {
        switch (combinationInt)
        {
            case 1: // Carte Haute
                if (highestValue == 14) // As uniquement
                {
                    if (minBetOnTable > maxCoin * 0.05) { betMin = 0; betMax = 0; } // Paie juste pour voir si c'est quasi gratuit
                    else { betMin = minBetOnTable; betMax = minBetOnTable; } 
                }
                else 
                {
                    if (minBetOnTable > 0) { betMin = 0; betMax = 0; } // Fold instantané
                    else { betMin = 0; betMax = 0; }
                }
                break;

            case 2: // Paire
                if (highestValue >= 12) // Paire de Dame, Roi ou As
                {
                    if (minBetOnTable > maxCoin * 0.25) { betMin = 0; betMax = 0; } // Pot Control : ne s'enflamme pas avec une seule paire
                    else { betMin = minBetOnTable; betMax = Math.Max(minBetOnTable, (int)(maxCoin * 0.30)); } // Relance pour faire payer les autres
                }
                else // Paire moyenne/faible
                {
                    if (minBetOnTable > maxCoin * 0.10) { betMin = 0; betMax = 0; } // Fold facile
                    else { betMin = minBetOnTable; betMax = minBetOnTable; } // Just call
                }
                break;

            case 3: // Double Paire
                if (highestValue >= 12) // Top Double Paire
                {
                    // Value bet : il relance fort pour rentabiliser
                    betMin = Math.Max(minBetOnTable, (int)(maxCoin * 0.25));
                    betMax = Math.Max(minBetOnTable, (int)(maxCoin * 0.50));
                }
                else
                {
                    // Prudent face à une relance adverse qui pourrait cacher un brelan
                    if (minBetOnTable > maxCoin * 0.40) { betMin = 0; betMax = 0; } 
                    else { betMin = minBetOnTable; betMax = Math.Max(minBetOnTable, (int)(maxCoin * 0.30)); }
                }
                break;

            default: // Monstre (Combinaison 4 et plus)
                if (highestValue >= 10) 
                {
                    // Grosse carte haute + Gros jeu = Relance massive / Tapis
                    int strongRaise = Math.Max(minBetOnTable * 2, (int)(maxCoin * 0.50));
                    betMin = strongRaise > maxCoin ? maxCoin : strongRaise; 
                    betMax = maxCoin; 
                }
                else
                {
                    // Jeu monstre mais avec petites cartes (ex: Brelan de 2) -> Fait un peu plus attention
                    betMin = Math.Max(minBetOnTable, (int)(maxCoin * 0.30));
                    betMax = Math.Max(minBetOnTable, (int)(maxCoin * 0.70));
                }
                break;
        }
    }
}

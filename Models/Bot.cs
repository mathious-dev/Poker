namespace Poker.Models;

public class Bot : Player
{
    private int _level;
    public int Level
    {
        get{return _level;}
        set
        {
            if(value<4&&value>0)
                _level=value;
            else
                throw new ArgumentOutOfRangeException("Le niveau du bot doit être entre 1 et 3 compris");
        }
    }
    public Bot(int level)
    {
        this.Level=level;
    }
    public override int PlayerAllIn()
    {
        int amountBet=Bet(this.Coin);
        Console.WriteLine($"\nLe bot {this.Name} fait tapis !");
        return amountBet;
    }
    public int BotAction(int minBetOnTable,List<Card>?cards)
    {
        var (combinationStart,highestValue)=EvaluateDeck(this.Deck,cards);
        int amountBet=DeclarationOfAction( this.Level, highestValue, combinationStart, this.Coin,minBetOnTable);
        if(amountBet>=minBetOnTable)
        {
            if(amountBet==this.Coin)
                this.PlayerAllIn();
            else
                Bet(amountBet);
        }
            
        return amountBet;
    }
    public (int,int) EvaluateDeck(Card[] cardsOfBot,List<Card>?cardsOnTable)
    {
        int combinationNumber=0;
        int highestValue=0;
        var allCards=new List<Card>();
        if(cardsOnTable!=null&& cardsOnTable.Any())
            allCards=Rule.GroupCards(cardsOfBot,cardsOnTable);
        else
            allCards.AddRange(cardsOfBot);
        (combinationNumber,highestValue)=TestAllCombinationForBot(allCards);
        return (combinationNumber,highestValue);
    }
    public (int,int) TestAllCombinationForBot(List<Card> cards)
    {
        var royalFlush=Rule.GroupCardByFollowRoyalFlush(cards);//ok
        if(royalFlush.Any())
            return ((int)Rule.RuleEnum.RoyalFlush,royalFlush.First());

        var followFlush=Rule.GroupCardByFollowFlush(cards);//ok
        if(followFlush.Any())
            return((int)Rule.RuleEnum.FollowFlush,followFlush.First());

        var square=Rule.SearchPairOrThreeSameOrFourSame(cards,4);//ok
        if(square.Any())
            return((int)Rule.RuleEnum.Square,square.First());

        var pair=Rule.SearchPairOrThreeSameOrFourSame(cards,2);//ok
        var triple=Rule.SearchPairOrThreeSameOrFourSame(cards,3);
        if(triple.Any()&&pair.Any(p=>p!=triple.First())) //il faut bien vérifier que c'est différent sinon dès qu'on a brelan, il trouvera aussi une paire donc un faux full
            return((int)Rule.RuleEnum.Full,triple.First());

        var color=Rule.GroupCardByColor(cards);//ok
        if(color.Any())
            return((int)Rule.RuleEnum.Color,color.First());

        var follow=Rule.GroupCardByFollow(cards);//ok
        if(follow.Any())
            return((int)Rule.RuleEnum.Follow,follow.First());

        if(triple.Any())//ok
            return((int)Rule.RuleEnum.ThreeSameKind,triple.First());

        if(pair.Count()>=2) //ok
            return((int)Rule.RuleEnum.DoublePair,pair.First());

        if(pair.Any())//ok
            return((int)Rule.RuleEnum.Pair,pair.First());

        var highCard=cards.Max(c=>c.Number);//ok
        return((int)Rule.RuleEnum.HighCard,highCard);

    }
    public  bool Bluff(int level)
    {
        Random randomBluff=new Random();
        bool bluff=false;
        switch(level)
        {
            case 1: BluffBot(11,ref bluff);break;//facile
            case 2: BluffBot(6,ref bluff);break;//moyen
            case 3: BluffBot(4,ref bluff);break;//difficile
        }
       return bluff;
    }
    public  void BluffBot(int chanceForBluff,ref bool bluff)
    {
        Random randomBluff=new Random();
        int isBluffing=0;
        isBluffing=randomBluff.Next(1,chanceForBluff);
        if(isBluffing==1)
            bluff=true;
        else
            bluff=false;
    }
    //probabilités faites avec l'ia, à vérifier
    public  void BotLevelEasy(int highestValue,int combinationInt,int maxCoin,ref int betMin,ref int betMax,int minBetOnTable)
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
    public  void BotLevelMiddle(int highestValue,int combinationInt,int maxCoin,ref int betMin,ref int betMax,int minBetOnTable)
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
    public  void BotLevelHard(int highestValue,int combinationInt,int maxCoin,ref int betMin,ref int betMax,int minBetOnTable)
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
    public  int DeclarationOfAction(int level,int highestValue,int combinationStart,int maxCoin,int minBetOnTable)
    {
        int amountBet=0;
        int betMin=0;
        int betMax=0;
        bool bluffOrNot=false;
        Random randomBet=new Random();
        if(combinationStart==1)
            bluffOrNot=Bluff(level);
        if(!bluffOrNot)
        {
            switch(level)
            {
                case 1 : BotLevelEasy(highestValue,combinationStart,maxCoin,ref  betMin,ref  betMax, minBetOnTable);break;
                case 2 : BotLevelMiddle(highestValue,combinationStart,maxCoin,ref  betMin,ref  betMax, minBetOnTable);break;
                case 3 : BotLevelHard(highestValue,combinationStart,maxCoin,ref  betMin,ref  betMax, minBetOnTable);break;
            }
            if (betMin >= betMax) 
                amountBet = betMin; 
            else
                amountBet = randomBet.Next(betMin, betMax + 1); //+1 pour inclure le betMax
        }
        else
            amountBet=this.Coin;
        
        return amountBet;
    }
}

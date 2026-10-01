namespace Poker.Models.Bots;

public class Bot : Player
{
    public override void PlayerAllIn()
    {
        Bet(this.Coin);
        Console.WriteLine($"\nLe bot {this.Name} fait tapis !");
        this.AllIn=true;
    }
    public virtual void ProbabilityBet(int highestValue,int combinationInt,int maxCoin,ref int betMin,ref int betMax,int minBetOnTable)
    {
        
    }
    public void BotAction(int minBetOnTable,List<Card>?cards)
    {
        var (combinationStart,highestValue)=EvaluateDeck(this.Deck,cards);
        int amountBet=DeclarationOfAction( highestValue, combinationStart, this.Coin,minBetOnTable);
        if(amountBet>=minBetOnTable)
        {
            if(amountBet>=this.Coin)
                this.PlayerAllIn();
            else
                Bet(amountBet);
        }
    }
    public (int,int) EvaluateDeck(Card[] cardsOfBot,List<Card>?cardsOnTable)
    {
        int combinationNumber;
        int highestValue;
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
    public bool Bluff()
    {
        bool bluff=false;
        // switch(level)
        // {
        //     case 1: BluffBot(11,ref bluff);break;//facile
        //     case 2: BluffBot(6,ref bluff);break;//moyen
        //     case 3: BluffBot(4,ref bluff);break;//difficile
        // }
       return bluff;
    }
    public void BluffBot(int chanceForBluff,ref bool bluff)
    {
        int isBluffing;
        isBluffing=Random.Shared.Next(1,chanceForBluff);
        if(isBluffing==1)
            bluff=true;  
        else
            bluff=false;
    }
    public  int DeclarationOfAction(int highestValue,int combinationStart,int maxCoin,int minBetOnTable)
    {
        int amountBet=0;
        int betMin=0;
        int betMax=0;
        bool bluffOrNot=false;
        // if(combinationStart==1)
        //     bluffOrNot=Bluff(level);
        if(!bluffOrNot)
        {
            this.ProbabilityBet(highestValue,combinationStart,maxCoin,ref betMin,ref betMax,minBetOnTable);
            if (betMin >= betMax) 
                amountBet = betMin; 
            else
                amountBet = Random.Shared.Next(betMin, betMax + 1); //+1 pour inclure le betMax
        }
        else
            amountBet=this.Coin;
        return amountBet;
    }
}

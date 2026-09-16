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
    public void BotAction(int level,int highestValue,Rule.RuleEnum combinationStart,int maxCoin)
    {
        switch(level)
        {
            case 1 : BotLevelEasy(1,highestValue,combinationStart,maxCoin);break;
            case 2 : BotLevelMiddle(2,highestValue,combinationStart,maxCoin);break;
            case 3 : BotLevelHard(3,highestValue,combinationStart,maxCoin);break;
        }
    }
    //à finir
    public void BotLevelEasy(int level,int highestValue,Rule.RuleEnum combinationStart,int maxCoin)
    {
        int combinationInt=(int)combinationStart;
        int amountBet=0;
        Random randomBet=new Random();
        if(combinationInt<3)
            amountBet=randomBet(maxCoin-20%,maxCoin-50%);
        // Bet();
    }
    public void BotLevelMiddle(int level,int highestValue,Rule.RuleEnum combinationStart,int maxCoin)
    {
        
    }
    public void BotLevelHard(int level,int highestValue,Rule.RuleEnum combinationStart,int maxCoin)
    {
        
    }
}

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
    public void BotAction(int level,int highestValue,Rule.RuleEnum combinationStart,int maxCoin,ref int  mainPot)
    {
        
        int amountBet=DeclarationOfAction( level, highestValue, combinationStart, maxCoin);
        Bet(ref mainPot,amountBet);
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
    
    //il faudra définir en fonction de la mise obligatoire aussi
    public  void BotLevelEasy(int highestValue,int combinationInt,int maxCoin,ref int betMin,ref int betMax)
    {
        switch((combinationInt,highestValue))
        {
            case(1,>10):
            betMin=(int)(maxCoin*0.30);
            betMax=(int)(maxCoin*1);
            break;
            case(1,<6):
            betMin=(int)(maxCoin*0.20);
            betMax=(int)(maxCoin*0.40);
            break;
            case(2,<9):
            betMin=(int)(maxCoin*0.60);
            betMax=(int)(maxCoin*0.90);
            break;
            case(<3,>9):
            betMin=(int)(maxCoin*0.70);
            betMax=(int)(maxCoin*0.90);
            break;
            case(>3,>2):
            betMin=(int)(maxCoin*1);
            betMax=(int)(maxCoin*1);
            break;
            case(<14,>2):
            betMin=(int)(maxCoin*1);
            betMax=(int)(maxCoin*1);
            break;
            default:
            betMin=0;
            betMax=0;
            ;break;
        }
    }
    //définir chance
    public  void BotLevelMiddle(int highestValue,int combinationInt,int maxCoin,ref int betMin,ref int betMax)
    {
        switch((combinationInt,highestValue))
        {
            case(1,>10):
            betMin=(int)(maxCoin*0.30);
            betMax=(int)(maxCoin*1);
            break;
            case(2,<9):
            betMin=(int)(maxCoin*0.60);
            betMax=(int)(maxCoin*0.90);
            break;
            case(<3,>9):
            betMin=(int)(maxCoin*0.70);
            betMax=(int)(maxCoin*0.90);
            ;break;
            case(>3,>2):
            betMin=(int)(maxCoin*1);
            betMax=(int)(maxCoin*1);
            break;
            default:
            betMin=0;
            betMax=0;
            ;break;
        }
    }
    //définir chance
    public  void BotLevelHard(int highestValue,int combinationInt,int maxCoin,ref int betMin,ref int betMax)
    {
        switch((combinationInt,highestValue))
        {
            case(1,>10):
            betMin=(int)(maxCoin*0.30);
            betMax=(int)(maxCoin*1);
            break;
            case(1,<6):
            betMin=(int)(maxCoin*0.20);
            betMax=(int)(maxCoin*0.40);
            break;
            case(2,<9):
            betMin=(int)(maxCoin*0.60);
            betMax=(int)(maxCoin*0.90);
            break;
            case(<3,>9):
            betMin=(int)(maxCoin*0.70);
            betMax=(int)(maxCoin*0.90);
            break;
            case(>3,>2):
            betMin=(int)(maxCoin*1);
            betMax=(int)(maxCoin*1);
            break;
            default:
            betMin=0;
            betMax=0;
            ;break;
        }
    }
    public  int DeclarationOfAction(int level,int highestValue,Rule.RuleEnum combinationStart,int maxCoin)
    {
        int combinationInt=(int)combinationStart;
        int amountBet=0;
        int betMin=0;
        int betMax=0;
        bool bluffOrNot=false;
        Random randomBet=new Random();
        if(combinationInt==1)
            bluffOrNot=Bluff(level);
        if(!bluffOrNot)
        {
            switch(level)
            {
                case 1 : BotLevelEasy(highestValue,combinationInt,maxCoin,ref  betMin,ref  betMax);break;
                case 2 : BotLevelMiddle(highestValue,combinationInt,maxCoin,ref  betMin,ref  betMax);break;
                case 3 : BotLevelHard(highestValue,combinationInt,maxCoin,ref  betMin,ref  betMax);break;
            }
            amountBet=randomBet.Next(betMin,betMax);
        }
        else
            amountBet=maxCoin;
        
        return amountBet;
    }
}

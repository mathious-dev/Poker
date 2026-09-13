namespace Poker;

public class Rule
{
    public enum RuleEnum
    {
        HighCard,
        Pair,
        DoublePair,
        ThreeSameKind,
        Follow,
        Color,
        Full,
        Square,
        FollowFlush,
        RoyalFlush
    }
    public RuleEnum combination{get;set;}
    public (List<Player>,Player) RuleHightCard(List<Player> players)
    {
        List<Player> listPlayersWithSameHigh=new List<Player>();
        int higherCard=2;
        Player winner=new Player();
        foreach(Player player in players)
        {
            foreach(Card card in player.Deck)
            {
                if(card.Number>higherCard)
                {
                    higherCard=card.Number;
                    winner=player;
                }
            }
        }
        return (listPlayersWithSameHigh,winner);
    }
    public (List<Player>,Player) RulePair(List<Player>players,int[] cardPlace)
    {
    
    }
}

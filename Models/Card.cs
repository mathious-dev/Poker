namespace Poker.Models;
public enum TypeCard{
        Carreau,
        Pique,
        Coeur,
        Trefle
    }
public class Card
{
    
    public TypeCard Type{get;set;}
    private int _number;
    public int Number
    {
        get{return _number;}
        set
        {
            if(value<15&&value>0)
                _number=value;
            else
                throw new ArgumentOutOfRangeException("Le nombre doit être entre 1 et 14 compris");
        }
    }
    public Card(int number)
    {
        this.Number=number;
    }
    
}

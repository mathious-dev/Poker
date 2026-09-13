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
    public int Number{get;set;}
    // public Card StartPlacingCard(int tour,int[]? cardPlace)
    // {
    //     if(tour==1 &&cardPlace is null)
    //     {
    //         Card newCard=new Card();
    //         return newCard;
    //     }
    // }
    // public Card StartGame()
    // {
        
    // }
}

namespace Poker.Models;
public enum TypeCard{
        Carreau=1,
        Pique,
        Coeur,
        Trefle
    }
public enum FaceCard
{
    Valet = 11,
    Dame = 12,
    Roi = 13,
    As = 14
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
    public static List<Card> GeneralDeckCard()
    {
        var listCards=new List<Card>();
        for(int i=2;i<=14;i++)//car l'As va valoir à la fois 1 et 14
        {
            for(int j=1;j<5;j++)
            {
                listCards.Add(new Card(i){Type=(TypeCard)j});
            }
        }
        return listCards;
    }
    public static void ShowCards(IEnumerable<Card> cards)
    {
        if(cards.Any())//si c'est une liste donc les cartes sur la table
        {
            Console.WriteLine($"\nVoici les cartes sur la table\n");
            foreach(Card card in cards)
            {
                if(card.Number>10)
                    Console.WriteLine($"{(FaceCard)card.Number} de {card.Type} ");
                else
                    Console.WriteLine($" {card.Number} de {card.Type}");
            }
        }
        else if(cards!=null)//si c'est un tableau donc le Deck du joueur
        {
            Console.WriteLine($"\nVoici les cartes du joueur\n");
            foreach(Card card in cards)
            {
                if(card.Number>10)
                    Console.WriteLine($"{(FaceCard)card.Number} de {card.Type} ");
                else
                    Console.WriteLine($" {card.Number} de {card.Type}");
            }
        }
        
    }
}

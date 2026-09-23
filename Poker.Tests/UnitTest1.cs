namespace Poker.Tests;
using Poker.Models;
public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        //Arrange
        var player1=new Player
        {
            Name="Math",
            Deck =
            [
                new(5){Type=TypeCard.Carreau},
                new(7){Type=TypeCard.Carreau}
            ]
        };
        var player2=new Player
        {
            Name="Benoit",
            Deck =
            [
                new(6){Type=TypeCard.Pique},
                new(14){Type=TypeCard.Carreau}
            ]
        };
        var listPlayer=new List<Player>();
        listPlayer.Add(player1);
        listPlayer.Add(player2);
        var listCard=new List<Card>()
        {
            new Card(10) {  Type = TypeCard.Pique },
            new Card(10) { Type = TypeCard.Trefle },
            new Card(10) {  Type = TypeCard.Coeur },
            new Card(8) {  Type = TypeCard.Coeur },
            new Card(10) { Type = TypeCard.Coeur }
        };
        //Act
        (var othersWinners,var winner,var combination) =Rule.RuleSquare(listPlayer,listCard);
        System.Console.WriteLine($"Le gagnat est : {winner} avec une combinaison de {combination}");
        
        //Assert
        Assert.Equal(player2,winner);
        Assert.Empty(othersWinners);

    }
}
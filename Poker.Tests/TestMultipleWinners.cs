namespace Poker.Tests;
using Poker.Models;
public class TestMultipleWinners
{
     [Fact]
    public void Test()
    {
        //Arrange
        var player1=new Player
        {
            Name="Math",
            Deck =
            [
                new(13){Type=TypeCard.Carreau},
                new(7){Type=TypeCard.Carreau}
            ]
        };
        var player2=new Player
        {
            Name="Benoit",
            Deck =
            [
                new(13){Type=TypeCard.Pique},
                new(7){Type=TypeCard.Pique}
            ]
        };
        var listPlayer=new List<Player>();
        listPlayer.Add(player1);
        listPlayer.Add(player2);
        var listCard=new List<Card>()
        {
            new Card(13) {  Type = TypeCard.Coeur },
            new Card(4) { Type = TypeCard.Trefle },
            new Card(6) {  Type = TypeCard.Coeur },
            new Card(8) {  Type = TypeCard.Coeur },
            new Card(2) { Type = TypeCard.Coeur }
        };
        //Act
        (var othersWinners,var winner,var combination) =Rule.RulePair(listPlayer,listCard);
        var winners=new List<Player>();
        winners.Add(winner);
        winners.AddRange(othersWinners);
        //Assert
        Assert.Contains(player1, winners);
        Assert.Contains(player2, winners);
        Assert.Equal(2, winners.Count);
    }
}

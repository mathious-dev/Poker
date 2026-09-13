namespace Poker;

public class Gestion
{
    public Gestion()
    {
        Init();
        Menu();
    }
    public void Init()
    {
        
    }
    public void Menu()
    {
        int choice=1;
        int i=1;
        while(choice!=3)
        {
            string[] options={"Jouer","Tester","Quitter"};
            foreach(string option in options)
            {
                Console.WriteLine($"{i}.{option}");
                i++;
            }
            choice=IntEnter();
            switch(choice)
            {
                case 1: break;
                case 2: break;
                case 3: Console.WriteLine("Fin du jeu");break;
            }
        }
        
    }
    public int IntEnter()
    {
        int intChoice;
        string choice=Console.ReadLine();
        int.TryParse(choice,out intChoice);
        return intChoice;
        
    }
}

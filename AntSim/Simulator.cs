using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntSim
{
    public class Simulator
    {
        private const int maxFoodQuantity = 30;
        private const int maxFoodCells = 50;

        public World World { get; private set; }
        public List<Ant> Ants { get; private set; }

        public Simulator(int worldDimensions, int homeDimensions, Location homeStart)
        {
            // init main world
            World = new World(worldDimensions, homeDimensions, homeStart);
            Ants = new List<Ant>();

            Random random = new Random();

            // add food
            for (var i = 0; i < maxFoodCells; i++)
            {
                Cell c = World.RandomCell(random);
                if(c.AvailableFood == 0)
                    c.AvailableFood = maxFoodQuantity;
                //Cell c = World.RandomCell(random);
                //Console.WriteLine(c.Location.X + "," + c.Location.Y);
            }

            //foreach(var c in World.AllCells)
            //{
            //    if(c.AvailableFood > 0)
            //        Console.WriteLine(c.Location.X + " " + c.Location.Y);
            //}

            // add food
            for (var i = 0; i < 1; i++)
            {
                Cell c = World.RandomHomeCell(random);
                if (c.Ant == null)
                {
                    c.Ant = new Ant(World, c.Location, (FacingDirection)random.Next(0, 8));
                    Ants.Add(c.Ant);
                }
                //Cell c = World.RandomCell(random);
                //Console.WriteLine(c.Location.X + "," + c.Location.Y);
            }

        }

        public void TimeTick()
        {
            foreach(Ant a in Ants)
            {
                Console.WriteLine("Ant was at: " + a.Location.ToString());

                Action action = Action.Idle; ;
                var simpleMind = new SimpleMind(a.CurrentCell(), a.AheadCells());

                if (a.IsForaging())
                    action = simpleMind.seekFood();
                else
                    action = simpleMind.seekHome();

                Console.WriteLine(action);

                switch (action)
                {
                    case Action.MoveForward:
                        a.MoveForward();
                        break;
                    case Action.TurnLeft:
                        a.Turn(-1);
                        break;
                    case Action.TurnRight:
                        a.Turn(1);
                        break;
                    case Action.TakeFood:
                        a.TakeFood();
                        a.Turn(4);
                        break;
                    case Action.DropFood:
                        a.DropFood();
                        a.Turn(4);
                        break;
                }

                Console.WriteLine("Ant is now at: " + a.Location.ToString());
            }
        }


    }
}

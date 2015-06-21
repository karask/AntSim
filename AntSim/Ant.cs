using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntSim
{
    public class Ant
    {
        private Location[] directionDelta;

        private World World;
        public Location Location { get; set; }
        public FacingDirection Facing { get; set; }
        public bool HasFood { get; set; }

        public Ant(World world, Location location, FacingDirection facing)
        {
            Location = location;
            Facing = facing;
            World = world;
            // new relevant coordinates -- correspond to FacingDirections
            directionDelta = new Location[8] {
                new Location(0, -1),
                new Location(1, -1),
                new Location(1, 0),
                new Location(1, 1),
                new Location(0, 1),
                new Location(-1, 1),
                new Location(-1, 0),
                new Location(-1, -1)
            };

            Console.WriteLine(location.ToString());
        }

        public bool IsForaging()
        {
            return !HasFood;
        }

        public void MoveForward()
        {
            Cell aheadCell = AheadCell();
            Cell currentCell = CurrentCell();

            if (aheadCell != null)
            {
                aheadCell.Ant = this;
                this.Location = aheadCell.Location;
                currentCell.Ant = null;
                Console.WriteLine("Moving to: " + Location.ToString());
            }
        }

        public void Turn(int angle)
        {
            if (angle < 0 && this.Facing == FacingDirection.N)
                this.Facing = FacingDirection.NW;
            else
                this.Facing = (FacingDirection)(((int)Facing + angle) % 8);
        }

        public void TakeFood()
        {
            this.HasFood = true;
            Cell c = CurrentCell();
            c.AvailableFood -= 1;
        }

        public void DropFood()
        {
            this.HasFood = false;
            Cell c = CurrentCell();
            c.AvailableFood += 1;
        }


        public Cell CurrentCell()
        {
            return World.GetCell(Location);
        }

        public Dictionary<string, Cell> AheadCells()
        {
            Dictionary<string, Cell> aheadCells = new Dictionary<string, Cell>();

            Location ahead = CurrentCell().Location + directionDelta[(int)Facing];
            Location aheadLeft;
            if (Facing == FacingDirection.N)
                aheadLeft = CurrentCell().Location + directionDelta[7];
            else
                aheadLeft = CurrentCell().Location + directionDelta[((int)Facing - 1) % 8];
            Location aheadRight = CurrentCell().Location + directionDelta[((int)Facing + 1) % 8];

            aheadCells.Add("ahead", checkValidLocation(ahead) ? World.GetCell(ahead) : null);
            aheadCells.Add("aheadLeft", checkValidLocation(aheadLeft) ? World.GetCell(aheadLeft) : null);
            aheadCells.Add("aheadRight", checkValidLocation(aheadRight) ? World.GetCell(aheadRight) : null);

            Console.WriteLine(Facing.ToString());
            foreach(var kValue in aheadCells)
            {
                Console.WriteLine(kValue.Key + "=" + kValue.Value.Location.ToString());
            }
            return aheadCells;
        }

        public Cell AheadCell()
        {
            Location ahead = CurrentCell().Location + directionDelta[(int)Facing];
            //Console.WriteLine("Facing " + Facing.ToString() + " - next loc: " + World.GetCell(ahead).Location.ToString());
            return checkValidLocation(ahead) ? World.GetCell(ahead) : null;
        }


        private bool checkValidLocation(Location loc)
        {
            if (loc.X >= World.WorldDimensions || loc.X < 0 || loc.Y >= World.WorldDimensions || loc.Y < 0)
                return false;

            return true;
        }
    }


    public enum FacingDirection : byte
    {
        N,
        NE,
        E,
        SE,
        S,
        SW,
        W,
        NW
    }

    public enum Goals : byte
    {
        SeekFood,
        SeekHome
    }

    public enum Action: byte
    {
        Idle,
        MoveForward,
        TurnLeft,
        TurnRight,
        TakeFood,
        DropFood
    }

}

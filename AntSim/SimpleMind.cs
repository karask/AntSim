using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntSim
{
    public class SimpleMind
    {
        private Cell currentCell;
        private Cell ahead, aheadLeft, aheadRight;
        private static Random random = new Random();

        public SimpleMind(Cell currentCell, Dictionary<string, Cell> aheadCells)
        {
            //random = new Random();
            this.currentCell = currentCell;
            this.ahead = aheadCells["ahead"];
            this.aheadLeft = aheadCells["aheadLeft"];
            this.aheadRight = aheadCells["aheadRight"];
        }

        public Action seekFood()
        {
            if (currentCell.hasFood() && !currentCell.IsHome)
                return Action.TakeFood;
            else if (ahead != null && ahead.hasFood() && !ahead.IsHome && ahead.Ant == null)
                return Action.MoveForward;
            else if (aheadLeft != null && aheadLeft.hasFood() && !aheadLeft.IsHome && aheadLeft.Ant == null)
                return Action.TurnLeft;
            else if (aheadRight != null && aheadRight.hasFood() && !aheadRight.IsHome && aheadRight.Ant == null)
                return Action.TurnRight;

            else 
                return (Action)random.Next((int)Action.MoveForward, (int)Action.TakeFood);

            return Action.Idle;
        }

        public Action seekHome()
        {
            if (currentCell.IsHome)
                return Action.DropFood;
            else if (ahead != null && ahead.IsHome)
                return Action.MoveForward;
            else if (aheadLeft != null && aheadLeft.IsHome)
                return Action.TurnLeft;
            else if (aheadRight != null && aheadRight.IsHome)
                return Action.TurnRight;

            else
                return (Action)random.Next((int)Action.MoveForward, (int)Action.TakeFood);

            return Action.Idle;
        }

    }
}

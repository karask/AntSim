using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntSim
{
    // simple rules: http://mute-net.sourceforge.net/howAnts.shtml
    public class SimpleMind
    {
        private Cell currentCell;
        private Dictionary<string, Cell> aheadCells;
        private Cell ahead, aheadLeft, aheadRight;
        private static Random random = new Random();

        public SimpleMind(Cell currentCell, Dictionary<string, Cell> aheadCells)
        {
            //random = new Random();
            this.currentCell = currentCell;
            this.aheadCells = aheadCells;

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
                return actionFromPheromones(aheadCells, false);

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
                return actionFromPheromones(aheadCells, true);

        }

        private Action actionFromPheromones(Dictionary<string, Cell> aheadCells, bool hasFood)
        {
            // remove null Cell values from dictionary
            foreach (var item in aheadCells.Where(kvp => kvp.Value == null).ToList())
            {
                aheadCells.Remove(item.Key);
            }

            // if no ahead cells, either turn left or right action allowed
            if(aheadCells.Count() == 0) 
                return (Action)random.Next((int)Action.TurnLeft, (int)Action.TakeFood);

            if(hasFood)
            {
                // if all ahead same then either move or turn
                if(aheadCells.All(kvp => kvp.Value.HomePheromone == aheadCells.FirstOrDefault().Value.HomePheromone))
                    return (Action)random.Next((int)Action.MoveForward, (int)Action.TakeFood);

                // get string key for cell with max FoodPheromone
                string maxKey = string.Empty;
                float maxPheromone = 0;
                foreach (var kvp in aheadCells)
                {
                    if (kvp.Value != null)
                    {
                        if (kvp.Value.HomePheromone > maxPheromone)
                        {
                            maxKey = kvp.Key;
                            maxPheromone = kvp.Value.HomePheromone;
                        }
                    }
                }

                switch(maxKey)
                {
                    case "ahead":
                        return Action.MoveForward;
                        break;
                    case "aheadLeft":
                        return Action.TurnLeft;
                        break;
                    case "aheadRight":
                        return Action.TurnRight;
                        break;
                    default:
                        return (Action)random.Next((int)Action.MoveForward, (int)Action.TakeFood);
                }
            }
            else
            {
                // if all ahead same then either move or turn
                if (aheadCells.All(kvp => kvp.Value.FoodPheromone == aheadCells.FirstOrDefault().Value.FoodPheromone))
                    return (Action)random.Next((int)Action.MoveForward, (int)Action.TakeFood);

                // get string key for cell with max HomePheromone
                string maxKey = string.Empty;
                float maxPheromone = 0;
                foreach (var kvp in aheadCells)
                {
                    if (kvp.Value != null)
                    {
                        if (kvp.Value.FoodPheromone > maxPheromone)
                        {
                            maxKey = kvp.Key;
                            maxPheromone = kvp.Value.FoodPheromone;
                        }
                    }
                }

                // if stronger pheromone cell is home (note that we search for food) then turn in the opposite direction
                switch (maxKey)
                {
                    case "ahead":
                        if (aheadCells["ahead"].IsHome)
                            return Action.TurnDirection;
                        else
                            return Action.MoveForward;
                        break;
                    case "aheadLeft":
                        return Action.TurnLeft;
                        break;
                    case "aheadRight":
                        return Action.TurnRight;
                        break;
                    default:
                        return (Action)random.Next((int)Action.MoveForward, (int)Action.TakeFood);
                }
            }

            
        }

    }
}

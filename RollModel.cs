namespace RollCount
{
    public class RollModel
    {
        public RollModel(int rollMaxNumber, params int[] rolls)
        {
            RollMaxNumber = rollMaxNumber;
            Rolls = [.. rolls];
        }

        public RollModel(int rollMaxNumber)
        {
            RollMaxNumber = rollMaxNumber;
            Rolls = [.. Enumerable.Range(1, rollMaxNumber)];
        }


        public int RollMaxNumber { get; set; }
        public List<int> Rolls { get; set; } = [];
    }
}

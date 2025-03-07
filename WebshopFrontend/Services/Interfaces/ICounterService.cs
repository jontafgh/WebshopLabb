namespace WebshopFrontend.Services.Interfaces
{
    public interface ICounterService
    {
        public int GetCount();
        public int AddToCount(int numberToAdd);
        public int SetCount(int numberToSet);

        public event EventHandler<int> CounterChanged;
    }
}

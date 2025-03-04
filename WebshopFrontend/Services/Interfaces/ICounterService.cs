namespace WebshopFrontend.Services.Interfaces
{
    public interface ICounterService
    {
        public int GetCount();
        public int AddToCount(int numberToAdd);

        public event EventHandler<int> CounterChanged;
    }
}

using WebshopFrontend.Services.Interfaces;

namespace WebshopFrontend.Services
{
    public class CartCounterService : ICounterService
    {
        private int _counter = 0;

        public event EventHandler<int>? CounterChanged;

        public int GetCount()
        {
            return _counter;
        }

        public int AddToCount(int numberToAdd)
        {
            _counter += numberToAdd;
            CounterChanged?.Invoke(this, _counter);
            return _counter;
        }

    }
}

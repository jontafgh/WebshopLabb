﻿using WebshopFrontend.Contracts;

namespace WebshopFrontend.Services
{
    public class CartCounterService : ICounterService
    {
        private int _counter;

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

        public int SetCount(int numberToSet)
        {
            _counter = numberToSet;
            CounterChanged?.Invoke(this, _counter);
            return _counter;
        }
    }
}

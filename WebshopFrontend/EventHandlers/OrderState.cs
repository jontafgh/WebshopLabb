using WebshopShared;
namespace WebshopFrontend.EventHandlers
{
    public class OrderState
    {
        public OrderDto Order { get; set; }
        public event Action OnStateChange;
        public void SetOrder(OrderDto order)
        {
            Order = order;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnStateChange?.Invoke();
    }
}

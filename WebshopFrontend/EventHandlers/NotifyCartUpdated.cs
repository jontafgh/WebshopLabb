namespace WebshopFrontend.EventHandlers
{
    public class NotifyCartUpdated
    {
        public event EventHandler? CartUpdated;

        public void NotifyCartUpdate(object sender)
        {
            this.CartUpdated?.Invoke(sender, EventArgs.Empty);
        }
    }
}

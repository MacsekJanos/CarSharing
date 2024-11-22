namespace IVCFB2_HSZF_2024251.Persistence.MsSql
{
    public class DBEvent : EventArgs
    {
        public event EventHandler<string>? ActionCompleted;

        public DBEvent()
        {
            ActionCompleted += (sender, message) => Console.WriteLine(message);
        }
        public void OnActionCompleted(string message)
        {
            ActionCompleted?.Invoke(this, message);
        }
    }
}
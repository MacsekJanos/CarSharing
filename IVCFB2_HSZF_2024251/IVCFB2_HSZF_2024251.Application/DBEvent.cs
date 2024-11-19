namespace IVCFB2_HSZF_2024251.Application
{
    public class DBEvent
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

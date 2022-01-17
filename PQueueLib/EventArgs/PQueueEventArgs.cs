namespace PQueueLib.EventArgs
{
    public class PQueueEventArgs: System.EventArgs
    {
        public string Message{ get; }

        public PQueueEventArgs(string message)
        {
            Message = message;
        }


    }
}

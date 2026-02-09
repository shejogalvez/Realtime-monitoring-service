namespace app.IOAbstractions
{
    public interface ILogger
    {
        public void WriteLine(string input);
        public void WriteLine(object input);
    }
}
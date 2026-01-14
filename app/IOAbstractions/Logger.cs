namespace app.IOAbstractions
{
    public class Logger: ILogger
    {
        public void WriteLine(string input)
        {
            Console.WriteLine(input);
        }
        public void WriteLine(object input)
        {
            Console.WriteLine(input);
        }
    }
}
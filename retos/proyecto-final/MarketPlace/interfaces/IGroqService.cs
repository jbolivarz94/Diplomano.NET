public interface IGroqService
{
    Task<string> AskAsync(string prompt);
}
namespace IocTest.Core
{
    public interface IDisplayService
    {
        void SetColor(string color);
        void Write(string message);
        void ResetColor();
    }
}
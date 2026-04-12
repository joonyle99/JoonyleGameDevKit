namespace JoonyleGameDevKit
{
    public interface IManager
    {
        public int Priority { get; }
        public void Initialize();
    }
}

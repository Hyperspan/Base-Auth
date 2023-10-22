namespace Hyperspan.Base.Shared.Config
{
    public class BaseEntity<T> : IBaseEntity<T>
    {
        public T Id { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now.ToUniversalTime();
    }

    public interface IBaseEntity<T>
    {
        public T Id { get; set; }
    }
}

namespace Base.Shared.Config
{
    public class BaseEntity<T> : IBaseEntity<T>
    {
        public T Id { get; set; }
        public DateTime CreatedTime { get; set; }
    }

    public interface IBaseEntity<T>
    {
        public T Id { get; set; }
    }
}

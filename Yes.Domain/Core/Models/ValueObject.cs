namespace Yes.Domain.Core.Models
{
    public class ValueObject<T> : IEquatable<ValueObject<T>>
    {
        // 私有构造函数，防止外部直接实例化
        private ValueObject(int id, T entity)
        {
            Id = id;
            Value = entity;
        }

        // 公共静态工厂方法，用于创建值对象
        public static ValueObject<T> Create(int id, T entity)
        {
            return new ValueObject<T>(id, entity);
        }

        // 只读属性
        public int Id { get; }
        public T Value { get; }

        // 重写 Equals 方法
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((ValueObject<T>)obj);
        }

        // 实现 IEquatable<T> 接口的 Equals 方法
        public bool Equals(ValueObject<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id && EqualityComparer<T>.Default.Equals(Value, other.Value);
        }

        // 重写 GetHashCode 方法
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Id;
                hashCode = hashCode * 397 ^ EqualityComparer<T>.Default.GetHashCode(Value);
                return hashCode;
            }
        }

        // 重写 ToString 方法
        public override string ToString()
        {
            return $"ValueObject: Id={Id}, Value={Value}";
        }
    }
}

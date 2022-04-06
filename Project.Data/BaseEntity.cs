using System;

namespace Project.Data
{
    public abstract partial class BaseEntity<T>
    {
        public T Id { get; set; }
    }
}

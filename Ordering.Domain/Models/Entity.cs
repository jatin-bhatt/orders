using MediatR;

namespace Ordering.Domain.Models {
    public abstract class Entity<T> {
        
        T _Id;
        public virtual T Id {
            get {
                return _Id;
            }
            protected set {
                _Id = value;
            }
        }

        private List<INotification> _domainEvents;
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly();

        public void AddDomainEvent(INotification eventItem) {
            _domainEvents = _domainEvents ?? new List<INotification>();
            _domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(INotification eventItem) {
            _domainEvents?.Remove(eventItem);
        }

        public void ClearDomainEvents() {
            _domainEvents?.Clear();
        }

        public override bool Equals(object obj) {
            var other = obj as Entity<T>;
            return other != null && other.Id.Equals(Id);
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }

        public static bool operator ==(Entity<T> left, Entity<T> right) {
            if (Object.Equals(left, null))
                return (Object.Equals(right, null)) ? true : false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(Entity<T> left, Entity<T> right) {
            return !(left == right);
        }
    }
}

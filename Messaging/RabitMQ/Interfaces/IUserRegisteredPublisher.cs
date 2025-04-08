using SharedKernel.Contracts; 

namespace Messaging.RabitMQ.Interfaces
{
    public interface IUserRegisteredPublisher
    {
        Task PublishUserRegisteredEventAsync(UserRegisteredEvent userRegisteredEvent);
    }

}

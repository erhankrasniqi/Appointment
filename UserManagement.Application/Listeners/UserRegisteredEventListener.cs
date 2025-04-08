using MediatR;
using Messaging.RabitMQ.Interfaces;
using SharedKernel.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UserManagement.Application.Commands;

namespace UserManagement.Application.Listeners
{
    public class UserRegisteredEventListener
    {
        private readonly IMessageSubscriber _messageSubscriber;
        private readonly IMediator _mediator;

        public UserRegisteredEventListener(IMessageSubscriber messageSubscriber, IMediator mediator)
        {
            _messageSubscriber = messageSubscriber;
            _mediator = mediator;
        }

        public void StartListeningForUserRegisteredEvent()
        {
            // Dëgjo për mesazhe në RabbitMQ dhe kur merr mesazhin, thirri metodën që do të përpunojë
            _messageSubscriber.SubscribeToQueue("user_registration_queue", HandleUserRegisteredEvent);
        }

        private async void HandleUserRegisteredEvent(string message)
        {
            var userRegisteredEvent = JsonSerializer.Deserialize<UserRegisteredEvent>(message);

            var addUserCommand = new AddUserCommand
            {
                AuthId = userRegisteredEvent.UserId,
                Name = "Erhan",
                SurnameName = "Krasniqi",
                // Mund të shtoni të dhënat e tjera për regjistrimin e përdoruesit
            };

            // Dërgo komandën për regjistrimin e përdoruesit
            await _mediator.Send(addUserCommand);
        }
    }
}
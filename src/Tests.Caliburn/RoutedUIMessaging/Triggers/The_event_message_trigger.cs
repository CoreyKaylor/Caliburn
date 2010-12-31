namespace Tests.Caliburn.RoutedUIMessaging.Triggers
{
    using System;
    using Fakes;
    using Fakes.UI;
    using global::Caliburn.PresentationFramework.RoutedMessaging;
    using global::Caliburn.PresentationFramework.RoutedMessaging.Triggers;
    using NUnit.Framework;
    using Rhino.Mocks;

    [TestFixture]
    public class The_event_message_trigger : TestBase
    {
        IInteractionNode node;
        FakeElement element;
        FakeMessage message;

        protected override void given_the_context_of()
        {
            node = Mock<IInteractionNode>();

            element = new FakeElement();
            message = new FakeMessage {AvailabilityEffect = Mock<IAvailabilityEffect>()};
        }

        [Test]
        public void can_attach_itself_to_an_element()
        {
            var trigger = new EventMessageTrigger
            {
                Message = message,
                EventName = FakeElement.EventName
            };

            node.Expect(x => x.UIElement).Return(element).Repeat.Any();

            trigger.Attach(node);

            Assert.That(trigger.Node, Is.EqualTo(node));
            Assert.That(message.InvalidatedHandler, Is.Not.Null);
            Assert.That(message.InitializeCalledWith, Is.EqualTo(node));
        }

        [Test]
        public void can_trigger_message_processing()
        {
            var trigger = new EventMessageTrigger
            {
                Message = message,
                EventName = FakeElement.EventName
            };

            node.Expect(x => x.UIElement).Return(element).Repeat.Any();
            node.Expect(x => x.ProcessMessage(Arg<IRoutedMessage>.Is.Equal(message), Arg<EventArgs>.Is.TypeOf));


            trigger.Attach(node);
            element.RaiseClick();
        }

        [Test]
        public void can_update_availability()
        {
            var trigger = new EventMessageTrigger
            {
                Message = message,
                EventName = FakeElement.EventName
            };

            node.Expect(x => x.UIElement).Return(element);

            node.Expect(x => x.UIElement).Return(element);
            message.AvailabilityEffect.Expect(x => x.ApplyTo(element, false));


            trigger.Attach(node);
            trigger.UpdateAvailabilty(false);
        }
    }
}
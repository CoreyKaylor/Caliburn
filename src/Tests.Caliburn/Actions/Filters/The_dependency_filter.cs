﻿namespace Tests.Caliburn.Actions.Filters
{
    using System.Collections.Generic;
    using global::Caliburn.Core;
    using global::Caliburn.PresentationFramework;
    using global::Caliburn.PresentationFramework.Filters;
    using global::Caliburn.PresentationFramework.RoutedMessaging;
    using Microsoft.Practices.ServiceLocation;
    using NUnit.Framework;
    using NUnit.Framework.SyntaxHelpers;
    using Rhino.Mocks;

    [TestFixture]
    public class The_dependency_filter : TestBase
    {
        private IServiceLocator _container;
        private TheMethodHost _methodHost;
        private IRoutedMessageHandler _handler;
        private IMessageTrigger _trigger;
        private DependenciesAttribute _attribute;

        protected override void given_the_context_of()
        {
            _container = Mock<IServiceLocator>();
            _methodHost = new TheMethodHost();
            _handler = Mock<IRoutedMessageHandler>();
            _handler.Stub(x => x.Unwrap()).Return(_methodHost);
            _trigger = Stub<IMessageTrigger>();
            _trigger.Message = Stub<IRoutedMessage>();
        }

        [Test]
        public void can_initialize_DependencyObserver()
        {
            var metadata = new List<object>();

            _attribute = new DependenciesAttribute("AProperty", "A.Property.Path");
            _handler.Stub(x => x.Metadata).Return(metadata).Repeat.Any();
            _attribute.Initialize(typeof(TheMethodHost), typeof(TheMethodHost), _container);
            _handler.Stub(x => x.Unwrap()).Return(_methodHost).Repeat.Any();
            _attribute.MakeAwareOf(_handler);

            Assert.That(metadata.FirstOrDefaultOfType<DependencyObserver>(), Is.Not.Null);

            _attribute.MakeAwareOf(_handler, _trigger);
            //TODO: assert DependencyObserver.MakeAwareOf(IMessageTrigger trigger, IEnumerable<string> dependencies)
        }

        private class TheMethodHost : PropertyChangedBase
        {
        }
    }
}
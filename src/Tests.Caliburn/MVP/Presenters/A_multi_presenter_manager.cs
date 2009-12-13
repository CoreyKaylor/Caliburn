﻿namespace Tests.Caliburn.MVP.Presenters
{
    using Fakes;
    using global::Caliburn.PresentationFramework.ApplicationModel;
    using global::Caliburn.Testability.Extensions;
    using NUnit.Framework;
    using NUnit.Framework.SyntaxHelpers;
    using Rhino.Mocks;

    [TestFixture]
    public class A_multi_presenter_manager : A_presenter
    {
        protected MultiPresenterManager _presenterManager;
        private IPresenter _currentPresenter;

        protected override PresenterBase CreatePresenter()
        {
            return new MultiPresenterManager();
        }

        protected override void given_the_context_of()
        {
            base.given_the_context_of();

            _presenterManager = (MultiPresenterManager)_presenter;
            _currentPresenter = Mock<IPresenter>();
        }

        [Test]
        public void can_shutdown_if_current_presenter_is_null()
        {
            Assert.That(_presenterManager.CanShutdown());
        }

        [Test]
        public void asks_current_presenter_if_can_shutdown()
        {
            _presenterManager.CurrentPresenter = _currentPresenter;

            _currentPresenter.Expect(x => x.CanShutdown())
                .Return(false);

            Assert.That(_presenterManager.CanShutdown(), Is.False);
        }

        [Test]
        public void initializes_current_presenter_during_its_initialization()
        {
            _presenterManager.CurrentPresenter = _currentPresenter;

            _presenterManager.Initialize();

            _currentPresenter.AssertWasCalled(x => x.Initialize());
        }

        [Test]
        public void shuts_down_current_presenter_during_its_shutdown()
        {
            _presenterManager.CurrentPresenter = _currentPresenter;

            _presenterManager.Shutdown();

            _currentPresenter.AssertWasCalled(x => x.Shutdown());
        }

        [Test]
        public void activates_current_presenter_during_its_activation()
        {
            _presenterManager.CurrentPresenter = _currentPresenter;

            _presenterManager.Activate();

            _currentPresenter.AssertWasCalled(x => x.Activate());
        }

        [Test]
        public void deactivates_current_presenter_during_its_deactivation()
        {
            _presenterManager.CurrentPresenter = _currentPresenter;

            _presenterManager.Activate();
            _presenterManager.Deactivate();

            _currentPresenter.AssertWasCalled(x => x.Deactivate());
        }

        [Test]
        public void can_shutdown_current_if_current_is_null()
        {
            bool wasShutdown = false;

            _presenterManager.ShutdownCurrent(isSuccess => wasShutdown = isSuccess);

            Assert.That(wasShutdown);
        }

        [Test]
        public void cannot_shutdown_current_if_current_does_not_allow()
        {
            _presenterManager.CurrentPresenter = _currentPresenter;

            _currentPresenter.Expect(x => x.CanShutdown())
                .Return(false);

            bool wasShutdown = false;

            _presenterManager.ShutdownCurrent(isSuccess => wasShutdown = isSuccess);

            Assert.That(wasShutdown, Is.False);
        }

        [Test]
        public void can_shutdown_current_if_current_allows()
        {
            _presenterManager.CurrentPresenter = _currentPresenter;

            _currentPresenter.Expect(x => x.CanShutdown())
                .Return(true);

            _presenterManager.AssertThatChangeNotificationIsRaisedBy(x => x.CurrentPresenter)
                .When(() =>{
                    bool wasShutdown = false;

                    _presenterManager.ShutdownCurrent(isSuccess => wasShutdown = isSuccess);

                    Assert.That(wasShutdown);
                });

            _currentPresenter.AssertWasCalled(x => x.Deactivate());
            _currentPresenter.AssertWasCalled(x => x.Shutdown());
        }

        [Test]
        public void can_execute_custom_shutdown_on_shutdown_current()
        {
            var presenter = new FakePresenter
            {
                CanShutdownResult = false,
                CustomCanShutdownResult = true
            };

            _presenterManager.CurrentPresenter = presenter;

            bool canShutdown = false;

            _presenterManager.ShutdownCurrent(isSuccess => canShutdown = isSuccess);

            Assert.That(presenter.CanShutdownWasCalled);
            Assert.That(canShutdown);
        }

        [Test]
        public void can_stop_custom_shutdown_on_shutdown_current()
        {
            var presenter = new FakePresenter
            {
                CanShutdownResult = false,
                CustomCanShutdownResult = false
            };

            _presenterManager.CurrentPresenter = presenter;

            bool canShutdown = false;

            _presenterManager.ShutdownCurrent(isSuccess => canShutdown = isSuccess);

            Assert.That(presenter.CanShutdownWasCalled);
            Assert.That(canShutdown, Is.False);
        }

        [Test]
        public void can_open_a_presenter()
        {
            _presenterManager.AssertThatChangeNotificationIsRaisedBy(x => x.CurrentPresenter)
                .When(() =>{
                    bool wasOpened = false;
                    _presenterManager.Open(_currentPresenter, isSuccess => wasOpened = isSuccess);
                    Assert.That(wasOpened);
                });

            _currentPresenter.AssertWasCalled(x => x.Initialize());
            _currentPresenter.AssertWasCalled(x => x.Activate());

            Assert.That(_presenterManager.Presenters, Has.Member(_currentPresenter));
        }

        [Test]
        public void opens_a_presenter_when_current_is_set()
        {
            _presenterManager.AssertThatChangeNotificationIsRaisedBy(x => x.CurrentPresenter)
                .When(() => _presenterManager.CurrentPresenter = _currentPresenter);

            _currentPresenter.AssertWasCalled(x => x.Initialize());
            _currentPresenter.AssertWasCalled(x => x.Activate());

            Assert.That(_presenterManager.Presenters, Has.Member(_currentPresenter));
        }

        [Test]
        public void deactivates_previous_when_opening_a_new_presenter()
        {
            _presenterManager.CurrentPresenter = _currentPresenter;

            bool wasOpened = false;

            _presenterManager.Open(Mock<IPresenter>(), isSuccess => wasOpened = isSuccess);

            Assert.That(wasOpened);

            _currentPresenter.AssertWasCalled(x => x.Deactivate());
            _currentPresenter.AssertWasNotCalled(x => x.Shutdown());
        }
    }
}
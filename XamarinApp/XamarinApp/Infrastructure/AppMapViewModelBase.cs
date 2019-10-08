using System;
using Prism;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;

namespace XamarinApp.Infrastructure {

    public abstract class AppMapViewModelBase : BindableBase, IInitialize, INavigationAware, IConfirmNavigation, IDestructible
    {

		const string RootUriPrependText = "/";
        DelegateCommand<string> _navigateAbsoluteCommand;
        DelegateCommand<string> _navigateCommand;
        DelegateCommand<string> _navigateModalCommand;
        DelegateCommand<string> _navigateNonModalCommand;

        /// <summary>
        /// Gets the navigate absolute command.
        /// </summary>
        /// <value>The navigate absolute command.</value>
        public DelegateCommand<string> NavigateAbsoluteCommand => _navigateAbsoluteCommand ?? (_navigateAbsoluteCommand = new DelegateCommand<string>(NavigateAbsoluteCommandExecute, CanNavigateAbsoluteCommandExecute));

        /// <summary>
        /// Gets the navigate command.
        /// </summary>
        /// <value>The navigate command.</value>
        public DelegateCommand<string> NavigateCommand => _navigateCommand ?? (_navigateCommand = new DelegateCommand<string>(NavigateCommandExecute, CanNavigateCommandExecute));

        /// <summary>
        /// Gets the navigate modal command.
        /// </summary>
        /// <value>The navigate modal command.</value>
        public DelegateCommand<string> NavigateModalCommand => _navigateModalCommand ?? (_navigateModalCommand = new DelegateCommand<string>(NavigateModalCommandExecute, CanNavigateModalCommandExecute));

        /// <summary>
        /// Gets the navigate non modal command.
        /// </summary>
        /// <value>The navigate non modal command.</value>
        public DelegateCommand<string> NavigateNonModalCommand => _navigateNonModalCommand ?? (_navigateNonModalCommand = new DelegateCommand<string>(NavigateNonModalCommandExecute, CanNavigateNonModalCommandExecute));

        /// <summary>
        /// Gets the navigation service.
        /// </summary>
        /// <value>The navigation service.</value>
        protected INavigationService NavigationService { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppMapViewModelBase"/> class.
        /// </summary>
        /// <param name="navigationService">The navigation service.</param>
        /// <exception cref="System.ArgumentNullException">navigationService</exception>
        protected AppMapViewModelBase(INavigationService navigationService)
        {
            if (navigationService == null)
            {
                throw new ArgumentNullException(nameof(navigationService));
            }
            this.NavigationService = navigationService;
        }

        /// <summary>
        /// Determines whether this instance accepts being navigated away from.  This method is invoked by Prism before a navigation operation and is a member of IConfirmNavigation.
        /// </summary>
        /// <param name="parameters">The navigation parameters.</param>
        /// <returns><c>True</c> if navigation can continue, <c>False</c> if navigation is not allowed to continue</returns>
        public virtual bool CanNavigate(INavigationParameters parameters)
        {
            return true;
        }

        /// <summary>
        /// Determines whether this instance can execute the NavigateAbsoluteCommand.
        /// </summary>
        /// <param name="uri">The uri.</param>
        /// <returns><c>true</c> if this instance can execute NavigateAbsoluteCommand; otherwise, <c>false</c>.</returns>
        protected virtual bool CanNavigateAbsoluteCommandExecute(string uri)
        {
            return !String.IsNullOrEmpty(uri);
        }

        /// <summary>
        /// Determines whether this instance can execute the NavigateAbsoluteCommand.
        /// </summary>
        /// <param name="uri">The uri.</param>
        /// <returns><c>true</c> if this instance can execute NavigateAbsoluteCommand; otherwise, <c>false</c>.</returns>
        protected virtual bool CanNavigateCommandExecute(string uri)
        {
            return !String.IsNullOrEmpty(uri);
        }

        /// <summary>
        /// Determines whether this instance can execute the NavigateModalCommand.
        /// </summary>
        /// <param name="uri">The uri.</param>
        /// <returns><c>true</c> if this instance can execute NavigateModalCommand; otherwise, <c>false</c>.</returns>
        protected virtual bool CanNavigateModalCommandExecute(string uri)
        {
            return !String.IsNullOrEmpty(uri);
        }

        /// <summary>
        /// Determines whether this instance can execute the NavigateNonModalCommand.
        /// </summary>
        /// <param name="uri">The uri.</param>
        /// <returns><c>true</c> if this instance can execute NavigateNonModalCommand; otherwise, <c>false</c>.</returns>
        protected virtual bool CanNavigateNonModalCommandExecute(string uri)
        {
            return !String.IsNullOrEmpty(uri);
        }

        /// <summary>
        /// <p>Invoked by Prism Navigation when the instance is removed from the navigation stack.</p>
        /// <p>Deriving class can override and perform any required clean up.</p>
        /// </summary>
        public virtual void Destroy()
        {
        }

        /// <summary>
        /// Navigates to the uri after creating a new navigation root. (Effectively replacing the Application MainPage.)
        /// </summary>
        /// <param name="uri">The uri text.</param>
        /// <returns>Task.</returns>
        protected virtual async void NavigateAbsoluteCommandExecute(string uri)
        {
            if (!CanNavigateAbsoluteCommandExecute(uri))
            {
                return;
            }
            if (!uri.StartsWith(RootUriPrependText))
            {
                uri = string.Concat(RootUriPrependText, uri);
            }
            await this.NavigationService.NavigateAsync(uri, null, false);
        }

        /// <summary>
        /// Navigates to the uri.
        /// </summary>
        /// <param name="uri">The uri text.</param>
        /// <returns>Task.</returns>
        protected virtual async void NavigateCommandExecute(string uri)
        {
            if (!CanNavigateCommandExecute(uri))
            {
                return;
            }
            await this.NavigationService.NavigateAsync(uri);
        }

        /// <summary>
        /// Navigates to the uri using a Modal navigation.
        /// </summary>
        /// <param name="uri">The uri text.</param>
        /// <returns>Task.</returns>
        protected virtual async void NavigateModalCommandExecute(string uri)
        {
            if (!CanNavigateModalCommandExecute(uri))
            {
                return;
            }
            await this.NavigationService.NavigateAsync(uri, useModalNavigation: true);
        }

        /// <summary>
        /// Navigates to the uri using Non-Modal navigation.
        /// </summary>
        /// <param name="uri">The uri text.</param>
        /// <returns>Task.</returns>
        protected virtual async void NavigateNonModalCommandExecute(string uri)
        {
            if (!CanNavigateNonModalCommandExecute(uri))
            {
                return;
            }
            await this.NavigationService.NavigateAsync(uri, useModalNavigation: false);
        }

		/// <summary>
        /// Invoked by Prism immediately after the ViewModel has been created.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public virtual void Initialize(INavigationParameters parameters)
        {
        }

        /// <summary>
        /// Invoked by Prism after navigating away from viewmodel's page.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        /// <summary>
        /// Invoked by Prism after navigating to the viewmodel's page.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
        }
    }
}

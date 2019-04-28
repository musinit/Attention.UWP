﻿using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Threading;
using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using Windows.UI;
using HENG.Services;
using System.Threading.Tasks;
using HENG.Helpers;

namespace HENG
{
    sealed partial class App
    {
        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            void CustomTitleBar()
            {
                CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;

                var titleBar = ApplicationView.GetForCurrentView().TitleBar;
                titleBar.ButtonBackgroundColor = Colors.Transparent;
                titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            }
            CustomTitleBar();
            await InitializeAsync();

            if (e.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                bool loadState = (e.PreviousExecutionState == ApplicationExecutionState.Terminated);
                ExtendedSplash extendedSplash = new ExtendedSplash(e.SplashScreen, loadState);
                Window.Current.Content = extendedSplash;
            }

            Window.Current.Activate();
            DispatcherHelper.Initialize();

            await DispatcherHelper.UIDispatcher.RunIdleAsync(async s =>
            {
                await BackgroundDownloadHelper.AttachToDownloadsAsync();
            });
        }

        private async Task InitializeAsync()
        {
            await ThemeSelectorService.InitializeAsync();
        }

        public static async Task StartupAsync()
        {
            await ThemeSelectorService.SetRequestedThemeAsync();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}

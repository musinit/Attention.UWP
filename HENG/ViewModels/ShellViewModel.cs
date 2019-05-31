﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using HENG.Helpers;
using HENG.Views;
using System;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using muxc = Microsoft.UI.Xaml.Controls;

namespace HENG.ViewModels
{
    public class ShellViewModel:ViewModelBase
    {
        private readonly NavigationService _navService;
        private muxc.NavigationView _navView;
        public ShellViewModel(NavigationService navigationService)
        {
            _navService = navigationService;
        }

        public void Initialize(muxc.NavigationView navView, Frame shellFrame)
        {
            _navView = navView;
            _navView.BackRequested += (sender, args) => { _navService.GoBack(); };

            _navService.CurrentFrame = shellFrame;
            _navService.CurrentFrame.Navigated += Frame_Navigated;
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            IsBackEnabled = _navService.CanGoBack;

            if (e.SourcePageType == typeof(SettingsView))
            {
                Selected = _navView.SettingsItem;
            }
            else
            {
                var item = _navView.MenuItems.OfType<muxc.NavigationViewItem>().FirstOrDefault(p => IsMenuItemForPageType(p, e.SourcePageType));
                Selected = item;
            }

            bool IsMenuItemForPageType(muxc.NavigationViewItem menuItem, Type sourcePageType)
            {
                var pageKey = menuItem.GetValue(NavHelper.NavigateToProperty) as string;
                var navigaedPageKey = _navService.GetKeyForPage(sourcePageType);

                return pageKey == navigaedPageKey;
            }
        }

        private bool _isBackEnabled;
        public bool IsBackEnabled
        {
            get { return _isBackEnabled; }
            set { Set(ref _isBackEnabled, value); }
        }

        private object _selected;
        public object Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        private ICommand _loadedCommand;
        public ICommand LoadedCommand
        {
            get
            {
                if (_loadedCommand == null)
                {
                    _loadedCommand = new RelayCommand(() =>
                    {
                        var first = _navView.MenuItems.OfType<muxc.NavigationViewItem>().FirstOrDefault();
                        if (first != null)
                        {
                            var pageKey = NavHelper.GetNavigateTo(first);
                            _navService.NavigateTo(pageKey);
                        }
                    });
                }
                return _loadedCommand;
            }
        }

        private ICommand _itemInvokedCommand;
        public ICommand ItemInvokedCommand
        {
            get
            {
                if (_itemInvokedCommand == null)
                {
                    _itemInvokedCommand = new RelayCommand<muxc.NavigationViewItemInvokedEventArgs>(args =>
                    {
                        if (args.IsSettingsInvoked)
                        {
                            var has = _navService.CurrentPageKey == typeof(SettingsViewModel).FullName;
                            if (!has)
                            {
                                _navService.NavigateTo(typeof(SettingsViewModel).FullName);
                            }
                            return;
                        }

                        var item = _navView.MenuItems.OfType<muxc.NavigationViewItem>()
                        .FirstOrDefault(menuItem => (string)menuItem.Content == args.InvokedItem as string);
                        if (item != null)
                        {
                            var pageKey = item.GetValue(NavHelper.NavigateToProperty) as string;
                            var has = _navService.GetKeyForPage(_navService.CurrentFrame.CurrentSourcePageType) == pageKey;
                            if (!has)
                            {
                                _navService.NavigateTo(pageKey);
                            }
                        }
                    });
                }
                return _itemInvokedCommand;
            }
        }
    }
}

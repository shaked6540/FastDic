using FastDic.Models;
using FastDic.Themes;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FastDic
{
    public partial class App : Application
    {
        static WordDatabase wordDatabase;

        public static WordDatabase WordDatatbase
        {
            get
            {
                if (wordDatabase == null)
                {
                    wordDatabase = new WordDatabase();
                }
                return wordDatabase;
            }
        }

        public App()
        {
            RequestedThemeChanged += App_RequestedThemeChanged;
            App_RequestedThemeChanged(null, new AppThemeChangedEventArgs(RequestedTheme));

            InitializeComponent();

            MainPage = new NavigationPage(new DictionaryView());
        }

        public App(string word)
        {
            RequestedThemeChanged += App_RequestedThemeChanged;
            App_RequestedThemeChanged(null, new AppThemeChangedEventArgs(RequestedTheme));
            InitializeComponent();
            MainPage = new NavigationPage(new DictionaryEntryPage(word, true));
        }

        private void App_RequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
        {
            var mergedDictionaries = Current.Resources.MergedDictionaries;
            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();

                switch (e.RequestedTheme)
                {

                    case OSAppTheme.Light:
                        mergedDictionaries.Add(new Light());
                        break;

                    case OSAppTheme.Dark:
                    default:
                        mergedDictionaries.Add(new Dark());
                        break;
                }
            }
        }

        public delegate void FinishActivity();
        public static event FinishActivity OnFinishActivity;
        public static void RaiseFinishActivity()
        {
            OnFinishActivity?.Invoke();
        }
    }
}

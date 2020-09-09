using FastDic.Models;
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FastDic
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DictionaryView : ContentPage
    {
        private List<WordString> words;
        private const int INITAL_FETCH_SIZE = 100;
        public DictionaryView()
        {
            InitializeComponent();
            
            words = App.WordDatatbase.GetLimitedWordStrings(100).GetAwaiter().GetResult();
            dictionaryListView.ItemsSource = words;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // get everything from the db
            if (words.Count <= INITAL_FETCH_SIZE)
            {
                words = await App.WordDatatbase.GetDistinctWordsAsync();
                dictionaryListView.ItemsSource = words;

                if (!string.IsNullOrWhiteSpace(TextSearchBar.Text))
                {
                    SearchBar_TextChanged(null, new TextChangedEventArgs(TextSearchBar.Text, TextSearchBar.Text));
                }
            }
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                dictionaryListView.ScrollTo(words[0], ScrollToPosition.Start, false);
            }
            else
            {
                var index = words.BinarySearchBy(e.NewTextValue, (word) => word.Word, StringComparer.OrdinalIgnoreCase);
                if (index > 0)
                {
                    dictionaryListView.ScrollTo(words[index], ScrollToPosition.Start, false);
                }
                else
                {
                    index = words.BinarySearchBy(e.NewTextValue, (word) => word.Word.Substring(0, Math.Min(word.Word.Length, e.NewTextValue.Length)), StringComparer.OrdinalIgnoreCase);
                    if (index > 0)
                    {
                        dictionaryListView.ScrollTo(words[index], ScrollToPosition.Start, false);
                    }
                }
            }
        }

        private async void DictionaryListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
            {
                await Navigation.PushAsync(new DictionaryEntryPage((e.Item as WordString).Word, false), false);
            }
        }
    }
}
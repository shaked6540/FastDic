using FastDic.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FastDic
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DictionaryEntryPage : ContentPage
    {
        private readonly string word;
        private readonly bool fromOutside;
        private readonly WordDefinition wordDefinition;
        public DictionaryEntryPage(string word, bool fromOutside)
        {
            InitializeComponent();
            this.word = word;
            this.fromOutside = fromOutside;
            wordDefinition = App.WordDatatbase.GetWordAsync(word).GetAwaiter().GetResult().FirstOrDefault();
            MakeLabels();
            BindingContext = wordDefinition;

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext == null)
            {
                await DisplayAlert("Word not found", $"{word} was not found in the dictionary", "Back");
                App.RaiseFinishActivity();
            }
        }

        private async void BackButton_Click(object sender, EventArgs e)
        {
            if (fromOutside)
            {
                App.RaiseFinishActivity();
            }
            else
            {
                await Navigation.PopAsync(false);
            }
        }

        private void MakeLabels()
        {
            if (wordDefinition == null)
            {
                return;
            }

            var margin = new Thickness(25, 0, 0, 0);

            foreach (JObject sense in JArray.Parse(wordDefinition.Definitions))
            {
                if (sense.ContainsKey("d"))
                {
                    var frame = new StackLayout();
                    var allGlosses = new List<Label>();
                    var tagLabel = new Label();
                    double glossFontSize = 16;
                    double lexicalCategoryFontSize = 20;

                    foreach (JObject definitionObject in sense["d"])
                    {
                        if (definitionObject.ContainsKey("g"))
                        {
                            var glosses = definitionObject["g"].Select(x => x.Value<string>()).Where(x => !string.IsNullOrWhiteSpace(x) && x != "en");

                            foreach (var gloss in glosses)
                            {
                                var label = new Label
                                {
                                    Text = $"{allGlosses.Count + 1}. {gloss}",
                                    Margin = margin,
                                    FontSize = glossFontSize
                                };
                                allGlosses.Add(label);
                            }
                        }
                        if (definitionObject.ContainsKey("t"))
                        {
                            var tags = definitionObject["t"].Select(x => x.Value<string>());
                            tagLabel.Text = string.Join(", ", tags);
                        }
                    }

                    if (sense.ContainsKey("l"))
                    {
                        Label lexicalCategory = new Label();
                        FormattedString fs = new FormattedString();
                        var span = new Span() { Text = $"\n{sense["l"].Value<string>().UpperCaseFirstLetter()}", FontAttributes = FontAttributes.Bold, FontSize = lexicalCategoryFontSize };
                        span.SetDynamicResource(Span.TextColorProperty, "LexicalCategoryColor");
                        fs.Spans.Add(span);
                        if (!string.IsNullOrWhiteSpace(tagLabel.Text))
                        {
                            fs.Spans.Add(new Span() { Text = allGlosses.Count > 0 ? $" ({tagLabel.Text}): " : $" ({tagLabel.Text})", FontAttributes = FontAttributes.None, FontSize = glossFontSize });
                        }
                        else
                        {
                            fs.Spans.Add(new Span() { Text = ": ", FontAttributes = FontAttributes.None, FontSize = glossFontSize });
                        }
                        lexicalCategory.FormattedText = fs;
                        frame.Children.Add(lexicalCategory);
                    }
                    if (allGlosses.Count > 0)
                    {
                        foreach (var label in allGlosses)
                            frame.Children.Add(label);
                    }
                    ScrollViewStackLayout.Children.Add(frame);
                }
            }
        }
    }
}
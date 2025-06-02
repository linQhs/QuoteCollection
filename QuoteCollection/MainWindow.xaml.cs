using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace QuoteCollection
{
    public partial class MainWindow : Window
    {
        private List<Quote> quotes = new List<Quote>();
        private Quote selectedQuote = null;

        public MainWindow()
        {
            InitializeComponent();
            UpdateQuotesList();
        }

        public class Quote
        {
            public string Text { get; set; }
            public string Author { get; set; }
            public string Tags { get; set; }
            public bool Favorite { get; set; }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(QuoteTextBox.Text))
            {
                MessageBox.Show("Введите текст цитаты");
                return;
            }

            quotes.Add(new Quote
            {
                Text = QuoteTextBox.Text,
                Author = AuthorTextBox.Text,
                Tags = TagsTextBox.Text,
                Favorite = FavoriteCheckBox.IsChecked == true
            });

            ClearForm();
            UpdateQuotesList();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedQuote == null) return;

            selectedQuote.Text = QuoteTextBox.Text;
            selectedQuote.Author = AuthorTextBox.Text;
            selectedQuote.Tags = TagsTextBox.Text;
            selectedQuote.Favorite = FavoriteCheckBox.IsChecked == true;

            ClearForm();
            UpdateQuotesList();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedQuote == null) return;

            quotes.Remove(selectedQuote);
            ClearForm();
            UpdateQuotesList();
        }

        private void ClearForm()
        {
            QuoteTextBox.Text = "";
            AuthorTextBox.Text = "";
            TagsTextBox.Text = "";
            FavoriteCheckBox.IsChecked = false;
            selectedQuote = null;
            AddButton.IsEnabled = true;
            UpdateButton.IsEnabled = false;
            DeleteButton.IsEnabled = false;
        }

        private void UpdateQuotesList()
        {
            QuotesListBox.Items.Clear();

            string searchText = SearchTextBox.Text.ToLower(); 

            var filteredQuotes = quotes.Where(q =>
                q.Text.ToLower().Contains(searchText) ||
                q.Author.ToLower().Contains(searchText) ||
                q.Tags.ToLower().Contains(searchText));

            if (FavoritesOnlyCheckBox.IsChecked == true)
            {
                filteredQuotes = filteredQuotes.Where(q => q.Favorite);
            }

            foreach (var quote in filteredQuotes)
            {
                QuotesListBox.Items.Add($"{quote.Text} ({quote.Author})");
            }
        }

        private void QuotesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (QuotesListBox.SelectedIndex == -1) return;

            selectedQuote = quotes[QuotesListBox.SelectedIndex];
            QuoteTextBox.Text = selectedQuote.Text;
            AuthorTextBox.Text = selectedQuote.Author;
            TagsTextBox.Text = selectedQuote.Tags;
            FavoriteCheckBox.IsChecked = selectedQuote.Favorite;
            AddButton.IsEnabled = false;
            UpdateButton.IsEnabled = true;
            DeleteButton.IsEnabled = true;
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateQuotesList();
        }

        private void FavoritesOnlyCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            UpdateQuotesList();
        }
    }
}
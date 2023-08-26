using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Household_Budget
{
    /// <summary>
    /// Interaction logic for AllExpensesView.xaml
    /// </summary>
    public partial class AllExpensesView : UserControl
    {
        private string mode;

        public string Mode//Інтервал часу для відображення
        {
            set
            {
                if (value == "day" || value == "week" || value == "month")
                {
                    mode = value;
                }
                else
                    mode = null;

                getEvents();
            }
        }
        public AllExpensesView()
        {
            InitializeComponent();
            ViewType.SelectedIndex = 0;
            getEvents();
            
        }

       

        public static readonly DependencyProperty CurrentDateProperty =
            DependencyProperty.Register("CurrentDate", typeof(DateTime?), typeof(AllExpensesView),
                new PropertyMetadata(default(DateTime?), new PropertyChangedCallback(CurrentDateChanged)));

        public DateTime? CurrentDate
        {
            get { return (DateTime?)GetValue(CurrentDateProperty); }
            set { SetValue(CurrentDateProperty, value); }
        }

        private async static void CurrentDateChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            await ((AllExpensesView)sender).getEvents();
        }

        private async Task getEvents()//Отримання списку фінансових операцій залежно від обраних пунктів в головному вікні
        {
            DateTime start = DateTime.MinValue;
            DateTime end = DateTime.MaxValue;
            Previous.Visibility = Visibility.Hidden;
            Next.Visibility = Visibility.Hidden;

            switch (mode)
            {
                case "day":
                    start = ((DateTime)CurrentDate).Date;
                    end = start.AddDays(1);
                    Previous.Visibility = Visibility.Visible;
                    Next.Visibility = Visibility.Visible;
                    break;
                case "week":
                    start = ((DateTime)CurrentDate).Date;
                    start = start.AddDays(DayOfWeek.Monday - start.DayOfWeek);
                    end = start.AddDays(7);
                    Previous.Visibility = Visibility.Visible;
                    Next.Visibility = Visibility.Visible;
                    break;
                case "month":
                    start = ((DateTime)CurrentDate).Date;
                    start = new DateTime(start.Year, start.Month, 1);
                    end = start.AddMonths(1);
                    Previous.Visibility = Visibility.Visible;
                    Next.Visibility = Visibility.Visible;
                    break;
            }

            decimal total = 0;
            switch (ViewType.SelectedIndex)//Показ доходів/витрат/усіх операцій
            {

                case 0:
                    using (dbBudgetContainer db = new dbBudgetContainer())
                    {
                        var articles = await db.Article.Where(a => a.DateTime >= start && a.DateTime < end).
                            OrderBy(a => a.DateTime).ToListAsync();
                        ExpensesList.ItemsSource = articles;
                        total = (articles.OfType<Income>().Sum(i => i.Summ) - articles.OfType<Expenditure>().Sum(e => e.Summ) ?? 0);
                    }
                    break;
                case 1:
                    using (dbBudgetContainer db = new dbBudgetContainer())
                    {
                        var expenses = await db.Article.OfType<Expenditure>().Where(a => a.DateTime >= start && a.DateTime < end).
                             OrderBy(a => a.DateTime).ToListAsync();
                        ExpensesList.ItemsSource = expenses;
                        total = expenses.Sum(e => e.Summ) ?? 0;
                    }
                    break;
                case 2:
                    using (dbBudgetContainer db = new dbBudgetContainer ())
                    {
                        var income = await db.Article.OfType<Income>().Where(a => a.DateTime >= start && a.DateTime < end).
                            OrderBy(a => a.DateTime).ToListAsync();
                        ExpensesList.ItemsSource = income;
                        total = income.Sum(i => i.Summ);
                    }
                    break;
            }

            Total.Content = total.ToString("0.####");

            ExpensesList.Items.Refresh();
            OnCalendarClick();
        }

        private void OnCalendarClick()
        {
            if (ExpensesList.DataContext != null)
            {
                List<Article> articles = (List<Article>)ExpensesList.DataContext;
                Article selected = articles.Where(a => a.DateTime >= (DateTime)MainWindow.MainView.ExpensesCurrentDate.SelectedDate).FirstOrDefault();
                if (selected != null)
                    ExpensesList.SelectedItem = selected;
            }
        }

        private async void ExensesList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Article article = (Article)ExpensesList.SelectedItem;
            RecordWindow edit = article.GetEditWindow();
            if (edit.ShowDialog() == true)
                await getEvents();
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Ви впевнені, що бажаєте видалити запис?", "Вы впевнені?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Article article = (Article)ExpensesList.SelectedItem;
                using (dbBudgetContainer db = new dbBudgetContainer())
                {
                    db.Article.Attach(article);
                    db.Article.Remove(article);

                    db.SaveChanges();
                }

                await getEvents();
            }
        }

        //Перехід вперед/назад
        private async void Previous_Click(object sender, RoutedEventArgs e)
        {
            switch (mode)
            {
                case "day":
                    CurrentDate = ((DateTime)CurrentDate).AddDays(-1);
                    break;
                case "week":
                    CurrentDate = ((DateTime)CurrentDate).AddDays(-7);
                    break;
                case "month":
                    CurrentDate = ((DateTime)CurrentDate).AddMonths(-1);
                    break;
            }

            await getEvents();
        }

        private async void Next_Click(object sender, RoutedEventArgs e)
        {
            switch (mode)
            {
                case "day":
                    CurrentDate = ((DateTime)CurrentDate).AddDays(1);
                    break;
                case "week":
                    CurrentDate = ((DateTime)CurrentDate).AddDays(7);
                    break;
                case "month":
                    CurrentDate = ((DateTime)CurrentDate).AddMonths(1);
                    break;
            }

            await getEvents();
        }

        private async void ViewType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await getEvents();
        }
    }
}

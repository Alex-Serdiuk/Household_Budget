using System;
using System.Collections.Generic;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow MainView; //Статичне посилання на головне вікно

        private AllExpensesView expensesView;
        public MainWindow()
        {
            MainView = this;
            InitializeComponent();

            ExpensesViewModePicker.SelectedIndex = 0;

           
        }

        

        private void ExpensesViewModePicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            showExpenses();
        }

        //Виклик форми для відображення витрат/доходів
        private void showExpenses()
        {
            MainExpensesPanel.Children.Remove(expensesView);

            Binding b = new Binding("SelectedDate");
            b.Source = ExpensesCurrentDate;
            b.Mode = BindingMode.TwoWay;

            expensesView = new AllExpensesView();
            expensesView.SetBinding(AllExpensesView.CurrentDateProperty, b);

            switch (ExpensesViewModePicker.SelectedIndex)
            {
                case 0:
                    break;
                case 1:
                    expensesView.Mode = "day";
                    break;
                case 2:
                    expensesView.Mode = "week";
                    break;
                case 3:
                    expensesView.Mode = "month";
                    break;
            }

            Grid.SetRow(expensesView, 1);
            Grid.SetColumnSpan(expensesView, 4);
            MainExpensesPanel.Children.Add(expensesView);
        }

        private void NewIncome_Click(object sender, RoutedEventArgs e)
        {
            Income income = new Income() { DateTime = DateTime.Now };
            RecordWindow window = income.GetEditWindow();
            if (window.ShowDialog() == true)
                showExpenses();
        }

        private void NewExpenditure_Click(object sender, RoutedEventArgs e)
        {
            Expenditure exp = new Expenditure() { DateTime = DateTime.Now };
            RecordWindow window = exp.GetEditWindow();
            window.Height = 400;
            if (window.ShowDialog() == true)
                showExpenses();
        }

        private void ShowDiagram_Click(object sender, RoutedEventArgs e)
        {
            if (GraphType.Text == "Всі" || GraphType.Text == "Витрати")
            {
                RecordWindow window = new RecordWindow();
                window.Width = 600;
                window.Height = 350;
                window.Title = "Витрати";
                ExpensedDiagramControl diag = new ExpensedDiagramControl((DateTime)StartDate.SelectedDate, (DateTime)EndDate.SelectedDate);
                Grid.SetRow(diag, 0);
                window.Win.Children.Add(diag);
                window.Show();
            }

            if (GraphType.Text == "Всі" || GraphType.Text == "Доходи")
            {
                RecordWindow window = new RecordWindow();
                window.Width = 600;
                window.Height = 350;
                window.Title = "Доходи";
                IncomeDiagramControl diag = new IncomeDiagramControl((DateTime)StartDate.SelectedDate, (DateTime)EndDate.SelectedDate);
                Grid.SetRow(diag, 0);
                window.Win.Children.Add(diag);
                window.Show();
            }
        }

        private void ShowGraph_Click(object sender, RoutedEventArgs e)
        {
            RecordWindow window = new RecordWindow();
            window.Width = 800;
            window.Height = 400;
            window.Title = "График";
            ExpenseChartControl chart = new ExpenseChartControl();
            Grid.SetRow(chart, 0);
            window.Win.Children.Add(chart);
            window.Show();

        }

        private void GraphDates_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                GraphMode.IsEnabled = true;
                ShowGraph.IsEnabled = true;
                ShowDiagram.IsEnabled = true;

                GraphMode.ItemsSource = null;

                TimeSpan graphRange = (TimeSpan)(EndDate.SelectedDate - StartDate.SelectedDate);

                if (graphRange < TimeSpan.Zero)
                {
                    StartDate.SelectedDate = EndDate.SelectedDate;
                    return;
                }

                if (graphRange > TimeSpan.FromDays(2))
                {
                    GraphMode.IsEnabled = true;
                    List<string> modes = new List<string>();

                    modes.Add("По дням");

                    if (graphRange > TimeSpan.FromDays(7))
                    {
                        modes.Add("По тижням");
                    }

                    if (graphRange > TimeSpan.FromDays(62))
                    {
                        modes.Add("По месяцам");
                    }

                    if (graphRange > TimeSpan.FromDays(731))
                    {
                        modes.Add("По рокам");
                    }

                    GraphMode.ItemsSource = modes;
                    GraphMode.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}

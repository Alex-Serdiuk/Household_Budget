using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
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
    /// Interaction logic for ExpenseChartControl.xaml
    /// </summary>
    public partial class ExpenseChartControl : UserControl
    {
        public ExpenseChartControl()
        {
            InitializeComponent();
            string mode = MainWindow.MainView.GraphType.Text;

            DateTime startDate = (DateTime)MainWindow.MainView.StartDate.SelectedDate;
            DateTime endDate = (DateTime)MainWindow.MainView.EndDate.SelectedDate;
            string timeSpan = MainWindow.MainView.GraphMode.Text;


            DateTimeAxis dayAxis = new DateTimeAxis//Вісь часу для відображення у днях
            {
                Orientation = AxisOrientation.X,
                Location = AxisLocation.Bottom,
                AxisLabelStyle = Resources["DateAxisStyle"] as Style,
                Minimum = startDate.AddDays(-1),
                Maximum = endDate.AddDays(1),
                IntervalType = DateTimeIntervalType.Days,
                Interval = (endDate - startDate).Days / 10 + 1
            };

            DateTimeAxis weekAxis = new DateTimeAxis//Вісь часу для відображення в тижнях
            {
                Orientation = AxisOrientation.X,
                Location = AxisLocation.Bottom,
                AxisLabelStyle = Resources["DateAxisStyle"] as Style,
                Minimum = startDate.AddDays(-7),
                Maximum = endDate.AddDays(7),
                IntervalType = DateTimeIntervalType.Weeks,
                Interval = (endDate - startDate).Days / 70 + 1
            };

            DateTimeAxis monthAxis = new DateTimeAxis//Вісь часу для відображення у місяцях
            {
                Orientation = AxisOrientation.X,
                Location = AxisLocation.Bottom,
                AxisLabelStyle = Resources["DateAxisStyle"] as Style,
                Minimum = startDate.AddMonths(-1),
                Maximum = endDate.AddMonths(1),
                IntervalType = DateTimeIntervalType.Months,
                Interval = (endDate - startDate).Days / 300 + 1
            };

            DateTimeAxis yearAxis = new DateTimeAxis//Вісь часу для відображення у роках
            {
                Orientation = AxisOrientation.X,
                Location = AxisLocation.Bottom,
                AxisLabelStyle = Resources["DateAxisStyle"] as Style,
                Minimum = startDate.AddYears(-1),
                Maximum = endDate.AddYears(1),
                IntervalType = DateTimeIntervalType.Years,
                Interval = (endDate - startDate).Days / 3650 + 1
            };

            List<Article> articles = new List<Article>();

            //Отримання всіх фінансових операцій з бази даних
            using (dbBudgetContainer db = new dbBudgetContainer())
            {
                articles = db.Article.Where(a => a.DateTime >= startDate && a.DateTime < endDate).ToList();
            }

            //Вибір прибутків із усіх фінансових операцій та групування за обраним інтервалом
            if (mode == "Доходи" || mode == "Всі")
            {
                LineSeries incomeGraph = new LineSeries();
                incomeGraph.IndependentValuePath = "Date";
                incomeGraph.DependentValuePath = "Summ";
                incomeGraph.Title = "Доходи";


                switch (timeSpan)
                {
                    case "По дням":
                        var incomeByDays = articles.OfType<Income>().GroupBy(i => i.DateTime.Date).Select(ig => new
                        {
                            Date = ig.FirstOrDefault().DateTime,
                            Summ = ig.Sum(i => i.Summ)
                        });

                        incomeGraph.IndependentAxis = dayAxis;
                        incomeGraph.ItemsSource = incomeByDays;

                        break;
                    case "По тижням":
                        var incomeByWeeks = articles.OfType<Income>().GroupBy(i => new
                        {
                            Day = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(i.DateTime, CalendarWeekRule.FirstDay, DayOfWeek.Monday),
                            i.DateTime.Year
                        }).Select(ig => new
                        {
                            Date = ig.FirstOrDefault().DateTime,
                            Summ = ig.Sum(i => i.Summ)
                        });

                        incomeGraph.IndependentAxis = weekAxis;
                        incomeGraph.ItemsSource = incomeByWeeks;
                        break;
                    case "По місяцям":
                        var incomeByMonth = articles.OfType<Income>().GroupBy(i => new
                        {
                            i.DateTime.Month,
                            i.DateTime.Year
                        }).Select(ig => new
                        {
                            Date = ig.FirstOrDefault().DateTime,
                            Summ = ig.Sum(i => i.Summ)
                        });

                        incomeGraph.IndependentAxis = monthAxis;
                        incomeGraph.ItemsSource = incomeByMonth;
                        break;
                    case "По рокам":
                        var incomeByYears = articles.OfType<Income>().GroupBy(i => i.DateTime.Year).
                            Select(ig => new
                            {
                                Date = ig.FirstOrDefault().DateTime,
                                Summ = ig.Sum(i => i.Summ)
                            });

                        incomeGraph.IndependentAxis = yearAxis;
                        incomeGraph.ItemsSource = incomeByYears;
                        break;
                }


                GraphChart.Series.Add(incomeGraph);
            }

            //Вибір витрат із усіх фінансових операцій та групування за обраним інтервалом
            if (mode == "Витрати" || mode == "Всі")
            {
                LineSeries expensesGraph = new LineSeries();
                expensesGraph.IndependentValuePath = "Date";
                expensesGraph.DependentValuePath = "Summ";
                expensesGraph.Title = "Витрати";


                switch (timeSpan)
                {
                    case "По дням":
                        var expenseByDays = articles.OfType<Expenditure>().GroupBy(e => e.DateTime.Date).Select(eg => new
                        {
                            Date = eg.FirstOrDefault().DateTime,
                            Summ = eg.Sum(i => i.Summ)
                        });

                        expensesGraph.IndependentAxis = dayAxis;
                        expensesGraph.ItemsSource = expenseByDays;

                        break;
                    case "По тижням":
                        var expenseByWeeks = articles.OfType<Expenditure>().GroupBy(e => new
                        {
                            Day = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(e.DateTime, CalendarWeekRule.FirstDay, DayOfWeek.Monday),
                            e.DateTime.Year
                        }).Select(eg => new
                        {
                            Date = eg.FirstOrDefault().DateTime,
                            Summ = eg.Sum(e => e.Summ)
                        });

                        expensesGraph.IndependentAxis = weekAxis;
                        expensesGraph.ItemsSource = expenseByWeeks;
                        break;
                    case "По місяцам":
                        var expenseByMonth = articles.OfType<Expenditure>().GroupBy(e => new
                        {
                            e.DateTime.Month,
                            e.DateTime.Year
                        }).Select(eg => new
                        {
                            Date = eg.FirstOrDefault().DateTime,
                            Summ = eg.Sum(e => e.Summ)
                        });

                        expensesGraph.IndependentAxis = monthAxis;
                        expensesGraph.ItemsSource = expenseByMonth;
                        break;
                    case "По рокам":
                        var expenseByYears = articles.OfType<Income>().GroupBy(e => e.DateTime.Year).
                            Select(eg => new
                            {
                                Date = eg.FirstOrDefault().DateTime,
                                Summ = eg.Sum(e => e.Summ)
                            });

                        expensesGraph.IndependentAxis = yearAxis;
                        expensesGraph.ItemsSource = expenseByYears;
                        break;
                }



                GraphChart.Series.Add(expensesGraph);
            }

            //Підрахунок загального бюджету за обраним інтервалом
            if (mode == "Всі")
            {
                LineSeries budgetGraph = new LineSeries();
                budgetGraph.IndependentValuePath = "Key";
                budgetGraph.DependentValuePath = "Value";
                budgetGraph.Title = "Бюджет";


                //Підрахунок бюджету на початок обраного інтервалу часу
                decimal startBudget = 0;
                using (dbBudgetContainer db = new dbBudgetContainer())
                {
                    decimal previousIncome = db.Article.OfType<Income>().Where(i => i.DateTime < startDate).Sum(i => (decimal?)i.Summ) ?? 0;

                    decimal previousExpense = db.Article.OfType<Expenditure>().Where(e => e.DateTime < startDate).Sum(e => e.Summ) ?? 0;

                    startBudget += previousIncome - previousExpense;


                }

                List<KeyValuePair<DateTime, decimal>> budget = new List<KeyValuePair<DateTime, decimal>>();

                budget.Add(new KeyValuePair<DateTime, decimal>(startDate, startBudget));

                //Розрахунок бюджету за обраним інтервалом часу
                switch (timeSpan)
                {
                    case "По дням":
                        var budgetByDay = articles.GroupBy(a => a.DateTime.Date);
                        foreach (var ag in budgetByDay)
                        {
                            decimal income = 0;
                            decimal expense = 0;
                            foreach (var a in ag)
                            {
                                if (a is Income)
                                    income += ((Income)a).Summ;
                                else
                                    expense += ((Expenditure)a).Summ ?? 0;
                            }

                            startBudget += income - expense;

                            budget.Add(new KeyValuePair<DateTime, decimal>(ag.FirstOrDefault().DateTime, startBudget));
                        }

                        budgetGraph.IndependentAxis = dayAxis;
                        break;
                    case "По тижням":
                        var budgetByWeek = articles.GroupBy(a => new
                        {
                            Day = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(a.DateTime, CalendarWeekRule.FirstDay, DayOfWeek.Monday),
                            a.DateTime.Year
                        });

                        foreach (var ag in budgetByWeek)
                        {
                            decimal income = 0;
                            decimal expense = 0;
                            foreach (var a in ag)
                            {
                                if (a is Income)
                                    income += ((Income)a).Summ;
                                else
                                    expense += ((Expenditure)a).Summ ?? 0;
                            }

                            startBudget += income - expense;

                            budget.Add(new KeyValuePair<DateTime, decimal>(ag.FirstOrDefault().DateTime, startBudget));
                        }

                        budgetGraph.IndependentAxis = weekAxis;

                        break;
                    case "По місяцям":
                        var budgetByMonth = articles.GroupBy(a => new
                        {
                            a.DateTime.Month,
                            a.DateTime.Year
                        });

                        foreach (var ag in budgetByMonth)
                        {
                            decimal income = 0;
                            decimal expense = 0;
                            foreach (var a in ag)
                            {
                                if (a is Income)
                                    income += ((Income)a).Summ;
                                else
                                    expense += ((Expenditure)a).Summ ?? 0;
                            }

                            startBudget += income - expense;

                            budget.Add(new KeyValuePair<DateTime, decimal>(ag.FirstOrDefault().DateTime, startBudget));
                        }

                        budgetGraph.IndependentAxis = monthAxis;

                        break;
                    case "По рокам":
                        var budgetByYears = articles.GroupBy(a => a.DateTime.Year);

                        foreach (var ag in budgetByYears)
                        {
                            decimal income = 0;
                            decimal expense = 0;
                            foreach (var a in ag)
                            {
                                if (a is Income)
                                    income += ((Income)a).Summ;
                                else
                                    expense += ((Expenditure)a).Summ ?? 0;
                            }

                            startBudget += income - expense;

                            budget.Add(new KeyValuePair<DateTime, decimal>(ag.FirstOrDefault().DateTime, startBudget));
                        }

                        budgetGraph.IndependentAxis = yearAxis;

                        break;
                }

                budgetGraph.ItemsSource = budget;
                GraphChart.Series.Add(budgetGraph);
            }
        }
    }
}

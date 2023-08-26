using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Household_Budget
{
    /// <summary>
    /// Interaction logic for IncomeEditControl.xaml
    /// </summary>
    public partial class IncomeEditControl : UserControl
    {
        public IncomeEditControl()
        {
            InitializeComponent();
            using (dbBudgetContainer db = new dbBudgetContainer())
            {
                var incomeSources = db.IncomeSource.ToList();
                IncomeSourceSelector.ItemsSource = incomeSources;             
            }
        }

        //Шаблон для перевірки суми доходу
        private void IncomeSum_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex number = new Regex(@"^\d*[.]?\d{0,4}$");
            e.Handled = !number.IsMatch(e.Text);
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            Income income = (Income)DataContext;
            if (String.IsNullOrEmpty(IncomeSourceSelector.Text))
            {
                MessageBox.Show("Введіть або виберіть джерело доходу", "Увага", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                if (MessageBox.Show("Ви впевнені, що хочете зберегти запис?", "Вы впевнені?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Window.GetWindow(this).DialogResult = true;
                    Window.GetWindow(this).Close();
                    using (dbBudgetContainer db = new dbBudgetContainer())
                    {
                        if (!db.IncomeSource.Any(s => s.Name == IncomeSourceSelector.Text))
                        {
                            if (MessageBox.Show($"Ви бажаєте створити нове джерело доходу? \"{IncomeSourceSelector.Text}\"?", "Вы впевнені?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                IncomeSource newSource = new IncomeSource() { Name = IncomeSourceSelector.Text };
                                income.IncomeSource = newSource;
                            }
                            else return;
                        }
                        else
                        {
                            income.IncomeSource = (IncomeSource)IncomeSourceSelector.SelectedItem;
                        }

                        db.Entry(income).State = income.Id == 0 ?
                        System.Data.Entity.EntityState.Added :
                        System.Data.Entity.EntityState.Modified;

                        db.Entry(income.IncomeSource).State = income.IncomeSource.Id == 0 ?
                            System.Data.Entity.EntityState.Added :
                            System.Data.Entity.EntityState.Unchanged;

                        await db.SaveChangesAsync();
                    }
                }
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}

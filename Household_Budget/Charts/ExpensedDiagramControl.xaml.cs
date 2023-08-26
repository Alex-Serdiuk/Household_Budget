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
    /// Кругова діаграма витрат за статтями та назвами
    /// <summary>
    /// Interaction logic for ExpensedDiagramControl.xaml
    /// </summary>
    public partial class ExpensedDiagramControl : UserControl
    {
        public ExpensedDiagramControl(DateTime start, DateTime end)
        {
            InitializeComponent();

            using (dbBudgetContainer db = new dbBudgetContainer())
            {
                var expenses = db.Article.OfType<Expenditure>().
                    Where(e => e.DateTime >= start && e.DateTime < end).GroupBy(e => new { e.TypeId, e.NameId }).
                    Select(e => new {
                        FullName = e.FirstOrDefault().ExpenditureType.Type + ":" + e.FirstOrDefault().ExpenditureName.Name,
                        Money = e.Sum(ex => ex.Summ)
                    }).ToList();

                Expenses.ItemsSource = expenses;
            }
        }
    }
}

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
    ///Кругова діаграма доходів
    /// <summary>
    /// Interaction logic for IncomeDiagramControl.xaml
    /// </summary>
    public partial class IncomeDiagramControl : UserControl
    {
        public IncomeDiagramControl(DateTime start, DateTime end)
        {
            InitializeComponent();
            using (dbBudgetContainer db = new dbBudgetContainer())
            {
                var income = db.Article.OfType<Income>().Where(i => i.DateTime >= start && i.DateTime < end).
                    GroupBy(i => i.SourceId).Select(i => new { FullName = i.FirstOrDefault().IncomeSource.Name, Money = i.Sum(inc => inc.Summ) }).ToList();

                Income.ItemsSource = income;
            }
        }
    }
}

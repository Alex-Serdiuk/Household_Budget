using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Household_Budget
{
    public partial class Income
    {
        public override Control GetEditControl()
        {
            IncomeEditControl control = new IncomeEditControl();
            control.DataContext = this;
            return control;
        }

        public override decimal Money
        {
            get { return Summ; }
        }

        public override int WindowHeight { get { return 270; } }

        public override string FullName
        {
            get
            {
                if (Id > 0)
                {
                    using (dbBudgetContainer db = new dbBudgetContainer())
                    {
                        db.Article.Attach(this);

                        if (!db.Entry(this).Reference(i => i.IncomeSource).IsLoaded)
                        {
                            db.Entry(this).Reference(i => i.IncomeSource).Load();
                        }
                    }

                    return IncomeSource.Name;
                }
                else
                    return "New Income";
            }
        }

        public override string Type
        {
            get { return "Income"; }
        }
    }
}

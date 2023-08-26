using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Household_Budget
{
    public partial class Expenditure
    {
        public override Control GetEditControl()
        {
            ExpenditureEditControl edit = new ExpenditureEditControl();
            edit.DataContext = this;
            return edit;
        }
        

        public override decimal Money
        {
            get { return (decimal)Summ; }
        }

        public override int WindowHeight { get { return 380; } }

        public override string FullName
        {
            get
            {
                if (Id > 0)
                {
                    using (dbBudgetContainer db = new dbBudgetContainer())
                    {
                        if (db.Entry(this).State == System.Data.Entity.EntityState.Detached)
                            db.Article.Attach(this);

                        if (!db.Entry(this).Reference(e => e.ExpenditureType).IsLoaded)
                            db.Entry(this).Reference(e => e.ExpenditureType).Load();

                        if (!db.Entry(this).Reference(e => e.ExpenditureName).IsLoaded)
                            db.Entry(this).Reference(e => e.ExpenditureName).Load();
                    }

                    return $"{ExpenditureType.Type}: {ExpenditureName.Name}";
                }
                else
                    return "New Expenditure";
            }
        }

        public override string Type
        {
            get { return "Expenditure"; }
        }
    }
}

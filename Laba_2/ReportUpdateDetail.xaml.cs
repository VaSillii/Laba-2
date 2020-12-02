using System;
using System.Collections;
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
using System.Windows.Shapes;
using static Laba_2.Risk;

namespace Laba_2
{
    /// <summary>
    /// Логика взаимодействия для ReportUpdateDetail.xaml
    /// </summary>
    public partial class ReportUpdateDetail : Window
    {
        public ReportUpdateDetail(Risk actualRisk, Risk riskPrevious)
        {
            InitializeComponent();

            List<RiskStruct> arr = new List<RiskStruct>();

            if (riskPrevious.Id == -1)
            {
                arr.Add(new RiskStruct("Идентификатор угрозы:", "", actualRisk.GetFullNameId()));
                arr.Add(new RiskStruct("Описание угрозы:", "", actualRisk.Name));
                arr.Add(new RiskStruct("Наименование угрозы:", "", actualRisk.Description));
                arr.Add(new RiskStruct("Источник угрозы", "", actualRisk.SourceRisk));
                arr.Add(new RiskStruct("Объект воздействия угрозы:", "", actualRisk.ObjectImpact));
                arr.Add(new RiskStruct("Нарушение конфиденциальности:", "", Risk.GetStatusAnswer(actualRisk.PrivacyViolatation)));
                arr.Add(new RiskStruct("Нарушение целостности:", "", Risk.GetStatusAnswer(actualRisk.IntegrityViolatation)));
                arr.Add(new RiskStruct("Нарушение доступности:", "", Risk.GetStatusAnswer(actualRisk.AvaliabilityViolatation)));
            }
            else
            {
                arr.Add(new RiskStruct("Идентификатор угрозы:", riskPrevious.GetFullNameId(), "УБИ." + actualRisk.GetFullNameId()));
                arr.Add(new RiskStruct("Описание угрозы:", riskPrevious.Name, actualRisk.Name));
                arr.Add(new RiskStruct("Наименование угрозы:", riskPrevious.Description, actualRisk.Description));
                arr.Add(new RiskStruct("Источник угрозы", riskPrevious.SourceRisk, actualRisk.SourceRisk));
                arr.Add(new RiskStruct("Объект воздействия угрозы:", riskPrevious.ObjectImpact, actualRisk.ObjectImpact));
                arr.Add(new RiskStruct("Нарушение конфиденциальности:", Risk.GetStatusAnswer(riskPrevious.PrivacyViolatation), Risk.GetStatusAnswer(actualRisk.PrivacyViolatation)));
                arr.Add(new RiskStruct("Нарушение целостности:", Risk.GetStatusAnswer(riskPrevious.IntegrityViolatation), Risk.GetStatusAnswer(actualRisk.IntegrityViolatation)));
                arr.Add(new RiskStruct("Нарушение доступности:", Risk.GetStatusAnswer(riskPrevious.AvaliabilityViolatation), Risk.GetStatusAnswer(actualRisk.AvaliabilityViolatation)));
            }
            this.DataGridReportUpdateDetail.DataContext = arr;
        }

    }
}
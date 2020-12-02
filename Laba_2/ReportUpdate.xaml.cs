using Laba_2.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Laba_2
{
    /// <summary>
    /// Логика взаимодействия для ReportUpdate.xaml
    /// </summary>
    public partial class ReportUpdate : Window
    {
        private readonly List<Risk> newRisks = new List<Risk>();
        private readonly List<Risk> oldRisks = new List<Risk>();
        private readonly Dictionary<int, int> itemsUpdate = new Dictionary<int, int>();
        private readonly Paginator paginator;

        public ReportUpdate(bool status, string msgUpdate, Dictionary<int, int> itemsUpdate, List<Risk> newRisks, List<Risk> oldRisks)
        {
            InitializeComponent();
            this.newRisks = newRisks;
            this.oldRisks = oldRisks;
            this.itemsUpdate = itemsUpdate;

            List<Risk> reportList = new List<Risk>();
            foreach (var item in itemsUpdate)
            {
                reportList.Add(this.newRisks[item.Key]);
            }

            this.paginator = new Paginator(reportList);

            StatusUpdate.Text = status ? "Успешно" : "Ошибка";
            MsgUpdate.Text = msgUpdate;
            CntItemsUpdate.Text = this.itemsUpdate.Count.ToString();
            dataGridReportUpdate.DataContext = this.paginator.GeneratePage(0);

            MessageBox.Show("Чтобы измененя вступили в силу закройте окно статуса.");
        }

        private void OnRowDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (newRisks.Count > 0)
            {
                Risk newRisk = (Risk)this.dataGridReportUpdate.SelectedItem;
                Risk oldRisk = new Risk() { Id=-1};

                int indexNewRisk = newRisks.IndexOf(newRisk);
                if (indexNewRisk != -1)
                {
                    if (itemsUpdate[indexNewRisk] != -1)
                    {
                        oldRisk = oldRisks[itemsUpdate[indexNewRisk]];
                    }
                    ReportUpdateDetail riskDetail = new ReportUpdateDetail(newRisk, oldRisk);
                    riskDetail.Show();
                }
            }
        }

        private void OnNextClicked(object sender, RoutedEventArgs e)
        {
            this.dataGridReportUpdate.DataContext = this.paginator.MoveToNextPage();
            this.dataGridReportUpdate.Items.Refresh();
        }

        private void OnPreviousClicked(object sender, RoutedEventArgs e)
        {
            this.dataGridReportUpdate.DataContext = this.paginator.MoveToPreviousPage();
            this.dataGridReportUpdate.Items.Refresh();
        }
    }
}

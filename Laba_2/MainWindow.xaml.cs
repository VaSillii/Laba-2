using Laba_2.Base;
using Microsoft.Win32;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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

namespace Laba_2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Risk> risks = new List<Risk>();
        public readonly Paginator paginator;

        public MainWindow()
        {   
            InitializeComponent();
            if (Utils.IsExistFile(BaseInicialize.GetFullNameFileExcel()))
            {
                risks = Utils.ReadDataFromFileJson(BaseInicialize.basePathSave, BaseInicialize.nameSaveFile);
            }
            else
            {
                MessageBox.Show("Данных нет.\nСейчас будет произведена первичная загрузка.");
                try
                {
                    risks = Utils.GetRisks();
                    Utils.SaveDataInJson(risks, BaseInicialize.basePathSave, BaseInicialize.nameSaveFile);
                }
                catch (WebException)
                {
                    MessageBox.Show("Ошибка! Проверьте подключение к интернету или повторите операцию позднее.");
                }
                catch (Exception)
                {
                    MessageBox.Show("Ошибка! Что-то пошло не так:(");
                }
            }

            if (!(risks is null) || risks.Count != 0)
            {
                this.paginator = new Paginator(risks);
                this.DataContext = this.paginator.GeneratePage(0);
            }
        }

        private void BtnSaveFileOnClick(object sender, RoutedEventArgs e)
        {
            new Thread(new ParameterizedThreadStart(Utils.SaveDataInXlsx)).Start(risks);
        }

        private void OnRowDoubleClick(object sender, MouseButtonEventArgs e)
        {
            RiskDetail riskDetail = new RiskDetail((Risk)dataGridView1.SelectedItem);
            riskDetail.Show();    
        }

        private void OnNextClicked(object sender, RoutedEventArgs e)
        {
            this.dataGridView1.DataContext = this.paginator.MoveToNextPage();
            this.dataGridView1.Items.Refresh();
        }

        private void OnPreviousClicked(object sender, RoutedEventArgs e)
        {
            this.dataGridView1.DataContext = this.paginator.MoveToPreviousPage();
            this.dataGridView1.Items.Refresh();
        }

        private void BtnUpdateDataOnClick(object sender, RoutedEventArgs e)
        {
            if (Utils.IsExistFile(BaseInicialize.GetFullNameFileExcel()))
            {
                this.paginator.UpdateCollectionRisks(Utils.ReadDataFromFileJson(BaseInicialize.basePathSave, BaseInicialize.nameSaveFile));
                this.dataGridView1.DataContext = this.paginator.GeneratePage(0);
                this.dataGridView1.Items.Refresh();
            }
                MessageBox.Show("Данные обновлны.");
        }

        private delegate void UpdateTextCallback();

        private void DownloadFileOnClick(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(DownloadFileOnClickHandler);
            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = true;
            thread.Start();
        }

        private void DownloadFileOnClickHandler()
        {
            Utils.DownloadFileOnMainWindow(this.risks);
            this.Dispatcher.Invoke(() =>
            {
                this.risks = Utils.ReadDataFromFileJson(BaseInicialize.basePathSave, BaseInicialize.nameSaveFile);
                this.paginator.UpdateCollectionRisks(this.risks);
                this.dataGridView1.DataContext = this.paginator.GeneratePage(0);
                this.dataGridView1.Items.Refresh();
            });
        }
    }
}

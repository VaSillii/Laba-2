using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.Threading;
using Newtonsoft.Json;
using System.Windows;
using Microsoft.Win32;

namespace Laba_2.Base
{
    public class Utils
    {
        private static object locker = new object();
        public static void DownloadFileOnMainWindow(object risks)
        {
            string msg = "Обновление прошло успешно.";
            bool flag = true;
            List<Risk> newRisks = new List<Risk>(); 
            List<Risk> oldRisks = (List<Risk>)risks;

            Dictionary<int, int> updatelistRisks = new Dictionary<int, int>();

            try
            {
                newRisks = Utils.GetRisks();

                if (oldRisks.Count == 0 && newRisks.Count != 0)
                {
                    for (int i = 0; i < newRisks.Count; i++)
                    {
                        updatelistRisks.Add(i, -1);
                    }
                } 
                else if (newRisks.Count != 0 && !oldRisks.SequenceEqual(newRisks))
                {
                    for (int i = 0; i < newRisks.Count; i++)
                    {
                        bool foundInOldRisk = false;
                        for (int j = 0; j < oldRisks.Count; j++)
                        {
                            if (newRisks[i].Id == oldRisks[j].Id)
                            {
                                foundInOldRisk = true;
                                if (!newRisks[i].Equals(oldRisks[j]))
                                {
                                    updatelistRisks.Add(i, j);
                                }
                            }
                        }
                        if (!foundInOldRisk)
                        {
                            updatelistRisks.Add(i, -1);
                        }
                    }
                }
            }
            catch (WebException)
            {
                msg = "Ошибка! Проверьте подключение к интернету или повторите опрерацию позднее.";
                flag = false;
            } catch (Exception e)
            {
                msg = "Ошибка! Что-то пошло не так:(";
                flag = false;
            }
            lock (locker)
            {
                if (updatelistRisks.Count != 0)
                {
                    Utils.SaveDataInJson(newRisks, BaseInicialize.basePathSave, BaseInicialize.nameSaveFile);
                }
                ReportUpdate reportUpdate = new ReportUpdate(flag, msg, updatelistRisks, newRisks, oldRisks);
                reportUpdate.ShowDialog();
            }
        }

        public static void DeleteLocalDb()
        {
            if (File.Exists(BaseInicialize.GetFullNameFileExcel()))
            {
                File.Delete(BaseInicialize.GetFullNameFileExcel());
            }
        }

        public static bool IsExistFile(string path)
        {
            return File.Exists(Path.Combine(BaseInicialize.basePathSave, BaseInicialize.nameSaveFile));
        }

        public static List<Risk> ReadDataFromFileJson(string path, string name)
        {
            List<Risk> risks = new List<Risk>();
            try
            {
                risks = JsonConvert.DeserializeObject<List<Risk>>(File.ReadAllText(Path.Combine(path, name)));
            }
            catch (JsonException)
            {
                MessageBox.Show("Ошибка. Данные локальной базы данных некорректны, обновите их!");
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка! Что-то пошло не так:( Попробуйте обновить данные." + e.Message);
            }
            return risks;
        }

        public static void SaveDataInJson(List<Risk> list, string path, string name_file)
        {
            string file_path_json = Path.Combine(path, name_file);
            try
            {
                //Utils.DeleteLocalDb();
               
                File.WriteAllText(file_path_json, JsonConvert.SerializeObject(list));
                
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show("Ошибка сохранения! " + e.Message);
            }
        }


        public static void SaveDataInXlsx(object risks)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel |*.xlsx";
            try
            {
                if (saveFileDialog.ShowDialog() == true)
                {
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                    using (ExcelPackage excelPackage = new ExcelPackage())
                    {
                        ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet 1");
                        worksheet.Cells["A1"].Value = "Идентификатор УБИ";
                        worksheet.Cells["B1"].Value = "Наименование УБИ";
                        worksheet.Cells["C1"].Value = "Описание";
                        worksheet.Cells["D1"].Value = "Источник угрозы (характеристика и потенциал нарушителя)";
                        worksheet.Cells["E1"].Value = "Объект воздействия";
                        worksheet.Cells["F1"].Value = "Нарушение конфиденциальности";
                        worksheet.Cells["G1"].Value = "Нарушение целостности";
                        worksheet.Cells["H1"].Value = "Нарушение доступности";
                        worksheet.Cells["A2"].LoadFromCollection((List<Risk>)risks);
                        byte[] bin = excelPackage.GetAsByteArray();
                        File.WriteAllBytes(saveFileDialog.FileName, bin);
                    }
                    MessageBox.Show("Файл сохранен!");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка сохранения!");
            }
        }
        public static byte[] DownloadFile()
        {
            byte[] arr = new byte[0];
            try
            {
                using (var client = new WebClient() { Encoding = Encoding.UTF8 })
                {
                    arr=  client.DownloadData(new Uri(BaseInicialize.url));
                }
            }
            catch (ArgumentNullException)
            {
                throw new WebException();
            } 
            catch (WebException)
            {
                throw new WebException();
            }
            catch (NotSupportedException)
            {
                throw new WebException();
            }

            return arr;
        }

        public static List<Risk> GetRisks()
        {
            List<Risk> risks = new List<Risk>();
            try
            {
                byte[] arr = DownloadFile();
                risks = ParseFile(arr);
            }
            catch (Exception)
            {
                throw;
            }
            return risks;
        }

        private static List<Risk> ParseFile(byte[] arr)
        {
            List<Risk> risks = new List<Risk>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (MemoryStream stream = new MemoryStream(arr))
            {
                using (ExcelPackage excelPackage = new ExcelPackage(stream))
                {
                    foreach (ExcelWorksheet worksheet in excelPackage.Workbook.Worksheets)
                    {
                        for (int i = worksheet.Dimension.Start.Row + 2; i <= worksheet.Dimension.End.Row; i++)
                        {
                            try
                            {
                                Risk risk = new Risk();
                                int numberColumn = 1;
                                foreach (var item in typeof(Risk).GetProperties())
                                {
                                    if (item.PropertyType.Name == "Int32")
                                    {
                                        item.SetValue(risk, int.Parse(worksheet.Cells[i, numberColumn].Value.ToString()));
                                    }
                                    else if (item.PropertyType.Name == "String")
                                    {
                                        item.SetValue(risk, worksheet.Cells[i, numberColumn].Value.ToString());
                                    }
                                    else if (item.PropertyType.Name == "Boolean")
                                    {
                                        item.SetValue(risk, int.Parse(worksheet.Cells[i, numberColumn].Value.ToString()) == 1 ? true : false);
                                    }
                                    numberColumn++;
                                }
                                risks.Add(risk);
                            }
                            catch
                            {
                                continue;
                            }
                        }
                    }
                }
            }
            return risks;
        }

    }
}

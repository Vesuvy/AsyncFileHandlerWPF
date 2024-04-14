using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace AsyncFileHandlerWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        
        private void BtnSelectFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                TbNumberOfFiles.Text = "1";
                TbResultFile.Text = File.ReadAllText(openFileDialog.FileName);
            }
        }
        /*private void BtnSaveFile_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Выберите папку для сохранения файла.");
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string directory = System.IO.Path.GetDirectoryName(saveFileDialog.FileName);
                string fileName = System.IO.Path.GetFileName(saveFileDialog.FileName);
                string filePath = System.IO.Path.Combine(directory, fileName);
                File.WriteAllText(filePath, TbResultFile.Text);
                MessageBox.Show("Файл сохранен.");
            }
        }*/
        private async void BtnSaveFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                await SaveFileContent(saveFileDialog.FileName, TbResultFile.Text);
                MessageBox.Show("Файл успешно сохранен.");
            }
        }
        private Task SaveFileContent(string filePath, string content)
        {
            return Task.Run(() => File.WriteAllText(filePath, content));
        }

        private async Task<string> ProcessFileAsync(string fileContent)
        {
            string text = fileContent;

            // Получаем параметры обработки из интерфейса
            bool deletePunctuation = true;
            int minLength = 0;
            if (ChboxDeleteWords.IsChecked == true && int.TryParse(TbLengthWords.Text, out int length))
            {
                minLength = length;
            }

            // Удаление знаков препинания
            if (deletePunctuation)
            {
                text = await Task.Run(() => Regex.Replace(text, @"\p{P}", ""));
            }

            // Удаление слов длиной менее minLength символов
            if (minLength > 0)
            {
                text = await Task.Run(() => string.Join(" ", text.Split().Where(word => word.Length >= minLength)));
            }

            return TbResultFile.Text = text;

            // Сохранение обработанного текста в файл

            //string resultFileName = System.IO.Path.GetFileNameWithoutExtension(fileName) + "_processed.txt";
            //string resultFilePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(fileName), resultFileName);

            //await File.WriteAllTextAsync(resultFilePath, text);
            //MessageBox.Show("Файл обработан и сохранен.");
        }
        private async void ChboxDeletePunct_Checked(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TbResultFile.Text))
            {
                TbResultFile.Text = await ProcessFileAsync(TbResultFile.Text);
            }
            else
            {
                MessageBox.Show("Файл не выбран");
            }
        }

        private async void ChboxDeleteWords_Checked(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TbResultFile.Text))
            {
                TbResultFile.Text = await ProcessFileAsync(TbResultFile.Text);
            }
            else
            {
                MessageBox.Show("Файл не выбран");
            }
        }
    }
}

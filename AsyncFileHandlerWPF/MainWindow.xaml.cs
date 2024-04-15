using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using static System.Net.Mime.MediaTypeNames;

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

        private string originalText = string.Empty;

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
                originalText = File.ReadAllText(openFileDialog.FileName);
                TbResultFile.Text = originalText;
            }
        }
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

        private async void ChboxDeleteWords_Checked(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TbResultFile.Text))
            {
                string a = "";
                await Dispatcher.InvokeAsync(() => 
                    a = string.Join(" ", TbResultFile.Text.Split().Where(word => word.Length >= int.Parse(TbLengthWords.Text))));
                TbResultFile.Text = a;                    
            }
            else
            {
                MessageBox.Show("Файл не выбран");
            }
        }
        private async void ChboxDeleteWords_Unchecked(object sender, RoutedEventArgs e)
        {
            await Dispatcher.InvokeAsync(() => TbResultFile.Text = originalText);
            
        }
        private async void ChboxDeletePunct_Checked(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TbResultFile.Text))
            {
                string a = "";
                await Dispatcher.InvokeAsync(() => 
                    a = Regex.Replace(TbResultFile.Text, @"\p{P}", ""));
                TbResultFile.Text = a;
            }
            else
            {
                MessageBox.Show("Файл не выбран");
            }
        }
        private async void ChboxDeletePunct_Unchecked(object sender, RoutedEventArgs e)
        {
            await Dispatcher.InvokeAsync(() => TbResultFile.Text = originalText);
        }
    }
}

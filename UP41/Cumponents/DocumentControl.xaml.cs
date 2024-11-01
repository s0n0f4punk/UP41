using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UP41.Windows;

namespace UP41.Cumponents
{
    /// <summary>
    /// Логика взаимодействия для DocumentControl.xaml
    /// </summary>
    public partial class DocumentControl : UserControl
    {
        private Document document;
        private DocumentWindow window;
        public DocumentControl(Document document, DocumentWindow window, bool canEdit)
        {
            InitializeComponent();
            this.document = document;
            this.window = window;
            DataContext = document;
            Trash.Visibility = canEdit ? Visibility.Visible : Visibility.Collapsed;
        }
        private void Trash_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить этот документ?", "Подтверждение", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    window.RemoveDocument(document);
                    window.Refresh();
                    MessageBox.Show("Документ успешно удален!");
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }

        private void Save_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                FileName = document.Name,
                DefaultExt = document.Format,
            };
            if (dialog.ShowDialog() == true)
            {
                File.WriteAllBytes(dialog.FileName, document.Bytes);
                MessageBox.Show("Файл успешно сохранен на компьютере!");
            }
        }
    }
}

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

namespace UP41.Cumponents
{
    /// <summary>
    /// Логика взаимодействия для ItemUC.xaml
    /// </summary>
    public partial class ItemUC : UserControl
    {
        private ItemLocation itemLocation;
        public ItemUC(WorkshopItem item)
        {
            InitializeComponent();
            DataContext = item;
        }

        private void MainImage_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                itemLocation = new ItemLocation()
                {
                    Item = DataContext as WorkshopItem
                };
                DragDrop.DoDragDrop(MainImage, new DataObject(DataFormats.Serializable,
                    itemLocation), DragDropEffects.Move);
            }
        }
    }
}

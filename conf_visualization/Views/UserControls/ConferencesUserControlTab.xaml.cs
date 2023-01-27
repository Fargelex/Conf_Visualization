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

namespace conf_visualization.Views.UserControls
{
    /// <summary>
    /// Interaction logic for ConferencesUserControlTab.xaml
    /// </summary>
    public partial class ConferencesUserControlTab : UserControl
    {
        public ConferencesUserControlTab()
        {
            InitializeComponent();
        }

        bool newItem = false;

        private void ConferencesDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            MessageBox.Show("ConferencesDataGrid_RowEditEnding "+ newItem.ToString());
            newItem = false;
        }

        private void ConferencesDataGrid_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            newItem = true;
            MessageBox.Show("ConferencesDataGrid_AddingNewItem");
            
        }

        private void ConferencesDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            saveButton.IsEnabled = true;
        }

        
    }
}

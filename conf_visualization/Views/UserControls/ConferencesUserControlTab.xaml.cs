using System;
using System.Collections.Generic;
using System.Globalization;
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
using conf_visualization.Models;
using conf_visualization.ViewModels;

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
            ConferencesDataGrid.SelectedIndex= 0;
        }

        private void ConferencesDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
         //   saveEditConferencesSettingsButton.Command.Execute(e.Row.Item);
          //  MessageBox.Show("ConferencesDataGrid_RowEditEnding");
        }

        private void ConferencesDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            saveEditConferencesSettingsButton.Command.Execute(e.Row.Item);
        }

        private void refreshContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            refreshContextMenuItem.Command.Execute(sender);
        }

        private void ConferencesDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            
            //CommandParameter="{Binding RelativeSource={RelativeSource FindAncestor, AncestorType={x:Type ContextMenu}}, Path=PlacementTarget.SelectedItem}"
            if (e.Key == Key.Delete)
            {
                deleteEditConferencesSettingsButton.Command.Execute((ConferenceModel)ConferencesDataGrid.SelectedItem);
            }
        }

        private void deleteConferenceContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            deleteEditConferencesSettingsButton.Command.Execute((ConferenceModel)ConferencesDataGrid.SelectedItem);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ConferencesDataGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            MessageBox.Show("");
            
        }
    }
}

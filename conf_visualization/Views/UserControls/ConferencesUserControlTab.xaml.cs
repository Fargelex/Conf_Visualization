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
using conf_visualization.ViewModels;

namespace conf_visualization.Views.UserControls
{
    /// <summary>
    /// Interaction logic for ConferencesUserControlTab.xaml
    /// </summary>
    public partial class ConferencesUserControlTab : UserControl
    {
        bool GridEdited = false;
        public ConferencesUserControlTab()
        {
            InitializeComponent();
        }

        private void ConferencesDataGrid_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            GridEdited = true;
            ConferencesSettingsButtonsStackPanel.Visibility = Visibility.Visible;
        }
        private void ConferencesDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            GridEdited = true;
            ConferencesSettingsButtonsStackPanel.Visibility = Visibility.Visible;
        }

        private void saveEditConferencesSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            ConferencesSettingsButtonsStackPanel.Visibility = Visibility.Collapsed;
        }

        private void cancelEditConferencesSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            ConferencesSettingsButtonsStackPanel.Visibility = Visibility.Collapsed;
        }
        

        private void ConferencesDataGrid_LostFocus(object sender, RoutedEventArgs e)
        {
                      
        }

        private void ConferencesDataGrid_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            MessageBox.Show("В таблице селекторов были изменения", "ConferencesDataGrid_ManipulationCompleted");
        }

        private void ConferencesDataGrid_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
           
        }

        private void ConferencesDataGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            if (GridEdited)
            {
                MessageBox.Show("В таблице селекторов были изменения", "Сохранить изменения?");
            }
        }
    }
}

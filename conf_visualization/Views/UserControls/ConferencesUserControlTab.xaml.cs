﻿using System;
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
        public ConferencesUserControlTab()
        {
            InitializeComponent();
        }

        bool newItem = false;



        private void ConferencesDataGrid_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {        
        }

        private void ConferencesDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

        }

        private void conferencesSettingsSaveButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void hideAddEditConferenceGridButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void editCOnferenceSettings_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}

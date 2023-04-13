// ------------------------------------------------------------------------
// Husky - Call of Duty BSP Extractor
// Copyright (C) 2018 Philip/Scobalula
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
// ------------------------------------------------------------------------
using System;
using System.Windows;
using System.Threading;
using System.Windows.Shell;
using Husky;
using System.Reflection;
using System.Globalization;

namespace HuskyUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Whether or not a thread is active
        /// </summary>
        public bool ThreadActive = false;

        /// <summary>
        /// Initializes MainWindow
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            // Set title
            Title = String.Format("Husky - Version {0}", Assembly.GetExecutingAssembly().GetName().Version);
            // Initial Print
            PrintLine("Load a supported CoD Game, then click the paper plane to export loaded BSP data.");
            PrintLine("");

            // Reset culture for OBJs.
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        }

        private void ExportClick(object sender, RoutedEventArgs e)
        {
            // Check is a thread already active
            if (ThreadActive)
                return;
            // Create new thread and export
            new Thread(delegate ()
            {
                UpdateProgressState(TaskbarItemProgressState.Indeterminate);
                ThreadActive = true;
                HuskyUtil.LoadGame(PrintLine);
                PrintLine("");
                ThreadActive = false;
                UpdateProgressState(TaskbarItemProgressState.Normal);
            }).Start();
        }

        /// <summary>
        /// Prints to the ConsoleBox
        /// </summary>
        /// <param name="value">Value to print</param>
        private void PrintLine(object value)
        {
            Dispatcher.BeginInvoke(new Action(() => ConsoleBox.AppendText(value.ToString() + Environment.NewLine)));
        }

        /// <summary>
        /// Updates Progress State
        /// </summary>
        /// <param name="state">State to set</param>
        private void UpdateProgressState(TaskbarItemProgressState state)
        {
            Dispatcher.BeginInvoke(new Action(() => TaskBarProgress.ProgressState = state));
        }

        /// <summary>
        /// Shows the About Window and Dims the Main Window
        /// </summary>
        private void AboutClick(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow()
            {
                Owner = this
            };
            aboutWindow.VersionLabel.Content = String.Format("Version: {0}", Assembly.GetExecutingAssembly().GetName().Version);
            DimBox.Visibility = Visibility.Visible;
            aboutWindow.ShowDialog();
            DimBox.Visibility = Visibility.Hidden;
        }
    }
}

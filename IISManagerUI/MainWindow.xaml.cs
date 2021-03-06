﻿using System.Windows;

namespace IISManagerUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(AppPoolsControl appPoolsControl, TopPanel topPanel, BottomPanel bottomPanel)
        {
            InitializeComponent();
            topPanelBorder.Child = topPanel;
            appPoolsScrollViewer.Content = appPoolsControl;
            bottomPanelBorder.Child = bottomPanel;
        }
    }
}

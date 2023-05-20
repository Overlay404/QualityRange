﻿using QualityRange.View.Pages;
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
using System.Windows.Shapes;

namespace QualityRange.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public static MainWindow Instance;
        private double positionCursorX;
        public MainWindow()
        {
            InitializeComponent();

            ShutdownBtn.MouseUp += (sender, e) => ShutdownApplication();

            MinimizeBtn.MouseUp += (sender, e) => MinimizedWindow();

            MaximizeBtn.MouseUp += (sender, e) => MaximizedWindow();

            DragMoveBar.MouseDown += DragMoveWindow;

            

            // SignInBtn ShopBasketBtn SearchBtn
        }
        private static void ShutdownApplication()
        {
            Application.Current.Shutdown();
        }

        private void MinimizedWindow()
        {
            WindowState =  WindowState.Minimized;
        }

        private void MaximizedWindow()
        {
            if(WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                return;
            }

            WindowState = WindowState.Maximized;
        }

        private void DragMoveWindow(object sender, MouseButtonEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                positionCursorX = PointToScreen(new Point(e.GetPosition(null).X, e.GetPosition(null).Y)).X;
                WindowState = WindowState.Normal;
                Top = 0;
                Left = positionCursorX - Width / 2;
            }
            DragMove();
        }

        #region ResizeWindows
        private bool ResizeInProcess = false;
        private void Resize_Init(object sender, MouseButtonEventArgs e)
        {
            if (sender is Rectangle senderRect)
            {
                ResizeInProcess = true;
                senderRect.CaptureMouse();
            }
        }

        private void Resize_End(object sender, MouseButtonEventArgs e)
        {
            Rectangle senderRect = sender as Rectangle;
            if (senderRect != null)
            {
                ResizeInProcess = false; ;
                senderRect.ReleaseMouseCapture();
            }
        }

        private void Resizeing_Form(object sender, MouseEventArgs e)
        {
            if (ResizeInProcess)
            {
                Rectangle senderRect = sender as Rectangle;
                Window mainWindow = senderRect.Tag as Window;
                if (senderRect != null)
                {
                    double width = e.GetPosition(mainWindow).X;
                    double height = e.GetPosition(mainWindow).Y;
                    senderRect.CaptureMouse();
                    if (senderRect.Name.ToLower().Contains("right"))
                    {
                        width += 5;
                        if (width > 0)
                        {
                            mainWindow.Width = width;
                        }
                    }
                    if (senderRect.Name.ToLower().Contains("left"))
                    {
                        width -= 5;
                        mainWindow.Left += width;
                        width = mainWindow.Width - width;
                        if (width > 0)
                        {
                            mainWindow.Width = width;
                        }
                    }
                    if (senderRect.Name.ToLower().Contains("bottom"))
                    {
                        height += 5;
                        if (height > 0)
                        {
                            mainWindow.Height = height;
                        }
                    }
                    if (senderRect.Name.ToLower().Contains("top"))
                    {
                        height -= 5;
                        mainWindow.Top += height;
                        height = mainWindow.Height - height;
                        if (height > 0)
                        {
                            mainWindow.Height = height;
                        }
                    }
                }
            }
        }
        #endregion
    }
}

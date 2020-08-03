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
namespace ZoomAndPanToCursor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MouseWheel += MainWindow_MouseWheel;

            MouseLeftButtonDown += MainWindow_MouseLeftButtonDown;
            MouseMove += MainWindow_MouseMove;
            MouseLeftButtonUp += MainWindow_MouseLeftButtonUp;
        }

        private void MainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => KnorticMatrix.HandleMouseDown(image, e);

        private void MainWindow_MouseMove(object sender, MouseEventArgs e) => KnorticMatrix.Pan(image, e);

        private void MainWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) => KnorticMatrix.HandleMouseUp();

        private void MainWindow_MouseWheel(object sender, MouseWheelEventArgs e) => KnorticMatrix.Zoom(image, e, 5.0);
    }
}

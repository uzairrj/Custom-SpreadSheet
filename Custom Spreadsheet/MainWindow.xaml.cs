using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace customProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        mainClass main_obj = new mainClass();

        CartesianChart graph = null;

        Grid content = null;

        string[,] prev = new string[26, 26];

        TextBox currentFocused = null;

        public string[] Labels { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            //Add references to access throut the class
            content = textBoxGrid;
            graph = myGraph;

            //Add textboxes dynamicaly
            for (int row = 0; row < 26; row++)
            {
                for (int col = 0; col < 26; col++)
                {
                    TextBox newBox = new TextBox();
                    newBox.Name = "text" + row + "_" + col;
                    newBox.LostFocus += onTextEnter;
                    newBox.GotFocus += ongotFocused;
                    Grid.SetColumn(newBox, col);
                    Grid.SetRow(newBox, row);
                    textBoxGrid.Children.Add(newBox);
                }
            }

            
        }

        void ongotFocused(object sender, RoutedEventArgs e)
        {
            TextBox textbox = (TextBox)sender;
            currentFocused = textbox;
            textbox.Text = prev[Grid.GetRow(textbox), Grid.GetColumn(textbox)];
        }
        /// <summary>
        /// when textbox value changed(Focused Changed)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void onTextEnter(object sender, RoutedEventArgs e)
        {
            TextBox textbox = (TextBox)sender;

            try
            {
                var operation = main_obj.parse(textbox.Text);

                prev[Grid.GetRow(textbox), Grid.GetColumn(textbox)] = textbox.Text;
                if (operation.oper > 0)
                {
                    textbox.Text = main_obj.applyOperation(operation, content).ToString();
                }
                textbox.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                textbox.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));

            }
            catch (Exception ex)
            {
                if (textbox.Text[0] == '=')
                {
                    prev[Grid.GetRow(textbox), Grid.GetColumn(textbox)] = textbox.Text;
                }
                textbox.Background = new SolidColorBrush(Color.FromRgb(242, 68, 71));
                textbox.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }
        }

        /// <summary>
        /// Generate Graph Button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void generateGraph(object sender, RoutedEventArgs e)
        {
            operation operation;
            List<double> array;
            List<string> labels;
            try
            {
                operation = main_obj.parseGraph(graphText.Text);
                array = main_obj.applyGraph(content, operation, out labels);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message,"Error",MessageBoxButton.OK,MessageBoxImage.Error);
                return;
            }
            graph.Series = new SeriesCollection
            {
                new ColumnSeries
                {
                    Name = "Mygraph",
                    Values = new ChartValues<double>(array)
                }
            };
            Labels = labels.ToArray();
 
            var doubleMapperWithMonthColors = new LiveCharts.Configurations.CartesianMapper<double>()
            .X((value, index) => index)
            .Y((value) => value)
            .Fill((v, i) =>
            {
                switch (i % 12)
                {
                    case 0: return Brushes.LightBlue; 
                    case 1: return Brushes.LightCoral; 
                    case 2: return Brushes.PaleGoldenrod; 
                    case 3: return Brushes.OrangeRed; 
                    case 4: return Brushes.BlueViolet; 
                    case 5: return Brushes.Chocolate; 
                    case 6: return Brushes.PaleVioletRed; 
                    case 7: return Brushes.CornflowerBlue; 
                    case 8: return Brushes.Orchid; 
                    case 9: return Brushes.Thistle; 
                    case 10: return Brushes.BlanchedAlmond; 
                    case 11: return Brushes.YellowGreen; 
                    default: return Brushes.Red;
                }
            });

            LiveCharts.Charting.For<double>(doubleMapperWithMonthColors, SeriesOrientation.Horizontal);
            
            graph.Update();
            DataContext = this;
        }

        private void onSumClicked(object sender, RoutedEventArgs e)
        {
            currentFocused.Text = "=SUM ";
            currentFocused.CaretIndex = 5;
        }
        private void onMeanClicked(object sender, RoutedEventArgs e)
        {
            currentFocused.Text = "=MEAN ";
            currentFocused.CaretIndex = 6;
        }
        private void onModeClicked(object sender, RoutedEventArgs e)
        {
            currentFocused.Text = "=MODE ";
            currentFocused.CaretIndex = 6;
        }
        private void onMedianClicked(object sender, RoutedEventArgs e)
        {
            currentFocused.Text = "=MEDIAN ";
            currentFocused.CaretIndex = 8;
        }

        private void onSave(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Dumy Excel File (*.ex)|*.ex";
            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllText(saveFileDialog.FileName, main_obj.save(content));
        }

        private void onLoad(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Dumy Excel File (*.ex)|*.ex";
            if (openFileDialog.ShowDialog() == true)
                main_obj.load(content,File.ReadAllText(openFileDialog.FileName));
        }

        private void onNew(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult =MessageBox.Show("Are you sure?\nYou did not save your work.", "Confirmation", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes) {
                main_obj.newGrid(content);
            }
        }
        private void onExit(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure?\nYou may have unsaved changes.", "Exit Confirmation", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
    }

    
}

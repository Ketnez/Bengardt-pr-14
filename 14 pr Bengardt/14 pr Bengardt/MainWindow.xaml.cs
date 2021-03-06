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
using System.IO;
using Microsoft.Win32;

namespace _14_pr_Bengardt
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Login login = new();
            login.ShowDialog();

            InitializeComponent();
           

        }
        private Matrix _matrix = new Matrix(3, 3);

        private void FillMatrix_Click(object sender, RoutedEventArgs e)
        {
            _matrix.Fill();
            sizeRow.Text = $"Строк {dataGrid.Items.Count}";
            sizeColumn.Text = $"Столбцов {dataGrid.Columns.Count}";
            dataGrid.ItemsSource = _matrix.ToDataTable().DefaultView;
        }

        private void SaveMatrix_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Текстовый документ (*.txt)|*.txt|Все файлы (*.*)|*.*";
            saveFileDialog.DefaultExt = ".txt";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.Title = "Экспорт";

            if (saveFileDialog.ShowDialog() == true)
            {
                _matrix.Export(saveFileDialog.FileName);
            }
            dataGrid.ItemsSource = null;
            dataGridResult.ItemsSource = null;
        }

        private void OpenMatrix_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Текстовый документ (*.txt)|*.txt|Все файлы (*.*)|*.*";
            openFileDialog.DefaultExt = ".txt";
            openFileDialog.FilterIndex = 1;
            openFileDialog.Title = "Импорт";

            if (openFileDialog.ShowDialog() == true)
            {
                _matrix.Import(openFileDialog.FileName);
            }
            dataGrid.ItemsSource = _matrix.ToDataTable().DefaultView;
        }

        private void Swap_Click(object sender, RoutedEventArgs e)
        {
            int tmp = 0;
            int Nmax = 0, Nmin = 0;
            int min = _matrix[0, 0];
            int max = _matrix[0, 0];
            for (int i = 0; i < _matrix.RowLength; i++)
            {
                for (int j = 0; j < _matrix.ColumnLength; j++)
                {
                    if (_matrix[i, j] > max)
                    {
                        max = _matrix[i, j];
                        Nmax = i;
                    }
                    if (_matrix[i, j] < min)
                    {
                        min = _matrix[i, j];
                        Nmin = i;
                    }
                }
            }
            for (int j = 0; j < _matrix.ColumnLength; j++)
            {
                tmp = _matrix[Nmax, j];
                _matrix[Nmax, j] = _matrix[Nmin, j];
                _matrix[Nmin, j] = tmp;
            }
            dataGridResult.ItemsSource = _matrix.ToDataTable().DefaultView;
        }



        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CurrentCellIndex_Click(object sender, MouseEventArgs e)
        {
            selectedCell.Text = $"[{dataGrid.Items.IndexOf(dataGrid.CurrentItem) + 1};" +
                $"{dataGrid.CurrentColumn.DisplayIndex + 1}]";
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = null;
        }

        private void ClearMatrix_Click(object sender, RoutedEventArgs e)
        {
            dataGridResult.ItemsSource = null;
        }
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
           
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Login login = new();
            
            login.Owner = this;
            login.ShowDialog();
        }
        private void options_Click(object sender, RoutedEventArgs e)
        {
            Settings settings = new();
            settings.Owner = this;
            settings.ShowDialog();
            inputRowCount.Text = MatrixSizeContainer.RowCount.ToString();
            inputColumnCount.Text = MatrixSizeContainer.ColumnCount.ToString();
            int rowCount = Int32.Parse(inputRowCount.Text);
            int columnCount = Int32.Parse(inputColumnCount.Text);
            _matrix = matrixActs.Generate(rowCount, columnCount);
            mainDataGrid.ItemsSource = Matrix.ToDataTable(_matrix).DefaultView;
            matrixInfo.Content = $"Матрица {_matrix.GetLength(0)} x {_matrix.GetLength(1)}";
        }
    }
}

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
using UKPO2;

namespace Interface
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DNAGraph dnaGraph;

        private String moleculeName;
        private int numOfFragments;
        private String[] fragments;

        //TEST
        private String[] testFragments = { "АГЦЦ", "ЦГГУ", "ГГУАА", "УААЦЦ" };
        private String testMolecule = "АГЦЦГГУААЦЦ";

        public MainWindow()
        {
            InitializeComponent();
            OnInitialization();
        }

        public void OnInitialization()
        {
            OutputCanvas.IsEnabled = false;
        }

        private void SubmitInputButton_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(MoleculeInputTextBox.Text) &&
                !String.IsNullOrWhiteSpace(FragNumTextBox.Text))
            {
                InitDNAGraph();
                ChangeActiveCanvas();
                FillOutput();
            }
            else
            {
                MessageBox.Show("Поля ввода не заполнены!");
            }
        }

        public void InitDNAGraph()
        {
            if (!((bool)TestDataCheckBox.IsChecked))
            {
                moleculeName = MoleculeInputTextBox.Text.ToString();
                numOfFragments = int.Parse(FragNumTextBox.Text.ToString());
                fragments = FragmentsCreator.GetFragments(moleculeName, numOfFragments);
                dnaGraph = new DNAGraph(moleculeName, fragments);
            }
            else
            {
                moleculeName = testMolecule;
                fragments = testFragments;
                dnaGraph = new DNAGraph(testMolecule, testFragments);
            }
        }

        public void ChangeActiveCanvas()
        {
            if (InputCanvas.IsEnabled == true)
            {
                MoleculeInputTextBox.Clear();
                FragNumTextBox.Clear();

                InputCanvas.IsEnabled = false;
                OutputCanvas.IsEnabled = true;
            }
            else
            {
                OutputCanvas.IsEnabled = false;
                InputCanvas.IsEnabled = true;

                MoleculeNameLabel.Content = "";
                FragmentsListBox.Items.Clear();
                OutputPathsListBox.Items.Clear();
            }
        }

        public void FillOutput()
        {
            MoleculeNameLabel.Content = moleculeName;

            foreach (var fragment in fragments)
            {
                FragmentsListBox.Items.Add(fragment);
            }
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeActiveCanvas();
        }

        private void FindPathsButton_Click(object sender, RoutedEventArgs e)
        {
            var paths = dnaGraph.GetPaths();

            if (paths.Count != 0)
            {
                foreach (var path in paths)
                {
                    String pathOutput = String.Empty;
                    foreach (var node in path)
                    {
                        pathOutput += node + " -> ";
                    }
                    pathOutput = pathOutput.Remove(pathOutput.Length - 4);

                    OutputPathsListBox.Items.Add(pathOutput);
                }
            }
            else
            {
                OutputPathsListBox.Items.Add("По полученным фрагментам невозможно составить молекулу");
            }
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            for (var i = 0; i < 10000; ++i)
            {
                var paths = dnaGraph.GetPaths();
                TestLabel.Content = i.ToString();
            }
        }
    }
}

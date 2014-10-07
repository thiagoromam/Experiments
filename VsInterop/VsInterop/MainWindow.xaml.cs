using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using EnvDTE;
using EnvDTE100;

// http://www.viva64.com/en/b/0169/
// http://msdn.microsoft.com/en-us/library/y849h0w1.aspx
// http://msdn.microsoft.com/en-us/library/1xt0ezx9.aspx

namespace VsInterop
{
    public partial class MainWindow
    {
        private readonly string _solutionFolder;
        private const string SolutionName = "GeneratedSolution";
        private const string ProjectName = "SharedContent";
        private DTE _ide;

        public MainWindow()
        {
            InitializeComponent();

            InitializeVisualStudio();
            _solutionFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), SolutionName + @"\");

            Closed += (s, e) => CloseVisualStudio();
        }

        void OnCreateSolutionClick(object sender, RoutedEventArgs e)
        {
            using (new MessageFilter())
            {
                var sln = (Solution4)_ide.Solution;

                sln.Create(_solutionFolder, SolutionName);

                var projectTemplate = sln.GetProjectTemplate(@"Store Apps\Universal Apps\1033\SharedEmptyMaster\SharedEmptyMaster.vstemplate", "CSharp");
                var projectPath = Path.Combine(_solutionFolder, ProjectName + @"\");
                sln.AddFromTemplate(projectTemplate, projectPath, ProjectName, false);

                var project = sln.Projects.Item(1);
                var projectNamespace = project.Properties.Cast<Property>().Single(p => p.Name == "RootNamespace");
                projectNamespace.Value = ProjectName;

                _ide.ExecuteCommand("File.SaveAll");

                MessageBox.Show("Solution Created");
            }
        }
        void OnShowSolutionInfoClick(object sender, RoutedEventArgs e)
        {
            using (new MessageFilter())
            {
                var sln = GetSolution();

                var message = string.Format("Projects Count: {0}", sln.Projects.Count);

                foreach (Project project in sln.Projects)
                {
                    message += "\n  Project: " + project.Name;
                    message = project.ProjectItems.Cast<ProjectItem>().Aggregate(message, (m, i) => m + ("\n      " + i.Name));
                }

                MessageBox.Show(message);
            }
        }
        void OnAddClassClick(object sender, RoutedEventArgs e)
        {
            if (!Regex.IsMatch(ClassName.Text, "^[A-Z][A-z0-9]+$"))
            {
                MessageBox.Show("Inform a valid class name");
                return;
            }

            using (new MessageFilter())
            {
                var sln = GetSolution();

                var className = ClassName.Text + ".cs";
                var classTemplate = sln.GetProjectItemTemplate("Class.zip", "CSharp");
                var sharedProject = sln.Projects.Cast<Project>().Single(f => f.Name == ProjectName);
                sharedProject.ProjectItems.AddFromTemplate(classTemplate, className);

                _ide.ExecuteCommand("File.SaveAll");

                MessageBox.Show(className + " added!");
            }
        }

        Solution4 GetSolution()
        {
            if (!_ide.Solution.IsOpen)
                _ide.Solution.Open(Path.Combine(_solutionFolder, SolutionName + ".sln"));

            return (Solution4)_ide.Solution;
        }
        void InitializeVisualStudio()
        {
            var type = Type.GetTypeFromProgID("VisualStudio.DTE.12.0", true);
            _ide = (DTE)Activator.CreateInstance(type, true);
        }
        void CloseVisualStudio()
        {
            _ide.Quit();
        }
    }
}

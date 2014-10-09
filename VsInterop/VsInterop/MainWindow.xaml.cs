using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using EnvDTE;
using EnvDTE100;

namespace VsInterop
{
    // http://www.viva64.com/en/b/0169/
    // http://msdn.microsoft.com/en-us/library/xc52cke4.aspx

    public partial class MainWindow
    {
        private readonly string _solutionFolder;
        private const string SolutionName = "GeneratedSolution";
        private const string SharedProjectName = "SharedContent";
        private DTE _ide;
        private DTEEvents _ideEvents;

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

                var sharedProjectTemplate = sln.GetProjectTemplate(@"Store Apps\Universal Apps\1033\SharedEmptyMaster\SharedEmptyMaster.vstemplate", "CSharp");
                var sharedProjectPath = Path.Combine(_solutionFolder, SharedProjectName + @"\");
                sln.AddFromTemplate(sharedProjectTemplate, sharedProjectPath, SharedProjectName, false);

                var sharedProject = sln.Projects.Item(1);
                var sharedProjectNamespace = sharedProject.Properties.Cast<Property>().Single(p => p.Name == "RootNamespace");
                sharedProjectNamespace.Value = SharedProjectName;

                var monogameProjectTemplate = sln.GetProjectTemplate(@"MonoGame\Windows.zip", "CSharp");
                const string monogameProjectName = "MonogameProject";
                var monogameProjectPath = Path.Combine(_solutionFolder, monogameProjectName + @"\");
                sln.AddFromTemplate(monogameProjectTemplate, monogameProjectPath, monogameProjectName, false);

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
                var sharedProject = sln.Projects.Cast<Project>().Single(f => f.Name == SharedProjectName);
                sharedProject.ProjectItems.AddFromTemplate(classTemplate, className);

                _ide.ExecuteCommand("File.SaveAll");

                MessageBox.Show(className + " added!");
            }
        }
        void OnShowVisualStudioClick(object sender, RoutedEventArgs e)
        {
            _ide.UserControl = true;
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

            _ideEvents = _ide.Events.DTEEvents;
            _ideEvents.OnBeginShutdown += InitializeVisualStudio;
        }
        void CloseVisualStudio()
        {
            _ide.Quit();
        }
    }
}

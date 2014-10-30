using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using EnvDTE;
using EnvDTE100;
using EnvDTE80;

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
            OnShowVisualStudioClick(null, null);

            Closed += (s, e) => CloseVisualStudio();
        }

        void OnOpenSolutionClick(object sender, RoutedEventArgs e)
        {
            TryExecute(() =>
            {
                using (new MessageFilter())
                    GetSolution();
            });
        }
        void OnCreateSolutionClick(object sender, RoutedEventArgs e)
        {
            TryExecute(() =>
            {
                using (new MessageFilter())
                {
                    var sln = (Solution4)_ide.Solution;

                    sln.Create(_solutionFolder, SolutionName);

                    // Shared Project
                    var sharedProjectTemplate = sln.GetProjectTemplate(@"Store Apps\Universal Apps\1033\SharedEmptyMaster\SharedEmptyMaster.vstemplate", "CSharp");
                    var sharedProjectPath = Path.Combine(_solutionFolder, SharedProjectName + @"\");
                    sln.AddFromTemplate(sharedProjectTemplate, sharedProjectPath, SharedProjectName, false);

                    var sharedProject = sln.Projects.Item(1);
                    var sharedProjectNamespace = sharedProject.Properties.Cast<Property>().Single(p => p.Name == "RootNamespace");
                    sharedProjectNamespace.Value = SharedProjectName;

                    // Monogame Project
                    var monogameProjectTemplate = sln.GetProjectTemplate(@"MonoGame\Windows.zip", "CSharp");
                    const string monogameProjectName = "MonogameProject";
                    var monogameProjectPath = Path.Combine(_solutionFolder, monogameProjectName + @"\");
                    sln.AddFromTemplate(monogameProjectTemplate, monogameProjectPath, monogameProjectName, false);

                    // Save
                    _ide.ExecuteCommand("File.SaveAll");

                    MessageBox.Show("Solution Created");
                }
            });
        }
        void OnShowSolutionInfoClick(object sender, RoutedEventArgs e)
        {
            TryExecute(() =>
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
            });
        }
        void OnAddClassClick(object sender, RoutedEventArgs e)
        {
            TryExecute(() =>
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
            });
        }
        void OnShowVisualStudioClick(object sender, RoutedEventArgs e)
        {
            TryExecute(() =>
            {
                using (new MessageFilter())
                    _ide.UserControl = true;
            });
        }
        void OnAddSampleClassClick(object sender, RoutedEventArgs e)
        {
            TryExecute(() =>
            {
                using (new MessageFilter())
                {
                    var sln = GetSolution();
                    var sharedProject = sln.Projects.Item(1);

                    #region Add Class via CodeModel
                    
                    //var csItemTemplatePath = sln.GetProjectItemTemplate("CodeFile", "CSharp");
                    //sharedProject.ProjectItems.AddFromTemplate(csItemTemplatePath, "GameObject.cs");

                    // Generate Class content
                    //var testClass = sharedProject.ProjectItems.Item("GameObject.cs");
                    //var similarProjectItems = SolutionHelper.GetSimilarProjectItems((Solution)sln, testClass);
                    //var fileCodeModel = (FileCodeModel2)similarProjectItems.Select(x => x.FileCodeModel).First(y => y != null);

                    //fileCodeModel.AddImport("Microsoft.Xna.Framework");

                    //var @namespace = fileCodeModel.AddNamespace(SharedProjectName);
                    //var @class = (CodeClass2)@namespace.AddClass("GameObject");

                    //var function = (CodeFunction2)@class.AddFunction("Update", vsCMFunction.vsCMFunctionFunction, "void", Access: vsCMAccess.vsCMAccessPublic);
                    //function.AddParameter("gameTime", "GameTime");

                    //// https://social.msdn.microsoft.com/Forums/vstudio/pt-BR/faad34d5-e046-494c-81c3-7b34483c3a7f/adding-autoimplemented-property-using-codeclass2?forum=vsx
                    ////var property = (CodeProperty2)@class.AddProperty("Position", "Position", "Vector2", -1, vsCMAccess.vsCMAccessPublic, null);

                    //testClass.Save();

                    #endregion

                    #region Add class via text editing

                    var builder = new StringBuilder();
                    builder.AppendLine("using Microsoft.Xna.Framework;");
                    builder.AppendLine();
                    builder.AppendLine("// ReSharper disable once CheckNamespace");
                    builder.AppendLine("namespace " + SharedProjectName);
                    builder.AppendLine("{");
                    builder.AppendLine("    public class GameObject");
                    builder.AppendLine("    {");
                    builder.AppendLine("        public Vector2 Location { get; set; }");
                    builder.AppendLine("        ");
                    builder.AppendLine("        public void Update(GameTime gameTime)");
                    builder.AppendLine("        {");
                    builder.AppendLine("        }");
                    builder.AppendLine("    }");
                    builder.Append("}");

                    var csItemTemplatePath = sln.GetProjectItemTemplate("CodeFile", "CSharp");
                    sharedProject.ProjectItems.AddFromTemplate(csItemTemplatePath, "GameObject.cs");
                    
                    var document = (TextDocument)_ide.ActiveDocument.Object("TextDocument");
                    var edit = (EditPoint2)document.StartPoint.CreateEditPoint();
                    edit.Insert(builder.ToString());

                    _ide.ExecuteCommand("File.SaveAll");

                    #endregion
                }
            });
        }

        Solution4 GetSolution()
        {
            if (!_ide.Solution.IsOpen)
                _ide.Solution.Open(Path.Combine(_solutionFolder, SolutionName + ".sln"));

            return (Solution4)_ide.Solution;
        }
        void InitializeVisualStudio()
        {
            TryExecute(() =>
            {
                var type = Type.GetTypeFromProgID("VisualStudio.DTE.12.0", true);
                _ide = (DTE)Activator.CreateInstance(type, true);

                _ideEvents = _ide.Events.DTEEvents;
                _ideEvents.OnBeginShutdown += InitializeVisualStudio;
            });
        }
        void CloseVisualStudio()
        {
            TryExecute(_ide.Quit);
        }

        private static void TryExecute(Action action)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}

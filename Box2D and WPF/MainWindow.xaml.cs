using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using  System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Box2DX.Collision;
using Box2DX.Common;
using System.Diagnostics;
using System.Reflection;

namespace Box2D_and_WPF
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public static Physics px;
		private MyModel3D p;
		private MyModel3D ground;
		public static bool isJump = false;
		private Solver solver;
		
		//public static TextBlock console = new TextBlock();
		public static string user_input = "";
		public static float freq = 1000.0f;
		const string writePath = @"engine.log";
		private const string PATH_TEX = @"\\Assets\level.png";
		//TaskWindow taskWindow = new TaskWindow();

		private MainViewModel MainViewModel { get; set; }
		public MainWindow()
		{
			InitializeComponent();
			Assembly assembly = Assembly.LoadFrom("Box2DX.dll");
			Trace.WriteLine(assembly.GetName().Name+" "+assembly.GetName().Version);
			MainViewModel = new MainViewModel();
			DataContext = MainViewModel;
			LoadConfig("user.cfg");
			/*Grid.SetColumnSpan(console, 2);
			Grid.SetRowSpan(console, 6);
			gridok.Children.Add(console);*/
			gridok.Background = new ImageBrush(new BitmapImage(new Uri($@"{Directory.GetCurrentDirectory()}{PATH_TEX}")));
			
			Trace.WriteLine($"Loading texture: {PATH_TEX}");
			px = new Physics(-1000, -1000, 1000, 1000, 0, -0.005f, false);
			px.SetModelsGroup(models);
			px.AddBox(-3, 0, 1,1, 0.5f, 0.3f, 0.2f,"wooden_box");
			p=px.AddBox(0, 0, 1,1, 0.5f, 0.3f, 0.2f,"rect");
			p.name = "player";
			ground = px.AddBox(-9, -2, 9, 0.25f, 0, 1.0f, 0.2f,"rect");
			ground.name = "ground";
			
			//plr_pos = player.GetPosition();
			this.LayoutUpdated += MainWindow_LayoutUpdated;
			//this.KeyDown += MainWindow_KeyDown;
			//console.Inlines.Add("Hello world!\n");
			//console.Inlines.Add($"{p.GetPosition().ToString()}\n");
			//console.Inlines.Add($"{px.world.GetBodyList().GetNext().GetPosition().Y}\n");

			//console.Inlines.Add($"{player.GetPosition().ToString()}\n");
			solver = new Solver();
			px.SetSolver(solver);
			solver.OnAdd += (model1, model2) =>
			{
				// Произошло столкновение тел model1 и model2
				if (model1.name == "player" && model2.name == "ground")
				{
					isJump = false;
					//console.Inlines.Add("Произошло столкновение тел model1 и model2\n");
					//console.Inlines.Add($"{p.GetPosition().ToString()}\n");
					//console.Inlines.Add($"{px.world.GetBodyList().GetNext().GetPosition().Y}\n");
				}
			};
		}
		private void MainWindow_LayoutUpdated(object sender, EventArgs e)
         		{
         			px.Step(freq, 10); // тут по хорошему нужно вычислять дельту времени, но мне лень :)
         			this.InvalidateArrange();
                    //plr_pos = player.GetPosition();
                    //player.SetPosition(plr_pos);
                }
	
		 private void PlayCommand_Click(object sender, RoutedEventArgs e)
        {
            //console.Text += "\n" + user_input; //Добовляем сообщение на панель сообщение с консоли
            string[] user_command = user_input.Split();
            switch (user_command[0])
            {
	            case "rs_fullscreen":
                     		            switch (user_command[1])
                     		            {
                     			            case "on":
                     				            this.WindowStyle = WindowStyle.None;
                     				            this.WindowState = WindowState.Maximized;
                     				            break;
                     			            case "off":
                     				            this.WindowStyle = WindowStyle.SingleBorderWindow;
                     				            this.WindowState = WindowState.Normal;
                     				            break;
                     		            }
		            break;
	            case "ps_frequency":
		            freq = float.Parse(user_command[1]);
		            break;
	            
	            case "bind":
		            KeyBinding OpenCmdKeyBinding = new KeyBinding();
			            OpenCmdKeyBinding.Key = (Key) new KeyConverter().ConvertFromString(user_command[2]);
			            OpenCmdKeyBinding.Command =
				            (ICommand) typeof(MainViewModel).GetProperty(user_command[1]).GetValue(null);
			            //(ICommand)typeof(MainViewModel).GetProperty(user_command[2]).GetValue(null)
			            //(Key)new KeyGestureConverter().ConvertFromString(user_command[1])
			            this.InputBindings.Add(OpenCmdKeyBinding);
			            break;
	            case "unbind":
		            foreach (KeyBinding i in this.InputBindings)
		            {
			            if (i.Key == (Key) new KeyConverter().ConvertFromString(user_command[1]))
			            {
				            i.Key = Key.None;
			            }
		            }
		            break;
	            case "unbindall":
		            this.InputBindings.Clear();
		            break;
            }
        }
		 private void LoadConfig(string path)
		 {
			 Trace.WriteLine($"Executing script \"{path}\"...");
			 using (StreamReader sr = new StreamReader($"{Directory.GetCurrentDirectory()}\\{path}", System.Text.Encoding.Default))
			 {
				 string line;
				 while ((line = sr.ReadLine()) != null)
				 {
					 user_input = line;
					 PlayCommand_Click(null, null);
				 }
			 }
		 }
		
	}
}

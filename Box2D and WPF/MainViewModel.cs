using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Box2DX.Common;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Box2D_and_WPF
{
    public class MainViewModel
    {
        private string PATH_TO_NEW_FILE_IMAGE = "sc-" + DateTime.Now.ToString("ddMMyyyy-hhmmss") + ".png";

        //public static Dictionary<string, ICommand> bind = new Dictionary<string, ICommand>();
        public MainViewModel()
        {
            //HelloCommand = new RelayCommand(_ => Hello());
            screenshot = new RelayCommand(_ => CreateScreenShot());
            jump = new RelayCommand(_ => Jump());
            right = new RelayCommand(_ => Right());
            left = new RelayCommand(_ => Left());
            //clear = new RelayCommand(_ => Clear());
            exit = new RelayCommand(_ => { System.Windows.Application.Current.Shutdown();});
            
        }

        //public ICommand HelloCommand { get; }
        public static ICommand screenshot { get; set; } 
        public static ICommand jump { get;  set;}
        public  static ICommand right { get;  set;}
        public  static ICommand left { get;  set;}
        public  static ICommand clear { get;  set;}
        public  static ICommand exit { get;  set;}
        private void Jump()
        {
            if (MainWindow.isJump==false)
            {
                MainWindow.isJump = true;
                MainWindow.px.world.GetBodyList().GetNext().ApplyForce(new Vec2(0,50),    MainWindow.px.world.GetBodyList().GetNext().GetWorldCenter());
            }
        }
        private void Right()
        {
            MainWindow.px.world.GetBodyList().GetNext().ApplyForce(new Vec2(5,0),    MainWindow.px.world.GetBodyList().GetNext().GetWorldCenter());
        }

        private void Left()
        {
            MainWindow.px.world.GetBodyList().GetNext().ApplyForce(new Vec2(-5, 0), MainWindow.px.world.GetBodyList().GetNext().GetWorldCenter());
        }
        private void Clear()
        {
            //MainWindow.console.Text = "";
            //MainWindow.user_input = "";
        }

        
        private void CreateScreenShot()
        {
            var bounds = System.Windows.Forms.Screen.GetWorkingArea(System.Drawing.Point.Empty);
            using (var bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (var g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(System.Drawing.Point.Empty, System.Drawing.Point.Empty, bounds.Size);
                }
                bitmap.Save(PATH_TO_NEW_FILE_IMAGE, ImageFormat.Png);
            }
        }
        
    }
}
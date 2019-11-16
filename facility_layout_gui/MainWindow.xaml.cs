using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Wpf;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Threading;


namespace WpfApp2
{

    public partial class MainWindow : Window
    {
        [DllImport("kernel.so", EntryPoint = "allib_load", CallingConvention = CallingConvention.Cdecl)]
        public static extern void allib_load();
        [DllImport("kernel.so", EntryPoint = "allib_init", CallingConvention = CallingConvention.Cdecl)]
        public static extern void allib_init();
        [DllImport("kernel.so", EntryPoint = "allib_evolve_n", CallingConvention = CallingConvention.Cdecl)]
        public static extern void allib_evolve_n(uint a);

        [DllImport("kernel.so", EntryPoint = "allib_info", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr allib_info();




        public MainWindow()
        {

            InitializeComponent();

            LineSeries mySeries = new LineSeries { Values = new ChartValues<int> { } };
            myChart.Series.Add(mySeries);
        }


        int[] space_data = new int[2];
        
        double[][] blocks = new double[100][]; //each block size for width and height
        double ratio;
        bool Flag = false;
        LineSeries mySeries = new LineSeries { Values = new ChartValues<int> { } };

        #region btnForExe
        private void Restartbutton_Click(object sender, RoutedEventArgs e)
        {
            allib_init();

            string solution;
            uint run = (uint)Convert.ToInt32(nfe.Text);
            canvas.Children.Clear();
            for (int i = 0; i < run; i++)
            {
                allib_evolve_n(1);
                solution = Marshal.PtrToStringAnsi(allib_info());
                Console.WriteLine(solution);
                Dispatcher.Invoke(new Action(() => { }), DispatcherPriority.ContextIdle, null);
                canvas.Children.Clear();
                DrawGraphs(solution, mySeries, i);
            }

            Dispatcher.Invoke(new Action(() => { }), DispatcherPriority.Background, null);
            output_box.Text += "Done!\n";
            myScroll.ScrollToEnd();
        }

        private void EXEbutton_Click(object sender, RoutedEventArgs e)
        {
            
            if (Flag == false)
            { 
                allib_load();
                allib_init();

                int angle = 0;

                try
                {
                    int check = 0;
                    if (pop_size.Text == "")
                    {
                        check++;
                        output_box.Text += " 請輸入pop size\n";
                        myScroll.ScrollToEnd();
                    }
                    if (nfe.Text == "") {
                        check++;
                        output_box.Text += " 請輸入generation\n";
                        myScroll.ScrollToEnd();
                    }
                    if (alpha.Text == "")
                    {
                        check++;
                        output_box.Text += " 請輸入alpha\n";
                        myScroll.ScrollToEnd();
                    }
                    if (beta.Text == "")
                    {
                        check++;
                        output_box.Text += " 請輸入beta\n";
                        myScroll.ScrollToEnd();
                    }
                    if (gamma.Text == "")
                    {
                        check++;
                        output_box.Text += " 請輸入gamma\n";
                        myScroll.ScrollToEnd();
                    }
                    if (check != 0)
                        return;

                    string write_para = "";
                    write_para += "@popsize:" + pop_size.Text + "\n";
                    write_para += "@gen:" + nfe.Text + "\n";
                    write_para += "@alpha:" + alpha.Text + "\n";
                    write_para += "@beta:" + beta.Text + "\n";
                    write_para += "@gamma:" + gamma.Text + "\n";

                    //string[] stringArray = new string[] { write_para };
                    //System.IO.File.WriteAllLines(@"C:\Users\Yu\WriteLines.txt", stringArray);


                    int item = 0;
                    using (System.IO.StreamReader SR = new System.IO.StreamReader(@".\meta\space.csv"))
                    {
                        string Line;
                        while ((Line = SR.ReadLine()) != null)
                        {
                            item++;
                            string[] ReadLine_Array = Line.Split(',');
                            try
                            {
                                if (item == 3)
                                {
                                    Int32.TryParse(ReadLine_Array[0], out space_data[0]);
                                    Int32.TryParse(ReadLine_Array[1], out space_data[1]);
                                    continue;
                                }
                                else if (item == 5)
                                {
                                    Int32.TryParse(ReadLine_Array[0], out angle);
                                }
                            }
                            catch (System.IndexOutOfRangeException) { }
                        }
                    }
                }
                catch (System.IO.IOException) { }
                catch (NullReferenceException) { }
                catch (FormatException) { }
                double ratio_width;
                double ratio_height;

                ratio_width = canvas.ActualWidth / space_data[0];
                ratio_height = canvas.ActualHeight / space_data[1];
                double A = testH.ActualHeight;
                double B = downH.ActualHeight;

                double width_star, height_star;
                if (ratio_height > ratio_width)
                    ratio = ratio_width;
                else
                    ratio = ratio_height;

                width_star = ((canvas.ActualWidth - (ratio * space_data[0])) / 2) / space_data[0];
                height_star = ((canvas.ActualHeight - (ratio * space_data[1])) / 2) / space_data[1];

                if (width_star < 0) width_star = 0;
                if (height_star < 0) height_star = 0;

                control_grid.RowDefinitions[0].Height = new GridLength(height_star, GridUnitType.Star);
                control_grid.RowDefinitions[2].Height = new GridLength(height_star, GridUnitType.Star);
                control_grid.ColumnDefinitions[0].Width = new GridLength(width_star, GridUnitType.Star);
                control_grid.ColumnDefinitions[2].Width = new GridLength(width_star, GridUnitType.Star);




                for (int i = 0; i < 100; i++)
                    blocks[i] = new double[5];
                int block_num = 0;
                try
                {
                    using (System.IO.StreamReader SR = new System.IO.StreamReader(@".\meta\blocks.csv"))
                    {
                        string Line;
                        while ((Line = SR.ReadLine()) != null)
                        {
                            string[] ReadLine_Array = Line.Split(',');
                            try
                            {
                                for (int j = 0; j < 5; j++)
                                    if (j != 0)
                                        Double.TryParse(ReadLine_Array[j], out blocks[block_num][j - 1]);
                                block_num++;
                            }
                            catch (System.IndexOutOfRangeException) { }
                        }
                    }
                }
                catch (System.IO.IOException) { }
                catch (NullReferenceException) { }
                catch (FormatException) { }

                var line = new Line()
                {
                    X1 = 0,
                    Y1 = 0,
                    X2 = 100,
                    Y2 = 0,
                    RenderTransform = new RotateTransform(-angle, 0, 0),
                    Stroke = Brushes.Green,
                    StrokeThickness = 1,
                };
                Canvas.SetBottom(line, 0);
                Canvas.SetLeft(line, 0);
                canvas.Children.Add(line);

                

                mySeries = new LineSeries
                {
                    Values = new ChartValues<double> { },
                    LineSmoothness = 0,
                    PointForeground = Brushes.White,
                    PointGeometry = null,
                    Title = "fitness",
                };
                myChart.Series.Add(mySeries);
                Flag = true;
            }

            string solution;
            uint run = (uint)Convert.ToInt32(nfe.Text);
            canvas.Children.Clear();
            for (int i = 0; i < run; i++)
            {
                allib_evolve_n(1);
                solution = Marshal.PtrToStringAnsi(allib_info());
                Console.WriteLine(solution);
                Dispatcher.Invoke(new Action(() => { }), DispatcherPriority.ContextIdle, null);
                canvas.Children.Clear();
                DrawGraphs(solution, mySeries, i);
            }

            Dispatcher.Invoke(new Action(() => { }), DispatcherPriority.Background, null);
            output_box.Text += "Done!\n";
            myScroll.ScrollToEnd();

        }
        #endregion

        public void DrawGraphs(String solution, LineSeries mySeries, int times)
        {                
            string[] solutions = solution.Split(' ');
            string[] s = solutions[1].ToString().Split('\n');

            Console.WriteLine(s[0]);
            double fitness = Convert.ToDouble(s[0]);

            mySeries.Values.Add(fitness);
            

            for (int i = 2; i < 12; i++)   //number need to be chande.
            {
                var grid = new Grid();
                double x_axis = Convert.ToDouble(solutions[i]);
                double y_axis = Convert.ToDouble(solutions[10 * 1 + i]);
                double is_rotate = Convert.ToDouble(solutions[10 * 2 + i]);

                x_axis *= 500;
                y_axis *= 430;

                //x_axis *= ratio;
                //y_axis *= ratio;

                x_axis *= 1.5;
                y_axis *= 1.5;

                if (is_rotate == 1)
                {
                    double tmp = x_axis;
                    x_axis = y_axis;
                    y_axis = x_axis;
                }
                    
                grid.Children.Add(new TextBlock()
                {
                    Text = (i-2).ToString(),
                    Foreground = Brushes.Black,
                    FontWeight = FontWeights.Light,
                });

                grid.Children.Add(new Rectangle()
                {
                    Width = blocks[i-2][0] * ratio,// init_rect_ratio_width,
                    Height = blocks[i-2][1] * ratio,// init_rect_ratio_height,
                    Stroke = Brushes.Blue,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    HorizontalAlignment = HorizontalAlignment.Left,
                });

                Canvas.SetLeft(grid, x_axis); //x-axis
                Canvas.SetBottom(grid, y_axis); //y-axis
                
                canvas.Children.Add(grid);
            }
            Dispatcher.Invoke(new Action(() => { }), DispatcherPriority.ContextIdle, null);
            //Thread.Sleep(3);


        }


        #region SizeReChanged
        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            if (e.PreviousSize.Width == 0) return;
            Canvas canvas = sender as Canvas;
            SizeChangedEventArgs canvas_Changed_Args = e;

            double old_Height = canvas_Changed_Args.PreviousSize.Height;
            double new_Height = canvas_Changed_Args.NewSize.Height;
            double old_Width = canvas_Changed_Args.PreviousSize.Width;
            double new_Width = canvas_Changed_Args.NewSize.Width;

            foreach (FrameworkElement grid_element in canvas.Children)
            {
                if (grid_element.GetType() == typeof(Grid))
                {
                    if (VisualTreeHelper.GetChild(grid_element, 0).GetType() == typeof(Line))
                        continue;
                    Rectangle rect_element;
                    rect_element = (Rectangle)VisualTreeHelper.GetChild(grid_element, 1); //get rectangle element
                    double old_Left = Canvas.GetLeft(grid_element);
                    double old_Bottom = Canvas.GetBottom(grid_element);
    
                    double obj_Height = rect_element.Height;
                    double obj_Width = rect_element.Width;

                    // < set Left-Bottom>
                    Canvas.SetLeft(grid_element, (new_Width * old_Left) / old_Width);
                    Canvas.SetBottom(grid_element, (new_Height * old_Bottom) / old_Height);

                    //< set Width-Heigth >
                    rect_element.Width = (new_Width * obj_Width) / old_Width;
                    rect_element.Height = (new_Height * obj_Height) / old_Height;
                }
            }

            //space_data[0] = 1;
            //control_grid.RowDefinitions[0].Height = new GridLength(height_star, GridUnitType.Star);
            //control_grid.RowDefinitions[2].Height = new GridLength(height_star, GridUnitType.Star);
            //control_grid.ColumnDefinitions[0].Width = new GridLength(width_star, GridUnitType.Star);
            //control_grid.ColumnDefinitions[2].Width = new GridLength(width_star, GridUnitType.Star);
        }

        #endregion

        #region MouseMoveRectangle
        bool isPressed = false;
        Point startPosition;
        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Rectangle)
            {
                Rectangle ClickedRectangle = (Rectangle)e.OriginalSource;
                ClickedRectangle.Opacity = 0.5;

                isPressed = true;
                startPosition = e.GetPosition(canvas);
                ClickedRectangle.CaptureMouse();

            }
            if (e.OriginalSource is TextBlock)
            {
                FrameworkElement grid_element, ClickedRectangle;
                grid_element = (FrameworkElement)VisualTreeHelper.GetParent((FrameworkElement)e.OriginalSource);
                ClickedRectangle = (FrameworkElement)VisualTreeHelper.GetChild(grid_element, 1);
                ClickedRectangle.Opacity = 0.5;

                isPressed = true;
                startPosition = e.GetPosition(canvas);
                ClickedRectangle.CaptureMouse();

            }
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Rectangle)
            {
                Rectangle ClickedRectangle = (Rectangle)e.OriginalSource;
                ClickedRectangle.Opacity = 1;
                ClickedRectangle.ReleaseMouseCapture();
                isPressed = false;
                foreach (FrameworkElement grid_element in canvas.Children)
                {
                    if (grid_element.GetType() == typeof(Line))
                        continue;
                    ((Rectangle)VisualTreeHelper.GetChild(grid_element, 1)).StrokeThickness = 1; //get rectangle element
                    ((Rectangle)VisualTreeHelper.GetChild(grid_element, 1)).Stroke = new SolidColorBrush(Colors.Blue);
                }
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.OriginalSource is Rectangle)
            {
                Grid clicked_grid_obj;
                Rectangle ClickedRectangle = (Rectangle)e.OriginalSource;
                clicked_grid_obj = (Grid)VisualTreeHelper.GetParent(ClickedRectangle);
                if (isPressed)
                {
                    Point position = e.GetPosition(canvas);
                    double daltaY = position.Y - startPosition.Y;
                    double daltaX = position.X - startPosition.X;
                    Canvas.SetBottom(clicked_grid_obj, Canvas.GetBottom(clicked_grid_obj) - daltaY);
                    Canvas.SetLeft(clicked_grid_obj, Canvas.GetLeft(clicked_grid_obj) + daltaX);
                    startPosition = position;

                    string click_box_id = ((TextBlock)VisualTreeHelper.GetChild(clicked_grid_obj, 0)).Text;
                    Rectangle click_rect_element = (Rectangle)VisualTreeHelper.GetChild(clicked_grid_obj, 1);
                    foreach (FrameworkElement grid_element in canvas.Children)
                    {
                        if (grid_element.GetType() == typeof(Line))
                            continue;
                        string each_id = ((TextBlock)VisualTreeHelper.GetChild(grid_element, 0)).Text;
                        if (click_box_id == each_id)
                            continue;
                        FrameworkElement rect_element;
                        rect_element = (FrameworkElement)VisualTreeHelper.GetChild(grid_element, 1); //get rectangle element
                        double old_Left = Canvas.GetLeft(grid_element);
                        double old_Bottom = Canvas.GetBottom(grid_element);
                        double obj_Height = rect_element.Height;
                        double obj_Width = rect_element.Width;


                        if ((int)Canvas.GetBottom(clicked_grid_obj) == (int)old_Bottom || (int)(Canvas.GetBottom(clicked_grid_obj) + click_rect_element.Height) == (int)(old_Bottom + obj_Height) || (int)Canvas.GetLeft(clicked_grid_obj) == (int)old_Left || (int)(Canvas.GetLeft(clicked_grid_obj) + click_rect_element.Width) == (int)(old_Left + obj_Width)) // cast to int because double is default, but double will too accuracy to align for user.
                        {
                            ((Rectangle)rect_element).StrokeThickness = 4;
                            ((Rectangle)rect_element).Stroke = new SolidColorBrush(Colors.Red);
                        }
                        else
                        {
                            ((Rectangle)rect_element).StrokeThickness = 1;
                            ((Rectangle)rect_element).Stroke = new SolidColorBrush(Colors.Blue);
                        }
                    }
                }
            }
        }


        #endregion
    }
}





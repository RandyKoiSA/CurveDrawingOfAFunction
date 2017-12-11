/* Author: Randy Le
 * Author's Email: Randy.l5933@csu.fullerton.edu
 * Course: CPSC 223N
 * 
 * Source Files:
 * 1.curvedrawing_3main.cs
 * 2.curvedrawing_3frame.cs
 * 3.curvedarwing_3logic.cs
 * 4.run.sh
 * 
 * Pupose of this entire program:
 * Tracing and following the function : cos(2t). Having to change polar coordinatess to cartesian coordinates
 * Compile this file:
 * mcs target:library curvedrawing_3frame.cs -r:System.Windows.Forms.dll -r:System.Drawing.dll -r:curvedrawing_3logic.dll -out:curvedrawing_3frame.dll
 */
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Timers;

public class curvedrawing_3frame : Form{
 
    private const int form_height = 900;
    private const int control_region_height = 100;
    private const int graphic_area_height = form_height - control_region_height;
    private const int form_width = graphic_area_height * 16 / 9;
    private const int horizontal_adjustment = 8;

    private Button go_button = new Button();
    private Button pause_button = new Button();
    private Button exit_button = new Button();

    private Point go_button_location = new Point(20, form_height - control_region_height + 20);
    private Point pause_button_location = new Point(250, form_height - control_region_height + 20);
    private Point exit_button_location = new Point(1200, form_height - control_region_height + 20);

    private Size maxframesize = new Size(form_width, form_height);
    private Size minframesize = new Size(form_width, form_height);
    //**************************************************************************
    Pen bic = new Pen(Color.Black, 1);

    //Declare constnats
    private const double scale_factor = 100.0;
    private const double linear_velocity = 45;
    private const double refresh_rate = 90.0; //Hz: times per sec the bitmap is updated and copied to graphic
    private const double dot_update_rate = 70.0; //Hz: times per sec the coordinates of the dot are updates

    private const double pixel_distance_traveled_in_one_tic = linear_velocity / dot_update_rate;
    private const double mathematical_distance_traveled_in_one_tic = pixel_distance_traveled_in_one_tic / scale_factor;

    //Declare the polar origin in C# coordinates
    private int polar_origen_x = (int)System.Math.Round(form_width / 2.0);
    private int polar_origen_y = (int)System.Math.Round(graphic_area_height / 2.0);

    //Variables for the Cartesian description of 
    private const double t_initial_value = 0.0; //Start curve drawing at y(0.0) = (0.0,0.0) = Origin
    private double t = t_initial_value;
    private double x;
    private double y;

    //Variables for the scaled description of the curve function
    private int x_scaled_int;
    private int y_scaled_int;
    private double x_scaled_double;
    private double y_scaled_double;

    //Declare clocks
    private static System.Timers.Timer graphic_area_refresh_clock = new System.Timers.Timer();
    private static System.Timers.Timer dot_refresh_clock = new System.Timers.Timer();

    //Declare visible graphical area and bitmap graphical area
    private System.Drawing.Graphics pointer_to_graphic_surface;
    private System.Drawing.Bitmap pointer_to_bitmap_in_memory =
                      new Bitmap(form_width, graphic_area_height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

    //Declare a tool kit of algorithms
    curvedrawing_3logic algorithm;


    public curvedrawing_3frame(){
        Size = new Size(form_width, form_height);
        BackColor = Color.LightGray;
        Text = "Curve Drawing of Cos(2t) by Randy Le";
        MaximumSize = maxframesize;
        MinimumSize = minframesize;
        DoubleBuffered = true;

        go_button.Text = "Go";
        go_button.BackColor = Color.Yellow;
        go_button.Location = go_button_location;
        go_button.Size = new Size(100, 32);
        pause_button.Text = "Pause";
        pause_button.BackColor = Color.Yellow;
        pause_button.Location = pause_button_location;
        pause_button.Size = new Size(100, 32);
        exit_button.Text = "Exit";
        exit_button.BackColor = Color.Yellow;
        exit_button.Location = exit_button_location;
        exit_button.Size = new Size(100, 32);

        Controls.Add(go_button);
        Controls.Add(pause_button);
        Controls.Add(exit_button);

        go_button.Click += new EventHandler(manage_go_button);
        pause_button.Click += new EventHandler(manage_pause_button);
        exit_button.Click += new EventHandler(closeprogram);

        //**********************************************************************
        algorithm = new curvedrawing_3logic();
        //Set initial values for the curve in standard mathermatical cartesian coordiantes system;
        t = t_initial_value;
        x = System.Math.Cos(2 * t) * System.Math.Cos(t);
        y = System.Math.Cos(2 * t) * System.Math.Sin(t);

        //prepare the refresh clock
        graphic_area_refresh_clock.Enabled = false;
        graphic_area_refresh_clock.Elapsed += new ElapsedEventHandler(Update_the_graphical_area);
        //prepare the dot clock
        dot_refresh_clock.Enabled = false;
        dot_refresh_clock.Elapsed += new ElapsedEventHandler(Update_the_position_of_the_dot);

        //Start both clocks running

        //Initialize the pointer used to write onto the bitmap stored in memory
        pointer_to_graphic_surface = Graphics.FromImage(pointer_to_bitmap_in_memory);
        initialize_bitmap();
    }
    protected void initialize_bitmap(){
        bic.DashStyle = DashStyle.Dash;
        pointer_to_graphic_surface.Clear(System.Drawing.Color.White);
		pointer_to_graphic_surface.DrawLine(bic, form_width / 2, graphic_area_height / 2, form_width / 2, 0);
		pointer_to_graphic_surface.DrawLine(bic, form_width / 2, graphic_area_height / 2, form_width / 2, graphic_area_height);
		pointer_to_graphic_surface.DrawLine(bic, form_width / 2, graphic_area_height / 2, 0, graphic_area_height / 2);
		pointer_to_graphic_surface.DrawLine(bic, form_width / 2, graphic_area_height / 2, form_width, graphic_area_height / 2);

    }
    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics board = e.Graphics;
        board.FillRectangle(Brushes.Green, 0, form_height - control_region_height, form_width, form_height);
        board.DrawImage(pointer_to_bitmap_in_memory, 0, 0, form_width, graphic_area_height);
        base.OnPaint(e);
    }
    protected void manage_go_button(Object sender, EventArgs events){
        double elapsed_time_between_updates_of_coordinates;
        double local_update_rate = dot_update_rate;
        if(local_update_rate < 1.0){
            local_update_rate = 1.0;
        }
        elapsed_time_between_updates_of_coordinates = 1000.0 / local_update_rate;
        dot_refresh_clock.Interval = (int)System.Math.Round(elapsed_time_between_updates_of_coordinates);
        Start_graphic_clock(refresh_rate);
        Start_dot_clock(dot_update_rate);
    }
    protected void manage_pause_button(Object sender, EventArgs events){
        graphic_area_refresh_clock.Enabled = false;
        dot_refresh_clock.Enabled = false;
    }
    protected void closeprogram(Object sender, EventArgs events){
        System.Console.WriteLine("The curvedrawing program is now closign");
        Close();
    }
    //**************************************************************************
    protected void Start_graphic_clock(double refreshrate){
        double elapsedtimebetweentics;
        if (refreshrate < 1.0) refreshrate = 1.0;
        elapsedtimebetweentics = 1000.0 / refreshrate;
        graphic_area_refresh_clock.Interval = (int)System.Math.Round(elapsedtimebetweentics);
        graphic_area_refresh_clock.Enabled = true;
        System.Console.WriteLine("Start graphical clock has terminated");
        
    }
    protected void Start_dot_clock(double dot_parameter_update_rate){
        double elapsedtimebetweenchangestodotcoordinates;
        if (dot_parameter_update_rate < 1.0) dot_parameter_update_rate = 1.0;
        elapsedtimebetweenchangestodotcoordinates = 1000 / dot_parameter_update_rate;
        dot_refresh_clock.Interval = (int)System.Math.Round(elapsedtimebetweenchangestodotcoordinates);
        dot_refresh_clock.Enabled = true;
        System.Console.WriteLine("Start dotclock has now terminated");
    }

    //**************************************************************************
    protected void Update_the_graphical_area(Object sender, ElapsedEventArgs evts){
        x_scaled_int = (int)System.Math.Round(x_scaled_double);
        y_scaled_int = (int)System.Math.Round(y_scaled_double);
        pointer_to_graphic_surface.FillEllipse(Brushes.DarkRed, x_scaled_int, y_scaled_int, 1, 1);
        Invalidate();
    }
    protected void Update_the_position_of_the_dot(Object sender, ElapsedEventArgs evts){
        //get next coordinate function from algorithm here
        algorithm.get_next_coordinates(mathematical_distance_traveled_in_one_tic, ref t, out x, out y);
        x_scaled_double = scale_factor * x + form_width/2;
        y_scaled_double = scale_factor * y + graphic_area_height / 2;
    }
}
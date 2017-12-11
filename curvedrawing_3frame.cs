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

//=========1=========2=========3=========4=========5=========6=========7=========8=========9=========10========11========12========13========14========15
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Timers;

public class curvedrawing_3frame : Form{ //Entering curvedrawing_3frame class
 
    //Setting display and boundaries*********************************************************************************************************************
    private const int form_height = 900;                                            //Height of the full program
    private const int control_region_height = 100;                                  //Height of the control panel w/ all the button             
    private const int graphic_area_height = form_height - control_region_height;    //Height of the display of the program
    private const int form_width = graphic_area_height * 16 / 9;                    //Width of the full program
    private const int horizontal_adjustment = 8;                    

	private Size maxframesize = new Size(form_width, form_height);                  //Maximum / Minimum size of the program can be resized
	private Size minframesize = new Size(form_width, form_height);
    //***************************************************************************************************************************************************

    //Display Variables and Buttons**********************************************************************************************************************
    private Button go_button = new Button();            //The go button to start the trace of the function 
    private Button pause_button = new Button();         //The pause button to pause the trace of the function at current position
    private Button exit_button = new Button();          //The exit button that closes the whole program

    private Point go_button_location = new Point(20, form_height - control_region_height + 20);         //Location of where the go_button will go
    private Point pause_button_location = new Point(250, form_height - control_region_height + 20);     //Location of where the pause_button will go
    private Point exit_button_location = new Point(1200, form_height - control_region_height + 20);     //Location of where the exit_button will go

    //***************************************************************************************************************************************************

    //Declare Instruments********************************************************************************************************************************
    Pen bic = new Pen(Color.Black, 1);    //The pen that used to trace the function out
    //***************************************************************************************************************************************************

	//Declare constants**********************************************************************************************************************************
	//  -Declare the 3 important #'s here:linear speed, refresh rate, motion rate 
	//  -Declare the constant velocity. The motion of drawing the spiral must
	//   process at this peed at all times.
	private const double linear_velocity = 44.5;    //pixels per second. These are not math units; these are graphical pixel
    //  -Declare rates for the clocks
	private const double refresh_rate = 58.5;       //Hertz: How many times per second the bitmap is copied to the visible surface
	private const double dot_update_rate = 70.0;    //Hz: times per sec the coordinates of the dot are updates
    private const double scale_factor = 100.0;
	//***************************************************************************************************************************************************

	//***************************************************************************************************************************************************
	//Declare a name for the distance the drawing brush travels in one tic of the dot_clock. For example, if the velocity of the brush drawing that dot is
    //120 pixels/sec adn the clock tics at the rate of 30Hz, then the brush moves a distance of 120/30 = 4 pixels per tic
    private const double pixel_distance_traveled_in_one_tic = linear_velocity / dot_update_rate;
    //Convert pixel_distance to mathematical distance
    private const double mathematical_distance_traveled_in_one_tic = pixel_distance_traveled_in_one_tic / scale_factor;
	//***************************************************************************************************************************************************

	//*****************************************************************************************************************************
	//To future programmers: There is a delicate balance among the three quantities:                                              *
	//    linear velocity: the distance in pixels traveled by the brush drawing the spiral in one second                          *
	//    refresh rate  :  the number of times per second the graphic area is repainted                                           *
	//    spiral rate   :  the number of times per second the coordinates of the brush painting the spiral are updated per second.*
	//If any one of the 3 is extreme compared to the other two the result is a disconnected spiral with gaps scattered along the  *
	//spiral curve.  For instance, these values result in a disconnected spiral:                                                  *
	//    linear velocity = 105.0                                                                                                 *
	//    refresh rate = 24.0                                                                                                     *
	//    spiral rate  = 18.0                                                                                                     *
	//However, the values shown below are more 'evenly balanced', and the result is a smooth connected spiral.                    *
	//    linear velocity = 44.5                                                                                                  *
	//    refresh rate = 47.5                                                                                                     *
	//    spiral rate = 50.0                                                                                                      *
	//You will simply have to experiment with different values for the 3 quantities to find values that make a good viewing       *
	//experience.  Keep in mind that these values vary with power of the computer used to test the program.  A computer with a    *
	//more powerful CPU, video card, size of memory, etc will be able to handle a larger range of values than a lesser computer.  *
	//*****************************************************************************************************************************

	//Declare the polar origin in C# coordinates*********************************************************************************************************
	private int polar_origen_x = (int)System.Math.Round(form_width / 2.0);
    private int polar_origen_y = (int)System.Math.Round(graphic_area_height / 2.0);
	//***************************************************************************************************************************************************


	//Variables for the Cartesian description of 
	private const double t_initial_value = 0.0; //Start curve drawing at y(0.0) = (0.0,0.0) = Origin
    private double t = t_initial_value;
    private double x; 
    private double y;
	//***************************************************************************************************************************************************

	//Variables for the scaled description of the curve function
	private int x_scaled_int;
    private int y_scaled_int;
    private double x_scaled_double;
    private double y_scaled_double;
	//***************************************************************************************************************************************************

	//Declare clocks
	private static System.Timers.Timer graphic_area_refresh_clock = new System.Timers.Timer();
    private static System.Timers.Timer dot_refresh_clock = new System.Timers.Timer();
	//***************************************************************************************************************************************************

	//Declare visible graphical area and bitmap graphical area
	private System.Drawing.Graphics pointer_to_graphic_surface;
    private System.Drawing.Bitmap pointer_to_bitmap_in_memory =
                      new Bitmap(form_width, graphic_area_height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
	//***************************************************************************************************************************************************

	//Declare a tool kit of algorithms
	curvedrawing_3logic algorithm;
	//***************************************************************************************************************************************************

	//***************************************************************************************************************************************************
    public curvedrawing_3frame(){
        Size = new Size(form_width, form_height);                               //Set Size of the program
        BackColor = Color.LightGray;                                            //Set background color of the program
        Text = "Curve Drawing of Cos(2t) by Randy Le";                          //Set the title of the program
        MaximumSize = maxframesize;                                             //Set max/min of resizing the program
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

        //=========================================================================================
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
		//=========================================================================================

		//Initialize the pointer used to write onto the bitmap stored in memory
		pointer_to_graphic_surface = Graphics.FromImage(pointer_to_bitmap_in_memory);
        initialize_bitmap();
    }//End of constructor
	//***************************************************************************************************************************************************

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
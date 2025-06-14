using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace PracTest2
{
    public partial class Form1 : Form
    {
        //Name: 
        //ID:

        //The smallest Easting value on the NZMG260 S14 (Hamilton) map
        const int MIN_EASTING = 2690000;
        //The largest Easting value on the NZMG260 S14 (Hamilton) map
        const int MAX_EASTING = 2730000;
        //The smallest Northing value on the NZMG260 S14 (Hamilton) map
        const int MIN_NORTHING = 6370000;
        //The largest Northing value on the NZMG260 S14 (Hamilton) map
        const int MAX_NORTHING = 6400000;

        //Create Lists to store the data from listbox and csv files
        List<string> licenceeList= new List<string>();
        List<String> locationList= new List<String>();
        List<int> eastingList= new List<int>();
        List<int> northingList= new List<int>();
        List<double> powerList= new List<double>(); 

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Draws a cell tower centered at the given x and y position
        /// in the colour specified.
        /// </summary>
        /// <param name="paper">Where to draw the tower</param>
        /// <param name="x">X position of the centre of the tower</param>
        /// <param name="y">Y position of the centre of the tower</param>
        /// <param name="power">The range of the tower, i.e. the radius of the circle</param>
        /// <param name="towerColour">Colour to draw the tower in</param>
        private void DrawTower(Graphics paper, int x, int y, double power, Color towerColour)
        {
            //The size of a side of the rectangle to represent a tower
            const int TOWER_SIZE = 6;
            //Brush and pen to draw the tower in the given colour
            SolidBrush br = new SolidBrush(towerColour);
            Pen pen1 = new Pen(towerColour, 2);
            //Caluclate the radius of the circle to represent the power as an integer
            int radius = (int)power;

            //Draw the tower centered around the given x and y point
            paper.FillRectangle(br, x - TOWER_SIZE / 2, y - TOWER_SIZE / 2, TOWER_SIZE, TOWER_SIZE);
            //Draw the circle to represent the range cenetred around the given x and y point
            paper.DrawEllipse(pen1, x - radius, y - radius, radius * 2, radius * 2);
        }

        /// <summary>
        /// This method will calculate the correct x coordinate value of the cell tower
        /// based on the given easting value.
        /// </summary>
        /// <param name="easting">The easting value of the cell tower</param>
        /// <returns>The x coordinate of the cell tower in the picturebox.</returns>
        private int CalculateX(int easting)
        {
            //calculate x position of easting value, must cast one of the values to a double
            //otherwise will perform integer division
            double ratio = (double)(easting - MIN_EASTING) / (MAX_EASTING - MIN_EASTING);
            int x = (int)(ratio * pictureBoxMap.Width);
            return x;
        }
        /// <summary>
        /// Calculates position of the y axis based on the northing value  
        /// </summary>
        /// <param name="northing"> The northing value of the cell tower </param>
        /// <returns> Y Coordinate of the cell tower in the picturebox </returns> 
        private int CalculateY(int northing)
        {
            //calculate y position of easting value, must cast one of the values to a double
            //otherwise will perform integer division
            double ratio = (double)(northing - MIN_NORTHING) / (MAX_NORTHING - MIN_NORTHING);
            int y = pictureBoxMap.Height -(int)(ratio * pictureBoxMap.Height);
            return y;
        }
        /// <summary>
        /// Method to return the number of towers less than or equal to a user input parameter
        /// </summary>
        /// <param name="powerInput">User input parameter to be compared</param>
        /// <returns> Number of Towers that are less than or equal to powerInput Value </returns>
        private int CountTowers(double powerInput)
        {
            int numTowers = 0;
            foreach (double powerValue in powerList )
            {
                if (powerInput >= powerValue)
                {
                    numTowers++;
                }
            }

            return numTowers;
        }
        /// <summary>
        /// Opens the .Csv File, adds the data to the Listbox. Draws the cell towers onto the map. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //set up a constant FILTER
            const string FILTER = "CSV Files|*.csv|All Files|*.*";
            //create a reader object
            StreamReader reader;
            //declare variables 
            string line = "";
            string[] csvArray;
            string licencee = "";
            string location = "";
            int easting = 0;
            int northing = 0;
            double power = 0;
            //create a graphics objects so we can draw onto the map
            Graphics paper = pictureBoxMap.CreateGraphics();
            int y = 0;
            int x = 0;


            //set the filter for dialogue control
            openFileDialog1.Filter = FILTER;

            //check to see if the user has selected a file to open
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //open the selected file
                reader = File.OpenText(openFileDialog1.FileName);

               
                while (!reader.EndOfStream)
                    try
                    {
                        //Read a line from the file
                        line = reader.ReadLine();
                        //Split the values Using an Array
                        csvArray= line.Split(',');

                        //check for the correct number of variables
                        if(csvArray.Length == 5) 
                        {
                            //Extract csvArray Values into the correct datatype
                            licencee = (csvArray[0]);
                            location = (csvArray[1]);
                            easting = int.Parse(csvArray[2]);
                            northing = int.Parse(csvArray[3]);
                            power = double.Parse(csvArray[4]);
                            //display the values into listbox
                            listBoxData.Items.Add
                                (licencee.ToString().PadRight(19)+
                                location.ToString().PadRight(31)+
                                easting.ToString().PadRight(10)+
                                northing.ToString().PadRight(10)+
                                power.ToString());

                            //Draw the Towers Using the Method Provided
                            //First Calculate x pos and y pos by calling the methods
                            y = CalculateY(northing);
                            x = CalculateX(easting);
                            //Call Draw Towers Method
                            DrawTower(paper, x, y, power, Color.Black);

                            //Add the Data into the Class level Lists by using the reader loop
                            licenceeList.Add(licencee);
                            locationList.Add(location); 
                            eastingList.Add(easting);
                            northingList.Add(northing);
                            powerList.Add(power);
                        }
                        else
                        {
                            //Write bad Data line information to the console window
                            Console.WriteLine("Bad Data in Line " + line);
                        }

                    }
                    catch(Exception ex)
                    { 
                    MessageBox.Show(ex.Message);
                    }
                //Close the file
                reader.Close();

            }
        }
        /// <summary>
        /// Ends the Application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        /// <summary>
        /// Menu event to diaplay the number of towers less than a user input parameter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void countTowersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (powerList.Count==0)
            {
                MessageBox.Show("Please open a valid file");
                    return;
            }
            try
            {
                double inputPower = double.Parse(textBoxPowerValue.Text);

                int numTowers = CountTowers(inputPower);

                MessageBox.Show("The number of Towers less than or equal to the Power Value is: " + numTowers);
            }
            catch (Exception ex)
            { 
                MessageBox.Show(ex.Message);
            }
            

        }
    }
}

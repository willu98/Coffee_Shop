//Author: Sam Zheng
//Created Date: 2015/12/7
//Modified Date: 2015/12/14
//File Name: View.cs
//Project Name: A5_Simulation
//Description: This class resoponsible for all the visuial part of the simulation

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace CofeeShop
{
    class View
    {
        ///////ATTRIBUTE//////
        //sprite font related variable
        private SpriteFont regularFont;
        private SpriteFont subTitleFont;
        private SpriteFont smallerFont;

        //texture 2D related variable
        private Texture2D cashierImg;
        private Texture2D customerImg;
        private Texture2D shopImg;

        //vectore or rectangle related variables
        private Rectangle[] cashierLoc = new Rectangle[4];
        private Vector2[] cashierNameLoc = new Vector2[4];
        private Vector2[] timeRankLoc = new Vector2[5];
        private Vector2[] wayPoint = new Vector2[18];

        //list variable that will draw the 
        private List<CustomerView> customerView = new List<CustomerView>();

        //the locations of infront of the cashiers
        public Vector2[] FrontCashierCustLoc { get; private set; }

        ///////OVERLOAD_CONSTRUCTOR//////
        /// <summary>
        /// OverLoad Constructor
        /// </summary>
        /// <param name="content"></param> allow this class use the content function, so that this class is able to load all the image
        public View(ContentManager content)
        {
            //load the picture file
            cashierImg = content.Load<Texture2D>(@"Texture2D\cashier");
            customerImg = content.Load<Texture2D>(@"Texture2D\customer");
            shopImg = content.Load<Texture2D>(@"Texture2D\shop");
            //load the spriteFont file
            regularFont = content.Load<SpriteFont>(@"Font\regularFont");
            subTitleFont = content.Load<SpriteFont>(@"Font\subTitleFont");
            smallerFont = content.Load<SpriteFont>(@"Font\smallerFont");

            //gives cashier images locations
            for (int i = 0; i < timeRankLoc.Length; i++)
            {
                if (i < cashierLoc.Length)
                {
                    cashierLoc[i] = new Rectangle(400 + (i * 100), 15, 40, 40);
                    cashierNameLoc[i] = new Vector2((cashierLoc[i].X - 15), (cashierLoc[i].Y - 15));
                }
                timeRankLoc[i] = new Vector2(100, 220 + (i * 20));
            }

            //gives a value to all the way point 
            for (int i = 0; i < wayPoint.Length; i++)
            {
                //gives the customerInLineVariable a X coordinate 
                if (i == 1 || i == 8 || i == 9 || i == 16)
                {
                    wayPoint[i].X = cashierLoc[0].X;
                }
                else if (i == 2 || i == 7 || i == 10 || i == 15)
                {
                    wayPoint[i].X = cashierLoc[1].X;
                }
                else if (i == 3 || i == 6 || i == 11 || i == 14)
                {
                    wayPoint[i].X = cashierLoc[2].X;
                }
                else if (i == 4 || i == 5 || i == 12 || i == 13)
                {
                    wayPoint[i].X = cashierLoc[3].X;
                }
                //gives the customerInLineVariable a Y coordinate 
                if (i <= 4 && i > 0)
                {
                    wayPoint[i].Y = 100;
                }
                else if (i > 4 && i <= 8)
                {
                    wayPoint[i].Y = 210;
                }
                else if (i > 8 && i <= 12)
                {
                    wayPoint[i].Y = 270;
                }
                else
                {
                    wayPoint[i].Y = 330;
                }

                wayPoint[5].Y = 170; // this is the first space in the line
                wayPoint[0] = new Vector2(-40, 125); //exit point
                wayPoint[17] = new Vector2(cashierLoc[0].X, 500); //entry point


                FrontCashierCustLoc = new Vector2[4];
            }

            //getting the locations of infront of the cashiers
            for (int i = 0; i < FrontCashierCustLoc.Length; i++)
            {
                FrontCashierCustLoc[i] = wayPoint[i];
            }
        }


        //getting the customer location
        public Vector2 ReturnCustomerLoc(int whichCustomer)
        {
            return customerView[whichCustomer].GetCustomerLoc();
        }

        /// <summary>
        /// This subprogram draws all aspcets of the shop, the shop an dcustomers
        /// </summary>
        /// <param name="spriteBatch">sb variable to draw</param>
        /// <param name="simTime">the time the sim has been running</param>
        /// <param name="numServed">the num of customers served</param>
        /// <param name="aveWaitTime">the ave time of a customer in the store</param>
        /// <param name="minWaitTime">the min time of a customer in the store</param>
        /// <param name="maxTime">the max time of someone in the store</param>
        /// <param name="isOver">determines if the sim is over</param>
        public void DrawShop(SpriteBatch spriteBatch, double simTime, int numServed, double aveWaitTime, double minWaitTime, double maxTime, bool isOver, string[] topFive)
        {
            //draw the shop background
            spriteBatch.Draw(shopImg, Vector2.Zero, Color.White);

            //draw all the status of this simulation such as time, the top 5 longest waiter, the number of custmoer have served
            spriteBatch.DrawString(subTitleFont, "Top 5 ", new Vector2(timeRankLoc[0].X, (timeRankLoc[0].Y - 20)), Color.Black);
            spriteBatch.DrawString(subTitleFont, "Customers Served: " + numServed, new Vector2(timeRankLoc[4].X, (timeRankLoc[4].Y + 30)), Color.Red);
            spriteBatch.DrawString(subTitleFont, "Average Time: " + Math.Round(aveWaitTime, 2) + " seconds", new Vector2(timeRankLoc[4].X, (timeRankLoc[4].Y + 50)), Color.Red);
            spriteBatch.DrawString(subTitleFont, "Max Time: " + maxTime + " seconds", new Vector2(timeRankLoc[4].X, (timeRankLoc[4].Y + 70)), Color.Red);
            spriteBatch.DrawString(subTitleFont, "Min Time: " + minWaitTime + " seconds", new Vector2(timeRankLoc[4].X, (timeRankLoc[4].Y + 90)), Color.Red);
            spriteBatch.DrawString(subTitleFont, "Simulation Time: " + Math.Round(simTime, 0) + " seconds", new Vector2(timeRankLoc[4].X, (timeRankLoc[4].Y + 110)), Color.Red);
            spriteBatch.DrawString(regularFont, "PRESS SPACE TO PAUSE AND UNPAUSE", new Vector2(0, 550), Color.Black);

            //if the simulation is over
            if (isOver)
            {
                spriteBatch.DrawString(regularFont, "SIMULATION IS OVER", new Vector2(300, 300), Color.Black);
            }


            //draw the customer image and their order
            for (int i = 0; i < customerView.Count; i++)
            {
                customerView[i].Draw(spriteBatch, customerImg, smallerFont);

            }

            //draw the ranking and the cashier name
            for (int i = 0; i < timeRankLoc.Length; i++)
            {
                //drawing the cashiers
                if (i < cashierLoc.Length)
                {
                    spriteBatch.Draw(cashierImg, cashierLoc[i], Color.White);
                    spriteBatch.DrawString(smallerFont, "Cashier#" + (i + 1), cashierNameLoc[i], Color.Black);
                }

               
            }
            for (int i = 0; i < topFive.Length; i++)
            {
                //drawing the top 5
                spriteBatch.DrawString(regularFont, (i + 1) + ": " + topFive[i], timeRankLoc[i], Color.Black);
            }
        }

        /// <summary>
        /// this sub-program, responsible for creating new customer, and all the customer movement 
        /// </summary>
        /// <param name="generateCustomer"></param>bool variable from outside, that determine should the program create new customer
        /// <param name="cashierAvailable"></param>bool variable that determine which cashier is available
        /// <param name="cashier"></param>status of each cashier //changed, copy this
        public void CustomerMovement(bool generateCustomer, bool[] cashierAvailable, CustomerNode[] cashier, string name) //changed, copy this and replace the oringinal one
        {
            //if statement that will determine when to generate a new customer
            if (generateCustomer)
            {
                customerView.Add(new CustomerView(name));
            }

            //show each cashier's stauts by changing cashier location 
            for (int i = 0; i < 4; i++)
            {
                if (cashier[i] == null && cashierLoc[i].Y >= 15) //if they are still recovering from the last customer's order, it will be far behind the counter
                {
                    cashierLoc[i].Y = 0;
                }
                else if (cashier[i] != null) //if the cashier is serveing or able to serve a new customer, this cashier will be standing right in front of the counter
                {
                    cashierLoc[i].Y = 15;
                }
            } // changed, this entire for loop is new

            //for loop that will call the sub-program from customerView to move the customer
            for (int i = 0; i < customerView.Count; i++)
            {
                if (i == 0)
                {
                    customerView[i].MoveCustomer(wayPoint, null, cashierAvailable, i, cashier); //changed, copy this and replace the oringial one
                }
                else
                {
                    customerView[i].MoveCustomer(wayPoint, customerView[i - 1], cashierAvailable, i, cashier); //changed, copy this and replace the oringial one
                }

                //remove the customer from the list variable that is no long appear on the screen
                if (customerView[i].isItDone)
                {
                    customerView.RemoveAt(i);
                }
            }

        }
    }
}

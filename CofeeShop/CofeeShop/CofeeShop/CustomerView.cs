//Author: Sam Zheng
//Created Date: 2015/12/7
//Modified Date: 2015/12/14
//File Name: CustomerView.cs
//Project Name: A5_Simulation
//Description: This class resoponsible for all the customer's information such as location, order

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
    class CustomerView
    {


        ///////ATTRIBUTE//////
        private Vector2 customerLoc;  //customer's current location
        private Vector2 wayPoint;  //customer's destination point
        private int numOfWayPoint; //the number of the customer's destination point in the array vairable called wayPoint
        private int currentNum;  //the customer number in the line
        private string order; //the customer's order
        public bool isItDone; //is the customer no longer appear on the screen

        //const variable
        const int MAX_NUM_CUSTOMER = 16; //const int variable that store the maximun number of customer
        const int ENTRY_POINT = 17; //way point number that is the entry Point
        const int EXIT_POINT = 0;//way point number that is the exit point
        const int MOVEMENT_SPEED = 5;//movement speed variable
        const int FIRST_ONE_IN_LINE = 5;//customer number that is the first one in the line
        const int LAST_ONE_IN_LINE = 17;//customer number that is the last one in the line



        ///////OVERLOAD_CONSTRUCTOR//////
        public CustomerView(string order)
        {
            //getting the name of the customer
            this.order = order;

            //the inital value of the customer location
            customerLoc = new Vector2(-40, 500);
            //the initial way point for the customer
            numOfWayPoint = ENTRY_POINT;
            //the initial value for the customer
            isItDone = false;
        }


        ///////ACCESSOR//////
        //accessor that will return the location of the customer
        public Vector2 GetCustomerLoc()
        {
            return customerLoc;
        }


        //accessor that will return the number of way point that this customer has
        public int GetNumOfWayPoint()
        {
            return numOfWayPoint;
        }


        //accessor that will return the currentNum variable's value
        public int GetCurrentNum()
        {
            return currentNum;
        }


        //accessor that will return the value of customer's order
        public string GetOrder()
        {
            return order;
        }


        ///////MODIFIER//////
        public void SetCurrentNum(int newData)
        {
            currentNum = newData;
        }


        ///////BEHAVIOOUR//////
        /// <summary>
        /// Move the customer base on their way point
        /// </summary>
        /// <param name="wayPoint"></param>the vector2 array that store every way point the line and the exit point and entry point
        /// <param name="prevCustomer"></param>the customerView class that store the previous cutsomer
        /// <param name="isCashierBusy"></param>bool array variable that help check which cashier is not occupy
        /// <param name="currentNum"></param>the number of the customer in the line
        /// <param name="cashier"></param>the status of each cashier in a array  
        public void MoveCustomer(Vector2[] wayPoint, CustomerView prevCustomer, bool[] isCashierBusy, int currentNum, CustomerNode [] cashier ) //changed, copy this
        {
            //give a customer a number in line
            this.currentNum = currentNum;

            //if statement that check is the this customer still on the screen
            if (customerLoc.X <= wayPoint[EXIT_POINT].X && numOfWayPoint == EXIT_POINT)
            {
                isItDone = true;
            }

            //if statement that will change the customer way point depending on the situation
            if (currentNum == MAX_NUM_CUSTOMER) //if there is 16 customer appear in the screen
            {
                this.wayPoint = wayPoint[ENTRY_POINT]; //way poing sets to entry point
            }
            else if (currentNum > MAX_NUM_CUSTOMER) //if the there is more than 16 customers appear in the screen and the way point will set near the previous customer
            {
                this.wayPoint = new Vector2((wayPoint[ENTRY_POINT].X - ((currentNum - MAX_NUM_CUSTOMER) * 100)), wayPoint[ENTRY_POINT].Y); //the way point will be set next to the previous customer
            }
            else
            {
                this.wayPoint = wayPoint[numOfWayPoint]; //the way point sets to  one of the way point in line
            }

            //if statement that will change the location of the customer image
            if (customerLoc.X > this.wayPoint.X)
            {
                customerLoc.X -= MOVEMENT_SPEED;
            }
            else if (customerLoc.X < this.wayPoint.X)
            {
                customerLoc.X += MOVEMENT_SPEED;
            }

            //if statement that will change the y-coordinate of the customer image
            if (customerLoc.X == this.wayPoint.X && customerLoc.Y != this.wayPoint.Y)
            {
                customerLoc.Y -= MOVEMENT_SPEED;
            }

            //if statement that will change the way point to next point in the line
            if (customerLoc == this.wayPoint)
            {
                //if the customer is not the first one in line and the store has not reach its limit and the next numOfWayPoint is one the same as the previous customer numeOfWayPoint
                if (numOfWayPoint > FIRST_ONE_IN_LINE && currentNum < LAST_ONE_IN_LINE)
                {
                    if (currentNum == 0 || numOfWayPoint != (prevCustomer.GetNumOfWayPoint() + 1))
                    {
                        numOfWayPoint--; //decrease the numOfWayPoint, which mean move up the line by one space
                    }
                }
                //if the customer is the first one in line
                else if (numOfWayPoint == FIRST_ONE_IN_LINE)
                {
                    //for loop that will check is there a cashier available
                    for (int i = 0; i < isCashierBusy.Length; i++) //changed, replace the old one
                    {
                        if (!isCashierBusy[i] && cashier[i] != null) //changed, replace the old one
                        {
                            numOfWayPoint = (i + 1);
                            isCashierBusy[i] = true;
                            break; // break the loop if there is isCashierBusy available
                        }
                    }
                }
                //if the customer is right infront one of the cashier
                else if (numOfWayPoint < FIRST_ONE_IN_LINE && cashier[numOfWayPoint - 1].IsCustomerServed == true) //changed, copy this and replace the old one
                {
                    isCashierBusy[numOfWayPoint - 1] = false;
                    //the cashier is waiting for the next customer 
                    cashier[numOfWayPoint - 1] = null; //changed, copy this and put it in the exact same spot (i move this line from the ShopSim class, so get rid of the one in ShopSim class)
                    numOfWayPoint = EXIT_POINT;
                }
            }

        }



        /// <summary>
        /// drawing the customer
        /// </summary>
        /// <param name="spriteBatch">spriet batch</param>
        /// <param name="img">the customer img</param>
        /// <param name="font">the font</param>
        public void Draw(SpriteBatch spriteBatch, Texture2D img, SpriteFont font)
        {
            //draws the customer
            spriteBatch.Draw(img, customerLoc, Color.White);

            //writes the customers name
            spriteBatch.DrawString(font, order, new Vector2(customerLoc.X + 5, customerLoc.Y + 15), Color.Black);
        }

    }
}

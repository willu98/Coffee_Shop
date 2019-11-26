//Author: Willima Pu
//File Name: ShopSim.cs
//Project Name: CoffeeShop
//Creation Date: December 6th, 2015
//Modification Date: December 14th, 2015
//Description: this cladd runs the main coffee shop simulation, by adding and removing customers, 
//             this class calls upon the statistics class to draw out the statistics and uses the view
//             class to draw all graphics
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
    class ShopSim
    {
        //the two diffrent queues for the customers
        //inside the store
        private CustomerQueue inCustomerQue;
        //outside the store
        private CustomerQueue outCustomerQue;
        //the queue of the whole store
        private CustomerQueue wholeQueue;

        //temp customer variable
        private CustomerNode tempCustomer;

        //variable for statistics
        private Statistics stats;

        //creating instances of the cashier
        private CustomerNode[] cashiers;
        private bool[] areCashiersBusy = new bool[4] { false, false, false, false };

        //list of the customers names
        private string customerName;
 
        //creating a view
        private View shopView;

        //a max number of 20 customers are allowed in the line/queue
        const int MAX_NUM_IN = 12;

        //the time required before a new customer comes
        const int TIME_FOR_ADD_CUSTOMER = 3000;

        //The time in which the simulation runs
        const int TIME_OF_SIM = 300000;
        private double addNewCustomerTimer = NO_TIME;
        private double simTimer = NO_TIME;

        //determines if the sim is over or not
        private bool isSimOver;

        //detremines if a customer is being added
        private bool addingCustomer;

        //random number generator for which tetrimino
        private Random generateRandNum = new Random();

        //the customer number
        private int customerNum = 1;
        //THE HEAD OF THE QUEUE
        const int QUEUE_HEAD = 0;

        //the amount of milliseconds in one second
        const int NO_TIME = 0;
        const int NO_VALUE = 0;
        //THE NUMBER OF MILLISECONDS IN ONE SECOND
        const int ONE_SECOND = 1000;

        //determines the number of customers served
        private int numServed = NO_VALUE;
        //th sumof the waiting times of the customer
        private double totalWaitTime = NO_VALUE;

        //the average wait time of the customers
        private double averageWaitTime;
        private double minTime = NO_TIME;
        private double maxTime = NO_TIME;
        
        //top 5 wait times
        string[] top5WaitTimes = new string[5];


        /// <summary>
        /// constructor for the shop simulator
        /// </summary>
        /// <param name="content">allows for content to be loaded</param>
        public ShopSim(ContentManager content)
        {
            //initializing the stats class
            stats = new Statistics();

            //initializing the view
            shopView = new View(content);

            //initializing the customer nodes
            cashiers = new CustomerNode[4];

            //no customers are added initially
            addingCustomer = false;

            //initializing the outdoor and indoor queues
            inCustomerQue = new CustomerQueue();
            outCustomerQue = new CustomerQueue();
            wholeQueue = new CustomerQueue();
        }



        /// <summary>
        /// This subprogram updates the shop
        /// </summary>
        /// <param name="gameTime">helps with recording time</param>
        public void UpdateShop(GameTime gameTime)
        {
            //increasing the timer
            simTimer += (float)gameTime.ElapsedGameTime.Milliseconds;

            //moving the customer
            shopView.CustomerMovement(addingCustomer, areCashiersBusy, cashiers, customerName);

            //updates Top 5 wait times
            stats.UpdateTopFive(inCustomerQue);
            top5WaitTimes = stats.ReturnTopFive();

            //if it has not been 300 seconds
            if (simTimer < TIME_OF_SIM)
            {
                //a customer is not being added
                addingCustomer = false;

                //incrementing the timer for a new customer
                addNewCustomerTimer += (float)gameTime.ElapsedGameTime.Milliseconds;

                //if it has been more than 3 seconds, add a new customer
                if (addNewCustomerTimer >= TIME_FOR_ADD_CUSTOMER)
                {
                    //adding a customer
                    addingCustomer = true;

                    //restarting the timer
                    addNewCustomerTimer = NO_TIME;

                    //getting the new customer
                    tempCustomer = new CustomerNode(customerNum);

                    //getting the name of the customer
                    customerName = tempCustomer.GetOrder();

                    //add a customer to the outside line
                    outCustomerQue.AddToQueue(tempCustomer);

                    //adding a custoemr to the shole queue list
                    wholeQueue.AddToQueue(tempCustomer);

                    //adding to the number of customers
                    customerNum++;

                    //if inside is not full then
                    if (inCustomerQue.GetCustomerAmount() <= MAX_NUM_IN)
                    {
                        //the head of the oud door queue is put as the tail of the indoor queue
                        inCustomerQue.AddToQueue(outCustomerQue.GetFirstCustomer());

                        //removing the out queue head
                        outCustomerQue.RemoveHead();
                    }
                }



                //checking each cashier
                for (int i = NO_VALUE; i < cashiers.Length; i++)
                {
                    //if the cashier is not busy
                    if (cashiers[i] == null)
                    {
                        //the next available vcustooemr is brought to the cashier
                        cashiers[i] = inCustomerQue.GetFirstCustomer();

                        //the front customer is dequeued
                        inCustomerQue.RemoveHead();

                        break; // changed, copy this
                    }


                    //if the cashier is no
                    if (cashiers[i] != null)
                    {
                        //updating the customer at the cashier
                        cashiers[i].UpdateCustomer(gameTime, true);

                        //if the customer is served
                        if (cashiers[i].IsCustomerServed == true)
                        {
                            //adding to the total wait time of the customers
                            totalWaitTime += cashiers[i].ReturnTotalTime();

                            //increasing the number of customers served
                            numServed++;

                            //getting the average wait time
                            averageWaitTime = stats.UpdateAveWaitTime(totalWaitTime / ONE_SECOND, numServed);

                            //checking to see if there is a min wait time or new max time
                            minTime = stats.GetMinWaitTime(cashiers[i].ReturnTotalTime());
                            maxTime = stats.GetMaxWaitTime(cashiers[i].ReturnTotalTime());

                            //removingthe front customer
                            wholeQueue.RemoveHead();

                            break;
                        }
                    }
                }
                inCustomerQue.Update(gameTime);
                outCustomerQue.Update(gameTime);
                wholeQueue.Update(gameTime);
            }                
            else
            {
                isSimOver = true;
            }

        }



        /// <summary>
        /// this subprogram draws the view of the shop
        /// </summary>
        /// <param name="sb">sprite batch for drawing</param>
        public void Draw(SpriteBatch sb)
        {
            //drawing the shop
            shopView.DrawShop(sb, simTimer / ONE_SECOND, numServed, averageWaitTime, minTime / ONE_SECOND, maxTime / ONE_SECOND, isSimOver, top5WaitTimes);
        }

    }
}

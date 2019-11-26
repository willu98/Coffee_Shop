//Author: Willima Pu
//File Name: CustomerNode.cs
//Project Name: CoffeeShop
//Creation Date: December 6th, 2015
//Modification Date: December 14th, 2015
//Description: this class maintains all information for one single customer
//             and updates the customer for things such as its time in the store
//             and other timers. This subprogram has subprograms that return the name/order type
//             that will be used in drawing
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
    class CustomerNode
    {
        //the next customer
        private CustomerNode nextCustomer;

        //the customer type, so order type and number
        private string customerType;

        //the order of the customer
        private int customerOrder;

        //determines the serving time based on the order
        private int customerServingTime;

        //The times for these customer sto be served in milliseconds
        const int COFFEE_TIME = 12000;
        const int FOOD_TIME = 18000;
        const int BOTH_TIME = 30000;

        //CONST FOR NO TIME
        const double NO_TIME = 0;

        //generator to get a random type of customer
        private Random generator = new Random();

        //the servig time of the customer
        private double servingTime = NO_TIME;
        private double totalWaitTime = NO_TIME;

        //the waiting time of the customer
        private double waitTime = NO_TIME;

        //determines if the customer is served
        public bool IsCustomerServed { get; private set; }

        //constants for the three types of orders
        const int COFFEE = 1;
        const int FOOD = 2;
        const int BOTH = 3;


        /// <summary>
        /// the constructor for the customer node
        /// </summary>
        /// <param name="customerNumber">the number of the customer</param>
        public CustomerNode(int customerNumber)
        {
            //the customer's order is randomly selected
            customerOrder = generator.Next(1, 4);

            //the customer is currently not served
            IsCustomerServed = false;


            switch (customerOrder)
            {
                //if the user orders coffee
                case COFFEE:
                    //they have the name coffee followed by their number
                    customerType = "Coffee." + customerNumber;

                    //assighned the coffee waiting time which is 12 seconds
                    customerServingTime = COFFEE_TIME;
                    break;


                //if the customers order is food
                case FOOD:

                    //they have the name Food followed by their number
                    customerType = "Food." + customerNumber;

                    //the customer's serving time is 18 seconds
                    customerServingTime = FOOD_TIME;
                    break;


                //if the customer is to order both
                case BOTH:

                    //they have the name both followed by their number
                    customerType = "Both." + customerNumber;

                    //the customers serving time is set to 30 seconds
                    customerServingTime = BOTH_TIME;
                    break;
            }
        }



        //returns the name of the customer
        public string GetOrder()
        {
            return customerType;
        }



        /// <summary>
        /// Gets the very next customer, in this case, the 
        /// customer behind is returned
        /// </summary>
        /// <returns>the next customer in line</returns>
        public CustomerNode GetNextCustomer()
        {
            return nextCustomer;
        }



        /// <summary>
        /// This subprogram adds onto the queue by placing aother customer to the last 
        /// customer in line
        /// </summary>
        /// <param name="nextCustomer">the customer that is put in the queue</param>
        public void SetNextCustomer(CustomerNode nextCustomer)
        {
            this.nextCustomer = nextCustomer;
        }



        /// <summary>
        /// This subprogram returns the time the customer waited
        /// this will be calle donce the customer is finished ordering
        /// </summary>
        /// <returns>the time the customer waited</returns>
        public double ReturnWaitTime()
        {
            return waitTime;
        }



        /// <summary>
        /// this subprogram returns the total time the customer was in the store
        /// </summary>
        /// <returns>the total time the customer was active</returns>
        public double ReturnTotalTime()
        {
            return totalWaitTime;
        }


        /// <summary>
        /// This updates the specific customer
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <param name="isOccupied">Determines if the custer is with a cashier or not</param>
        public void UpdateCustomer(GameTime gameTime, bool isOccupied)
        {
            //incrementing the total time of the customer
            totalWaitTime += (float)gameTime.ElapsedGameTime.Milliseconds;

            //if the customer is at a cashier
            if (isOccupied)
            {
                //incrementing the serving time of the customer
                servingTime += (float)gameTime.ElapsedGameTime.Milliseconds;

                //if the customer is served then
                if (servingTime >= customerServingTime)
                {
                    //the customer is no longer at the cashier
                    IsCustomerServed = true;
                }
            }
            else
            {
                //otherwise the wait time of the customer is increased
                //in other words if the customer is not with a cashier then
                waitTime += (float)gameTime.ElapsedGameTime.Milliseconds;
            }

        }


    }
}

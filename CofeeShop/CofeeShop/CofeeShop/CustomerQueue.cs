//Author: Michael Lotkin
//File Name: ShopSim.cs
//Project Name: CoffeeShop
//Creation Date: December 6th, 2015
//Modification Date: December 14th, 2015
//Description: this class runs the whole coffee shop simulation
//             by calling upon the shop simulation class' update subprogram
//             and the sraw is done by calling the shop sim's draw
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
    class CustomerQueue
    {
        //the first customer
        private CustomerNode firstCustomer;

        //the amount of customers
        private int amountOfCustomers;

        public CustomerQueue()
        {
            //the number of customers is set to zero initially
            amountOfCustomers = 0;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            //temp variable for the customers
            CustomerNode currentCustomer = firstCustomer;

            //checking each customer
            for (int i = 0; i < amountOfCustomers; i++)
            {
                //upadting each customer
                currentCustomer.UpdateCustomer(gameTime, false);

                //getting the next customer
                currentCustomer = currentCustomer.GetNextCustomer();
            }
        }


        /// <summary>
        /// provides the cashier with the first customer in line
        /// </summary>
        /// <returns>the front customer</returns>
        public CustomerNode GetFirstCustomer()
        {
            return firstCustomer;
        }


        /// <summary>
        /// returns number of custoemrs in the store
        /// </summary>
        /// <returns></returns>
        public int GetCustomerAmount()
        {
            return amountOfCustomers;
        }


        /// <summary>
        /// removes the front customer
        /// </summary>
        public void RemoveHead()
        {
            //if there is at least one customer
            if (amountOfCustomers > 0)
            {               
                //the first customer is set to the next customer
                firstCustomer = firstCustomer.GetNextCustomer();

                //decreasing the number of customers
                amountOfCustomers--;
            }
        }



        /// <summary>
        /// adds a customer to the end
        /// </summary>
        /// <param name="newCustomer"></param>
        public void AddToQueue(CustomerNode newCustomer)
        {
            //if there are no customers
            if (amountOfCustomers == 0)
            {
                //the first customer is set to the new customer
                firstCustomer = newCustomer;
            }
            else
            {
                //temp variable ot store the customers
                CustomerNode currentCustomer = firstCustomer;

                //checking until the final customer
                for(int i = 0; i < amountOfCustomers - 1; i++)
                {
                    //going to the next customer
                    currentCustomer = currentCustomer.GetNextCustomer();
                }

                //adding a customer to the end of the queue
                currentCustomer.SetNextCustomer(newCustomer);
            }

            //increasing the number of customers
            amountOfCustomers++;
        }
    }
}

//Author: Michael Lotkin
//File Name: Statistics.cs
//Project Name: CoffeeShop
//Creation Date: December 6th, 2015
//Modification Date: December 14th, 2015
//Description: this class calculates and returns all stats
//             these include the max waiting and min waiting time
//             as well as the top 5 waiter in the queue
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
    class Statistics
    {
        //the maximum and minimum waiting tiumes
        private double minWaitingTime;
        private double maxWaigtingTime;
        
        //stores top 5 times and names
        //these variables can be publicly accessed but not modified
        public double[] topFiveTimes { get; private set; }
        public string[] topFiveNames { get; private set; }

        //the top 5
        private string[] top5 = new string[5];

        /// <summary>
        /// Gives the variables their defult values once the variable has been called
        /// </summary>
        public Statistics()
        {

            //all of the other defult values are 0 (because the class just started to track the time)
            topFiveTimes = new double[5];
            topFiveNames = new string[5];
        }

        /// <summary>
        /// subprogram uses merge sort and temporary variables to go through every person in the linked list
        /// sorts all the customer based of the wait time (from least to greatest)
        /// updates the top 5 array 
        /// </summary>
        /// <param name="updatedQueue"></param>
        public void UpdateTopFive(CustomerQueue updatedQueue)
        {
            //temporary customerqueue variable
            CustomerQueue tempQueue = updatedQueue;

            //temporary list that will store all the nodes from the linked list
            List<double> numList = new List<double>();
            List<string> nameList = new List<string>();

            //while the linked list is not at the end
            //populates the temp list
            for ( int i = 0; i < tempQueue.GetCustomerAmount(); i++)
            {
                //adds the names and time list
                numList.Add(tempQueue.GetFirstCustomer().ReturnTotalTime());
                nameList.Add(tempQueue.GetFirstCustomer().GetOrder());
                //removes that node from the linked list
                tempQueue.RemoveHead();
            }


            // calls the merge sort method (sorts the list)
            MergeSort(numList,nameList);

            //sets the 2 arrays (times and names) using the array from the temp list
            for (int i = 0; i < numList.Count; i++)
            {
                top5[i] = nameList[i] + Convert.ToString(numList[i]);
            }
        }


        /// <summary>
        /// returns the top five wait times
        /// </summary>
        /// <returns>the top five wait times of the  customers</returns>
        public string[] ReturnTopFive()
        {
            return top5;
        }

        /// <summary>
        ///this updates and returns the average waiting time of the customers
        ///in the coffee shop
        /// </summary>
        /// <param name="totalTime">the total wait time elapsed</param>
        /// <param name="numServed">number of custoemrs served</param>
        /// <returns>average waiting time</returns>
        public double UpdateAveWaitTime(double totalTime, int numServed)
        {
            //stores the average wait time of the user
            double aveWaitTime;

            //calculates the average wait time 
            //uses the time from the customer variable to calculate the average 
            aveWaitTime = totalTime / numServed;

            //returning the average wait time
            return aveWaitTime;
        }

        /// <summary>
        /// checks to see if the wait time is 
        /// </summary>
        /// <param name="testTime"></param>
        /// <returns></returns>
        public double GetMinWaitTime(double testTime)
        {
            if (minWaitingTime == 0)
            {
                minWaitingTime = testTime;
            }

            if (testTime < minWaitingTime)
            {
                //a new min time is set
                minWaitingTime = testTime;
            }

            return minWaitingTime;
        }


        /// <summary>
        /// checks to see if the wait time is 
        /// </summary>
        /// <param name="testTime"></param>
        /// <returns></returns>
        public double GetMaxWaitTime(double testTime)
        {

            //if the min time is zero
            if (testTime > maxWaigtingTime)
            {
                //the min wait time is the 1st actual wait time of 
                //a customer that waited
                maxWaigtingTime = testTime;
            }

            return maxWaigtingTime;
        }


        /// <summary>
        /// Uses the merge sort algorith to sort the list from least to greatest
        /// This is done by using recursion to split the list in half and the merge subprogram to compare and merge the variables
        /// </summary>
        /// <param name="numbersList"></param>
        private void MergeSort(List<double> numbersList, List<string> namesList)
        {
            
            for (int i = 1; i < numbersList.Count; i++)
            {
                int counter = i - 1;

                while (counter >= 0 && numbersList[counter] < numbersList[counter + 1])
                {
                    double tempInt = numbersList[counter];
                    string tempString = namesList[counter];

                    numbersList[counter] = numbersList[counter + 1];
                    namesList[counter] = namesList[counter + 1];
                    numbersList[counter + 1] = tempInt;
                    namesList[counter + 1] = tempString;
                    counter--;
                }
            }
        }

        /// <summary>
        /// Merges the list by comparing variables and setting the greater variables above the lesser variables in the list
        /// </summary>
        /// <param name="firstHalfTimes"></param>
        /// <param name="secondHalfTimes"></param>
        /// <param name="timesList"></param>
        /// <param name="firstHalfOfNames"></param>
        /// <param name="secondHalfOfNames"></param>
        /// <param name="namesList"></param>
        private void Merge(List<double> firstHalfTimes, List<double> secondHalfTimes, List<double> timesList, List<string> firstHalfOfNames, List<string> secondHalfOfNames, List<string> namesList)
        {
            //stores the max indext of the list
            //adds to max indexs of the 2 lists to get the overall max index
            int maxIndex = firstHalfTimes.Count() + firstHalfTimes.Count();

            //counter variables used to determin the index of the lists
            int firstHalfIndex = 0;
            int secondHalfIndex = 0;
            int counter = 0;

            //loop keeps on runing until all the variables have been compared in each list
            while (firstHalfIndex < firstHalfTimes.Count() || secondHalfIndex < secondHalfTimes.Count())
            {
                //if atleast one list has a varaible that has not been compared
                if (firstHalfIndex < firstHalfTimes.Count() && secondHalfIndex < secondHalfTimes.Count())
                {
                    //compares 2 variables from the 2 lists
                    if (firstHalfTimes[firstHalfIndex] >= secondHalfTimes[secondHalfIndex])
                    {
                        //if the number in the in the second half is greater than the number in the first half than it sets that number in the appropriate index of the customer list
                        timesList[counter] = firstHalfTimes[firstHalfIndex];
                        namesList[counter] = firstHalfOfNames[firstHalfIndex];

                        //updates the counter variable and advances the first half index 
                        firstHalfIndex++;
                        counter++;
                    }
                    else
                    {
                        //if the number in the first half is greater than the number in the second half than it sets that number in the appropriate index of the customer list
                        timesList[counter] = secondHalfTimes[secondHalfIndex];
                        namesList[counter] = secondHalfOfNames[firstHalfIndex];

                        //updates the counter variable and advances the first second index 
                        secondHalfIndex++;
                        counter++;
                    }
                }

                //if there is a number in the first half list (and not in the second half list) that has not been compared
                else if (firstHalfIndex < firstHalfTimes.Count())
                {
                    //sets that variable in the appropriate index 
                    timesList[counter] = firstHalfTimes[firstHalfIndex];
                    namesList[counter] = firstHalfOfNames[firstHalfIndex];

                    //updates the counter variable and advances the first second index 
                    firstHalfIndex++;
                    counter++;
                }

                //if there is a number in the second half list (and not in the first half list) that has not been compared
                else if (secondHalfIndex < secondHalfTimes.Count())
                {
                    //sets that variable in the appropriate index 
                    timesList[counter] = secondHalfTimes[secondHalfIndex];
                    namesList[counter] = secondHalfOfNames[firstHalfIndex];

                    //updates the counter variable and advances the first second index 
                    secondHalfIndex++;
                    counter++;
                }
            }
        }
    }
}

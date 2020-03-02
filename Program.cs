using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

//Devon Alleyne 

/*
    Problem: Create a PID manager. It is responsible for assinging PID'S to a
             process, which is then returned when the process is completed. No
             two processes must have the same PID. While there is a min PID of
             300 and max PID of 5000, there are only 4700 available for the OS
             to use. I can keep track of the PID's in a list. 

             There are three functions that need to be implemented.

             Allocate_List - Allocates a list large enough to hold PID's of the
                             OS. No parameters need to be passed, returns an int
                             to determine if creating the list was successful

             Allocate_PID - Randomly assigns a PID between the number of 300 and
                            5000. No parameters need to be passed, returns the
                            PID if successful, if list is full the function will
                            return -1.

             Release_PID - A PID that the OS has finished using will be passed
                           into the function to be released

    Assumptions: While PID's are being assinged to processes the program needs to check
                 that the newly assigned PID doesnt have the same value as a
                 previously assigned one

                 PID's are released based on how fast the process takes to
                 execute, PID's that belong to a short processes are released before
                 ones of longer. To acheive this I will randomly pick a PID from
                 the list to be deleted. Even though I am picking a PID already
                 assigned to the list to be deleted, I will still perform a check to
                 make sure the intended PID to be release is within range of the
                 OS possible PID's.
*/

namespace pidManager
{
    class Program
    { 
        //Value of pid that will be store into the data structure 
        public static int pid = 0;

        //Value of the smallest pid that can be assigned, set to const to prevent changing
        public const int min_pid = 300;

        //Value of the largest pid that can be assigned, set to const to prevent changing
        public const int max_pid = 5000;

        //Amount of possible pids
        public static int pid_amount = max_pid - min_pid;

        public static List<int> pid_list;

        public static StreamWriter writer;

        public static void Main(string[] args)
        {
            //Attempt to allocate list
            if (Allocate_List() == 1)
            {
                writer = new StreamWriter("/Users/devonalleyne/Desktop/pidManager/output.txt",true);

                //Allocate list with size of possible pids
                pid_list = new List<int>(pid_amount);

                //Write to console to show available PID's
                Console.WriteLine("List creation successful. Can hold " + pid_amount + " PIDs");
                writer.WriteLine("List creation successful. Can hold " + pid_amount + " PIDs");
                Console.WriteLine();
                writer.WriteLine();

                Console.WriteLine("Assigned 10 random PIDs.");
                writer.WriteLine("Assigned 10 random PIDs.");
                Console.WriteLine();
                writer.WriteLine();

                //Loop to assign 10 pid's
                for (int i = 1; i <= 10; i++)
                {
                    //Attempt to allocate pid
                    if (Allocate_PID() != -1)
                    {
                        //Check if pid to be added to list is currently in
                        //the list, if it is continue to generate pid's untill
                        //you get a unique pid
                        do
                        {
                            pid = Allocate_PID();
                        }
                        while (pid_list.IndexOf(pid) != -1);

                        //reduce pid count by one, add pid to the list
                        pid_amount--;
                        pid_list.Add(pid);

                        //Write to console the PID assigned and how many pids
                        //are remaining
                        Console.WriteLine("PID #" + i + " is: " + pid);
                        writer.WriteLine("PID #" + i + " is: " + pid);
                        Console.WriteLine("PIDs available: " + pid_amount);
                        writer.WriteLine("PIDs available: " + pid_amount);
                        Console.WriteLine();
                        writer.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine("Unable to Assign PID");
                        writer.WriteLine("Unable to Assign PID");
                    }
                }

                //Write to console all the pids that are in use 
                Console.WriteLine("The value of assigned PIDs are:");
                writer.WriteLine("The value of assigned PIDs are:");
                foreach (int a in pid_list)
                {
                    Console.WriteLine(a);
                    writer.WriteLine(a);
                }
                Console.WriteLine();
                writer.WriteLine();

                Console.WriteLine("Deleting two random PIDs.");
                writer.WriteLine("Deleting two random PIDs.");
                Console.WriteLine();
                writer.WriteLine();

                //Loop to release 2 PID's
                for (int i = 0; i <= 1; i++)
                {
                    //Select a random pid to be released
                    Random random = new Random();
                    int random_pid = random.Next(0, (9-i));

                    //Check to see if randomly selected PID is within range of possible PID's
                    if(pid_list.ElementAt(random_pid)>=300 || pid_list.ElementAt(random_pid) <= 5000)
                    {
                        //Write to console which PID will be released
                        Console.WriteLine("PID to be released: " + pid_list.ElementAt(random_pid));
                        writer.WriteLine("PID to be released: " + pid_list.ElementAt(random_pid));

                        //Release PID
                        Release_PID(pid_list.ElementAt(random_pid));
                        Console.WriteLine();
                        writer.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine("Invaild PID.");
                        writer.WriteLine("Invaild PID.");
                        Console.WriteLine();
                        writer.WriteLine();
                    }
              
                }
                
                //Write to console PIDS in use after release
                Console.WriteLine("The value of assigned PIDs after release are:");
                writer.WriteLine("The value of assigned PIDs after release are:");
                foreach (int a in pid_list)
                {
                    Console.WriteLine(a);
                    writer.WriteLine(a);
                }
                Console.WriteLine();
                writer.WriteLine();

                Console.WriteLine("--------------------------------------------------------------------------------");
                writer.WriteLine("--------------------------------------------------------------------------------");
            }
            else if (Allocate_List() == -1)
            {
                Console.WriteLine("List creation failed.");
                writer.WriteLine("List creation failed.");
            }

            writer.Close();
        }

        //Attempts to declare a List big enough to store needed pids
        //If successful, the main program will make the lis
        public static int Allocate_List()
        {
            List<int> pid_list = new List<int>(pid_amount);
            try
            {
                pid_list.Add(1);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        //Generates a random PID vaule between the min and max pid, if the
        //OS still has pids available for use the value is returned, if not the
        //function returns -1
        public static int Allocate_PID()
        {
            Random random = new Random();

            if (pid_amount != 0)
            {
                pid = random.Next(min_pid, max_pid);
                return pid;
            }
            return -1;
        }

        //A PID will be passed into the function then deleted from the list
        public static void Release_PID(int pid)
        {
            try
            {
                pid_list.Remove(pid);
                Console.WriteLine("PID released");
                writer.WriteLine("PID released");
            }
            catch
            {
                Console.WriteLine("PID not available for release");
                writer.WriteLine("PID not available for release");
            }
            
        }

    }
}
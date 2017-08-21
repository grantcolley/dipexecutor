//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Development In Progress Ltd">
//     Copyright © 2017. All rights reserved.
// </copyright>
// <author>Grant Colley</author>
//-----------------------------------------------------------------------

namespace DipDistributor.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            string url;

            if (args == null 
                || args.Length.Equals(0))
            {
                url = "http://+:5000";
            }
            else
            {
                url = args[0];
            }

            var service = new DistributorService();
            service.Run(url);
        }
    }
}
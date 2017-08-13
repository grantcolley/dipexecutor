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
            var service = new DistributorService();
            service.Run();
        }
    }
}
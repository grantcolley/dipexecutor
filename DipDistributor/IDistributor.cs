//-----------------------------------------------------------------------
// <copyright file="IDistributor.cs" company="Development In Progress Ltd">
//     Copyright © 2017. All rights reserved.
// </copyright>
// <author>Grant Colley</author>
//-----------------------------------------------------------------------

using DipRunner;
using System.Threading.Tasks;

namespace DipDistributor
{
    public interface IDistributor
    {
        Task<Step> RunAsync(Step step);
    }
}

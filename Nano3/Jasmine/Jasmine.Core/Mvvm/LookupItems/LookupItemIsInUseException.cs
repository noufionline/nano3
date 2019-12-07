// ***********************************************************************
// Assembly         : Jasmine.Core
// Author           : Noufal
// Created          : 12-07-2017
//
// Last Modified By : Noufal
// Last Modified On : 12-08-2017
// ***********************************************************************
// <copyright file="LookupItemViewModel.cs" company="CICON">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;


namespace Jasmine.Core.Mvvm.LookupItems
{
    public class LookupItemIsInUseException : Exception
    {
        public LookupItemIsInUseException(string errorMessage) : base(errorMessage) { }
    }

    public class ItemIsInUseException : Exception
    {
        public ItemIsInUseException(string errorMessage) : base(errorMessage) { }
    }
}
// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


namespace IdentityServer4.Quickstart.UI
{
    public class LogoutInputModel
    {
        public string LogoutId { get; set; }
    }

    public class ErrorPageViewModel
    {
        public string ReturnUrl {get;set;}
        public string ChallengeUrl{get;set;}
        public string Email {get;set;}
    }
}

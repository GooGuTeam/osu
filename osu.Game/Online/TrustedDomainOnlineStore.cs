// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.IO.Stores;
using osu.Framework.Logging;

namespace osu.Game.Online
{
    public sealed class TrustedDomainOnlineStore : OnlineStore
    {
        private readonly string? allowedHost;

        public TrustedDomainOnlineStore(string? customApiUrl = null)
        {
            if (!string.IsNullOrEmpty(customApiUrl) && Uri.TryCreate(customApiUrl, UriKind.Absolute, out var uri))
                allowedHost = uri.Host;
        }

        protected override string GetLookupUrl(string url)
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
                return string.Empty;

            if (uri.Host.EndsWith(@".ppy.sh", StringComparison.OrdinalIgnoreCase))
                return url;

            if (allowedHost != null && uri.Host.EndsWith(allowedHost, StringComparison.OrdinalIgnoreCase))
                return url;

            Logger.Log($@"Blocking resource lookup from external website: {url}", LoggingTarget.Network, LogLevel.Important);
            return string.Empty;
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace MTCG.API.Routing
{
    public class IdRouteParser
    {
        public bool IsMatch(string resourcePath, string routePattern)
        {
            var pattern = "^" + routePattern.Replace("{id}", ".*").Replace("/", "\\/") + "(\\?.*)?$";
            return Regex.IsMatch(resourcePath, pattern);
        }

        public bool isUsername(string resourcePath, string routePattern) {
            return true;
        }

        public bool isTrade(string resourcePath, string routePattern) {
            var pattern = "^" + routePattern.Replace("{tradingdealid}", ".*").Replace("/", "\\/") + "(\\?.*)?$";
            return Regex.IsMatch(resourcePath, pattern);
        }

        public bool isShowDeck(string path) {
            return (path == "/deck" || path == "/deck?format=plain") ? true : false;
        }

        public Dictionary<string, string> ParseParameters(string resourcePath, string routePattern)
        {
            // query parameters
            var parameters = ParseQueryParameters(resourcePath);

            // id parameter
            var id = ParseIdParameter(resourcePath, routePattern);
            if (id != null)
            {
                parameters["id"] = id;
            }

            return parameters;
        }

        private string? ParseIdParameter(string resourcePath, string routePattern)
        {
            var pattern = "^" + routePattern.Replace("{id}", "(?<id>[^\\?\\/]*)").Replace("/", "\\/") + "$";
            var result = Regex.Match(resourcePath, pattern);
            return result.Groups["id"].Success ? result.Groups["id"].Value : null;
        }

        private Dictionary<string, string> ParseQueryParameters(string route)
        {
            var parameters = new Dictionary<string, string>();

            var query = route
                .Split("?", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Skip(1)
                .FirstOrDefault();

            if (query != null)
            {
                var keyValues = query
                    .Split("&", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                    .Select(p => p.Split("=", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
                    .Where(p => p.Length == 2);

                foreach (var p in keyValues)
                {
                    parameters[p[0]] = p[1];
                }
            }

            return parameters;
        }

    }
}

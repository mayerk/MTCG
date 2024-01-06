using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MTCG {
    public sealed class Configuration {

        private static Configuration? _Instance = null;

        public static Configuration Instance {
            get {
                if(_Instance == null) {
                    _Instance = new();
                    _Instance.DatabasePath = "Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=mtcg";
                }
                return _Instance ?? new Configuration();
            }
        }

        public string DatabasePath {
            get; set;
        } = "";
    }
}

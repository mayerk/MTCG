using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models {
    internal class UserData {
        public string Displayname { get; set; }
        public string Bio { get; set; }
        public string Image { get; set; }

        public UserData(string displayname, string bio, string image) {
            this.Displayname = displayname;
            this.Bio = bio;
            this.Image = image;
        }
    }
}

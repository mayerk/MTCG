using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models {
    public class UserData {
        public string Displayname { get; set; }
        public string Bio { get; set; }
        public string Image { get; set; }

        public UserData(string name, string bio, string image) {
            this.Displayname = name;
            this.Bio = bio;
            this.Image = image;
        }
    }
}

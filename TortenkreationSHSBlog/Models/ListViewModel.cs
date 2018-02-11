namespace TortenkreationSHSBlog.Models {

    using System.Collections.Generic;

    public class ListViewModel {

        public Occasion? Occasion { get; set; }

        public IEnumerable<Picture> Pictures { get; set; }

        public ListViewModel() {
            //nothing to do
        }

    }

}
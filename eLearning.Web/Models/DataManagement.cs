using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace edurate.Web.Models
{
    public class DataManagement
    {
        private UserRatingRepository userRatingRepository = null;

        public DataManagement()
        {

        }

        public DataManagement(UserRatingRepository userRatingRepository)
        {
            this.userRatingRepository = userRatingRepository;
        }

        public UserRatingRepository UserRatingRepository 
        {
            get
            {
                if (userRatingRepository == null)
                {
                    userRatingRepository = new UserRatingRepository();
                }
                return userRatingRepository;
            }
        }
    }
}
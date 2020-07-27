using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentC_MVC.Util
{
    public enum Response
    {
        SUCCESS,
        DATABASE_ERROR,
        UNFILLED_FIELDS,
        ALREADY_CAR,
        INEXISTENT_CAR,
        INCORRECT_CREDENTIALS,
        NOT_MATCH_PASS,
        INCORRECT_OLD_PASS,
        INVALID_DATE,
        IREAL_BIRTH,
        INVALID_ZIP,
        INCORRECT_ID,
        INEXISTENT_CUSTOMER,
        UNAVAILABLE_CAR,
        UNAVAILABLE_CAR_IN_CITY,
        INVERSED_DATES,
        INEXISTENT_RESERVATION,
        INCORRECT_PRICE,
        INCORRECT_SDATE,
        USED_USERNAME,
        INVALID_PLATE,
        INVALID_CITY
    }
}

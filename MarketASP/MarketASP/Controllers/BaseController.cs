using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static MarketASP.Clases.Enum;

namespace MarketASP.Controllers
{
    public class BaseController : Controller
    {
        public void Alert(int tipo, string message, NotificationType notificationType)
        {
            string msg = "";

            switch (tipo)
            {
                case 1:
                    msg = "<script language='javascript'>Swal.fire('" + notificationType.ToString().ToUpper() + "', '" + message + "','" + notificationType + "')" + "</script>";
                    break;
                case 2:
                    msg = "<script language='javascript'> " +
                        " Swal.fire({position: 'center', icon: 'success', title: '" + message + "' , showConfirmButton: true, }).then( " +
                        " (result) => {if (result.isConfirmed){ cerrarventana()}}) " +
                        "</script>";
                    break;

                default:
                    break;
            }

            TempData["notification"] = msg;
        }

    }
}
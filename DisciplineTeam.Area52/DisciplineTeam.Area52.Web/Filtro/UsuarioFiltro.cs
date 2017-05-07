using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DisciplineTeam.Area52.Web.Filtro
{
    public class UsuarioFiltro : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["usuario"] == null)
            {
                filterContext.Result = new RedirectResult("~/Usuario/Login");
            }

        }
    }
}
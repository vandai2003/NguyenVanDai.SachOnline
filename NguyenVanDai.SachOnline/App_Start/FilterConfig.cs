using System.Web;
using System.Web.Mvc;
using NguyenVanDai.SachOnline.Controllers;

namespace NguyenVanDai.SachOnline
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
      
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyCookBookWebApplication.Pages {
    public class ErrorPageModel : PageModel {

	    public string errorMsg = "שגיאה";

        public void OnGet() {
	        string specificErrorMsg = HttpContext.Session.GetString("errorMsg");

			if (!String.IsNullOrEmpty(specificErrorMsg)) {
				errorMsg = specificErrorMsg;
			}
        }
    }
}
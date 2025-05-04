using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Syddjurs.Utilities
{
    public  static class HandleShellMenuBasenOnRoles
    {
        public static void HideShowMenuItems(string jwtToken)
        {
            var roleClaims = JwtHelper.GetUserRolesFromToken(jwtToken);

            var roleNameList = new List<string>();
            foreach (var roleClaim in roleClaims) 
            {
                roleNameList.Add(roleClaim.Value);
            }

            //var adminItem = shell.Items.FirstOrDefault(i => i.Route == "admin");

            foreach(var menuItem in Shell.Current.Items)
            {
                switch (menuItem.Route)
                {
                    case "items":
                        if (roleNameList.Contains("Administrator") || roleNameList.Contains("Manager"))
                        {
                            menuItem.IsVisible = true;
                        }
                        else
                        {
                            menuItem.IsVisible = false;
                        }
                     break;

                    case "register":
                        if (roleNameList.Contains("Administrator") || roleNameList.Contains("Manager"))
                        {
                            menuItem.IsVisible = true;
                        }
                        else
                        {
                            menuItem.IsVisible = false;
                        }
                        break;

                    case "categories":
                        if (roleNameList.Contains("Administrator") || roleNameList.Contains("Manager"))
                        {
                            menuItem.IsVisible = true;
                        }
                        else
                        {
                            menuItem.IsVisible = false;
                        }
                        break;

                    default:
                        // code block
                        break;
                }
            }
        }

      
    }
}

using System;
using System.Threading.Tasks;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace MobileApplications.Services
{
    public class RuntimePermission
    {
        public async static Task<PermissionStatus> RuntimePermissionStatus(Permission permission)
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);
                if (status != PermissionStatus.Granted)
                {
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(permission);
                    return status = results[permission];
                }
                return status;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("\nIn App.Helpers.NetworkHelper.CheckRequestPermissionAsync() - Exception attempting to check or request permission to Permission.{0}:\n{1}\n", permission, ex);
                return PermissionStatus.Unknown;
            }
        }
    }
}

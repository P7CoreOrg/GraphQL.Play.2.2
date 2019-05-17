using System;
using AppIdentity.Contracts;
using AppIdentity.Models;
using P7Core.Utils;

namespace AppIdentity.Extensions
{
    public static class AppIdentityCreateInputModelExtensions
    {
        static void ValidateOperation(string exceptionMessage,Func<bool> condition)
        {
            if (!condition.Invoke())
            {
                throw new GraphQL.ExecutionError(exceptionMessage);
            }
        }
        public static void ValidateConstraints(
            this AppIdentityCreateInputModel model, 
            IAppIdentityConfiguration appIdentityConfiguration)
        {
            ValidateOperation("subject out of range", () =>
            {
                if (string.IsNullOrWhiteSpace(model.Subject)) return true;
                if (model.Subject.Length > appIdentityConfiguration.MaxSubjectLength) return false;
                return true;
               
            });
            ValidateOperation("appId out of range", () =>
            {
                if (string.IsNullOrWhiteSpace(model.AppId) ||
                    model.AppId.Length > appIdentityConfiguration.MaxAppIdLength)
                {
                    return false;
                }
                return true;
            });
            ValidateOperation("machineId out of range", () =>
            {
                if (string.IsNullOrWhiteSpace(model.MachineId) ||
                    model.MachineId.Length > appIdentityConfiguration.MaxMachineIdLength)
                {
                    return false;
                }
                return true;
            });
        }
    }
}

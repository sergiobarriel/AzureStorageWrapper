using System;
using System.IO;
using System.Runtime.CompilerServices;
using AzureStorageWrapper.Exceptions;
using EnsureThat;
using EnsureThat.Enforcers;

namespace AzureStorageWrapper.Extensions
{
    public static class EnsureThatExtension
    {
        public static string IsNotNullOrEmptySW(this StringArg _, string value, [CallerMemberName] string paramName = null)
            => Ensure.String.IsNotNullOrEmpty(value, paramName, opts => opts.WithException(new AzureStorageWrapperException($"{paramName} is empty!")));

        public static string IsNotUri(this StringArg _, string value, [CallerMemberName] string paramName = null)
        {
            IsNotNullOrEmptySW(_, value, paramName);
            return Uri.TryCreate(value, UriKind.Absolute, out var @__)
                   ? value
                   : throw new AzureStorageWrapperException($"{paramName} is not a valid absolute URI!");
        }


        public static long IsLteSW(this ComparableArg _, long value, long limit, [CallerMemberName] string paramName = null)
            => Ensure.Comparable.IsLte(value, limit, paramName, opts => opts.WithException(new AzureStorageWrapperException($"{paramName} should be lower than {limit}")));


        public static Stream IsNotZero(this AnyArg _, Stream value, [CallerMemberName] string paramName = null)
            => value.Length > 0
               ? value
               : throw new AzureStorageWrapperException($"{paramName} length is 0");
        public static byte[] IsNotZero(this AnyArg _, byte[] value, [CallerMemberName] string paramName = null)
            => value.Length > 0
               ? value
               : throw new AzureStorageWrapperException($"{paramName} length is 0");


        public static bool IsPaginateValid(this BoolArg _, bool paginate, int size, [CallerMemberName] string paramName = null)
        {
            if (paginate && size <= 0)
                throw new AzureStorageWrapperException($"{nameof(size)} should be greater than zero when {nameof(paginate)} is true.");
            return true;
        }
        public static bool IsPaginateValid(this BoolArg _, bool paginate, string ContinuationToken, [CallerMemberName] string paramName = null)
        {
            if (!paginate && !string.IsNullOrEmpty(ContinuationToken))
                throw new AzureStorageWrapperException($"{nameof(ContinuationToken)} should be greater than zero when {nameof(paginate)} is true.");
            return true;
        }
        public static bool IsNotExistContainer(this BoolArg _, bool value,string container, [CallerMemberName] string paramName = null)
            => Ensure.Bool.IsTrue(value, paramName, opts => opts.WithException(new AzureStorageWrapperException($"container {container} doesn't exists!")));
        public static bool IsTrue(this BoolArg _, bool value,string message)
            => Ensure.Bool.IsTrue(value, null, opts => opts.WithException(new AzureStorageWrapperException(message)));
    }
}

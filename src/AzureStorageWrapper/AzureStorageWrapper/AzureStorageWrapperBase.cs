using System;

namespace AzureStorageWrapper
{
    public abstract class AzureStorageWrapperBase
    {
        /// <summary>
        /// ~2 centuries of work are needed in order to have a 1% probability of at least one collision: https://alex7kom.github.io/nano-nanoid-cc
        /// </summary>
        internal string GetRandomId()
        {
            var guid = Guid.NewGuid().ToString("N");

            return guid.Substring(0, 15);
        }
    }
}
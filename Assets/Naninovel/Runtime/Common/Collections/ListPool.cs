// Copyright 2017-2021 Elringus (Artyom Sovetnikov). All Rights Reserved.

using System.Collections.Generic;

namespace Naninovel
{
    /// <inheritdoc cref="ObjectPool{T}"/>
    public static class ListPool<T>
    {
        private static readonly ObjectPool<List<T>> pool = new ObjectPool<List<T>>(null, Clear);

        /// <inheritdoc cref="ObjectPool{T}.Get"/>
        public static List<T> Get () => pool.Get();

        /// <inheritdoc cref="ObjectPool{T}.Return"/>
        public static void Return (List<T> list) => pool.Return(list);

        private static void Clear (List<T> list) => list.Clear();
    }
}

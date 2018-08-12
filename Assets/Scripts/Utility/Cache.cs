using System;

namespace Assets.Scripts.Utility
{
    public class Cache<T>
    {
        public delegate T SourceDelegate();

        public static implicit operator T(Cache<T> chache)
        {
            var currentTime = DateTime.Now;
            if ((currentTime - chache.lastTime).Seconds > chache.expiration)
            {
                chache.cachedValue = chache.source();
                chache.lastTime = currentTime;
            }
            return chache.cachedValue;
        }

        private T cachedValue;
        private DateTime lastTime = new DateTime(2000, 1, 1);
        private SourceDelegate source;
        private int expiration = 0;

        public Cache(SourceDelegate source_, int expiration_)
        {
            source = source_;
            expiration = expiration_;
        }
    }
}

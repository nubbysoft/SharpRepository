﻿using System;
using System.Linq.Expressions;
using System.Runtime.Caching;
using SharpRepository.Repository.Helpers;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;

namespace SharpRepository.Repository.Caching
{
    public abstract class TimeoutCachingStrategyBase<T, TKey> : CachingStrategyBase<T, TKey> where T : class
    {
        public int TimeoutInSeconds { get; set;  }

        internal TimeoutCachingStrategyBase(int timeoutInSeconds, int? maxResults, ICachingProvider cachingProvider = null)
            : base(maxResults, cachingProvider)
        {
            TimeoutInSeconds = timeoutInSeconds;
        }

        public override void Add(TKey key, T result)
        {
            // update the cached item
            SetCache(GetWriteThroughCacheKey(key), result);
        }

        public override void Update(TKey key, T result)
        {
            // update the cached item
            SetCache(GetWriteThroughCacheKey(key), result);
        }

        public override void Delete(TKey key, T result)
        {
            ClearCache(GetWriteThroughCacheKey(key));
        }

        public override void Save()
        {
            // nothing to do
        }

        // helpers
        protected new void SetCache<TCacheItem>(string cacheKey, TCacheItem result, IQueryOptions<T> queryOptions = null)
        {
            try
            {
                CachingProvider.Set(cacheKey, result, CacheItemPriority.Default, TimeoutInSeconds);

                if (queryOptions is IPagingOptions)
                {
                    CachingProvider.Set(cacheKey + "=>pagingTotal", ((IPagingOptions)queryOptions).TotalItems, CacheItemPriority.Default, TimeoutInSeconds);
                }
            }
            catch (Exception)
            {
                // don't let caching errors mess with the repository
            }
        }

        protected override string GetAllCacheKey<TResult>(IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector)
        {
            return String.Format("{0}/{1}/{2}", FullCachePrefix, TypeFullName, Md5Helper.CalculateMd5("All::" + (queryOptions != null ? queryOptions.ToString() : "null") + "::" + (selector != null ? selector.ToString() : "null")));
        }

        protected override string FindAllCacheKey<TResult>(ISpecification<T> criteria, IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector)
        {
            return String.Format("{0}/{1}/{2}/{3}", FullCachePrefix, TypeFullName, "FindAll", Md5Helper.CalculateMd5(criteria + "::" + (queryOptions != null ? queryOptions.ToString() : "null") + "::" + (selector != null ? selector.ToString() : "null")));
        }

        protected override string FindCacheKey<TResult>(ISpecification<T> criteria, IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector)
        {
            return String.Format("{0}/{1}/{2}/{3}", FullCachePrefix, TypeFullName, "Find", Md5Helper.CalculateMd5(criteria + "::" + (queryOptions != null ? queryOptions.ToString() : "null") + "::" + (selector != null ? selector.ToString() : "null")));
        }
    }
}

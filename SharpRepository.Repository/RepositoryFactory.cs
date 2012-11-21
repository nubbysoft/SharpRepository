﻿using System.Configuration;
using SharpRepository.Repository.Configuration;

namespace SharpRepository.Repository
{
    public static class RepositoryFactory
    {
        public static IRepository<T, TKey> GetInstance<T, TKey>() where T : class, new()
        {
            return GetInstance<T, TKey>("sharpRepository");
        }

        public static IRepository<T, TKey> GetInstance<T, TKey>(string configSection) where T : class, new()
        {
            return GetInstance<T, TKey>(GetConfiguration(configSection));
        }

        public static IRepository<T, TKey> GetInstance<T, TKey>(ISharpRepositoryConfiguration configuration) where T : class, new()
        {
            return configuration.GetInstance<T, TKey>();
        }

        private static ISharpRepositoryConfiguration GetConfiguration(string sectionName)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var section = config.GetSectionGroup(sectionName) as SharpRepositorySectionGroup;
            if (section == null)
                throw new ConfigurationErrorsException("Section " + sectionName + " is not found.");

            return section;
        }
    }
}
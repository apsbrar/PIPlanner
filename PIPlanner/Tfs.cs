using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.Framework.Common;

namespace PIPlanner
{
    public class Tfs
    {
        private readonly WorkItemStore store;

        public Tfs(Uri tfsUri)
        {
            TfsConfigurationServer configurationServer = TfsConfigurationServerFactory.GetConfigurationServer(tfsUri);
            // Get the catalog of team project collections
            ReadOnlyCollection<CatalogNode> collectionNodes = configurationServer.CatalogNode.QueryChildren(new[] { CatalogResourceTypes.ProjectCollection }, false, CatalogQueryOptions.None);

            // List the team project collections
            foreach (CatalogNode collectionNode in collectionNodes)
            {
                // Use the InstanceId property to get the team project collection
                Guid collectionId = new Guid(collectionNode.Resource.Properties["InstanceId"]);
                TfsTeamProjectCollection tfs = configurationServer.GetTeamProjectCollection(collectionId);
                store = (WorkItemStore) tfs.GetService(typeof (WorkItemStore));
            }
        }

        public ICollection<string> Projects
        {
            get { return (from Project proj in store.Projects select proj.Name).ToList(); }
        }

        public ICollection<string> GetIterationPaths(string projectName)
        {
            var result = new List<string>();

            foreach (Project project in store.Projects.Cast<Project>().
                Where(project => string.Compare(project.Name, projectName, StringComparison.OrdinalIgnoreCase) == 0))
            {
                foreach (Node node in project.IterationRootNodes)
                {
                    string path = project.Name + "\\" + node.Name;
                    result.Add(path);
                    RecursiveAddIterationPath(node, result, path);
                }

                break;
            }

            return result;
        }

        private static void RecursiveAddIterationPath(Node node, ICollection<string> result, string parentIterationName)
        {
            foreach (Node item in node.ChildNodes)
            {
                string path = parentIterationName + "\\" + item.Name;
                result.Add(path);
                if (item.HasChildNodes)
                {
                    RecursiveAddIterationPath(item, result, path);
                }
            }
        }

        public ICollection<string> GetWorkItemsInIterationPath(string iterationPath)
        {
            ICollection<string> result = new Collection<string>();
            string query = string.Format(CultureInfo.CurrentCulture,
                                         "SELECT [System.Id], [System.Title] FROM WorkItems WHERE [System.IterationPath] UNDER '{0}'",
                                         iterationPath);
            foreach (WorkItem item in store.Query(query))
            {
                result.Add(item.Fields["System.Id"].Value + ":" + item.Fields["System.Title"].Value);
            }

            return result;
        }
    }
}
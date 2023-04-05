using GraphQLBasic.Database;
using GraphQLBasic.Models;

namespace GraphQLBasic.GraphQL
{
    public class Query
    {
        private readonly TimeGraphContext dbContext;

        public Query(TimeGraphContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IQueryable<Project> Projects => dbContext.Projects;
        public IQueryable<TimeLog> TimeLogs => dbContext.TimeLogs;
    }
}

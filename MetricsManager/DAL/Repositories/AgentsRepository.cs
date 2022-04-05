using Core;
using Dapper;
using MetricsManager.Model;

namespace MetricsManager.DAL.Repositories
{
    public interface IAgentsRepository : IRepository<AgentInfo>
    {

    }
    public class AgentsRepository : IAgentsRepository
    {
        private readonly IConnectionManager _connectionManager;

        public AgentsRepository(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        public void Create(AgentInfo item)
        {
            using (var connection = _connectionManager.CreateOpenedConnection())
            {
                connection.Execute("INSERT INTO agents(agentaddress, isenabled) VALUES(@agentaddress, @isenabled)",
                new
                {
                    agentaddress = item.AgentAddress,
                    isenabled = true
                });
            }
        }

        public IList<AgentInfo> GetAll()
        {
            using (var connection = _connectionManager.CreateOpenedConnection())
            {
                return connection.Query<AgentInfo>("SELECT id, agentaddress, isenabled FROM agents").ToList();
            }
        }

        public AgentInfo GetById(int id)
        {
            using (var connection = _connectionManager.CreateOpenedConnection())
            {
                return connection.QuerySingle<AgentInfo>("SELECT id, agentaddress, isenabled FROM agents WHERE id = @id",
                new { id = id });
            }
        }

        public void Update(AgentInfo item)
        {
            using (var connection = _connectionManager.CreateOpenedConnection())
            {
                connection.Execute("UPDATE agents SET agentaddress = @agentaddress, isenabled = @isenabled WHERE id = @id",
                new
                {
                    agentaddress = item.AgentAddress,
                    isenabled = item.IsEnabled,
                    id = item.Id
                });
            }
        }

        public void Delete(int id)
        {
            using (var connection = _connectionManager.CreateOpenedConnection())
            {
                connection.Execute("DELETE FROM agents WHERE id=@id",
                new
                {
                    id = id
                });
            }
        }

        public IList<AgentInfo> GetMetricsOutPeriod(long fromTime, long toTime)
        {
            return GetAll();
        }

    }
}

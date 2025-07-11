using System.Data;
using Dapper;
using MySqlConnector;
using taipei_day_trip_dotnet.Entity;

namespace taipei_day_trip_dotnet.Data
{
    public class AttractionRepository : IAttractionRepository
    {
        private readonly TaipeiDbContext _context;
        private readonly string _connectionString;

        public AttractionRepository(TaipeiDbContext context, IConfiguration configuration)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IList<Attraction>> GetAllCategoriesAsync()
        {
            using (IDbConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "SELECT DISTINCT category FROM webpage";
                var result = (await connection.QueryAsync<Attraction>(sql)).ToList();

                return result;
            }
        }

        public async Task<IList<Attraction>> GetAllCategoriesAsync(string keyword)
        {
            using (IDbConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "SELECT DISTINCT category FROM webpage WHERE category = @keyword";
                var result = (await connection.QueryAsync<Attraction>(sql, new { keyword })).ToList();

                return result;
            }
        }
        public async Task<IList<Attraction>> GetAttractionsAsync(int page)
        {
            using (IDbConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                int pageSize = 12;
                string sql = "SELECT * FROM webpage limit @pageSize offset @offset";
                var result = (await connection.QueryAsync<Attraction>(sql, new
                {
                    pageSize = pageSize,
                    offset = (page - 1) * pageSize
                })).ToList();

                return result;
            }
            ;
        }

        public async Task<IList<Attraction>> GetAttractionsAsync(int page, string? keyword)
        {
            using (IDbConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                int pageSize = 12;

                if (string.IsNullOrWhiteSpace(keyword))
                {
                    string sql = "SELECT * FROM webpage LIMIT @pageSize OFFSET @offset";
                    var result = (await connection.QueryAsync<Attraction>(sql, new
                    {
                        pageSize = pageSize,
                        offset = (page - 1) * pageSize
                    })).ToList();

                    return result;
                }
                else
                {
                    string sql = "SELECT * FROM webpage WHERE category = @keyword OR name like @searchPattern LIMIT @pageSize OFFSET @offset";
                    var result = (await connection.QueryAsync<Attraction>(sql, new
                    {
                        pageSize = pageSize,
                        offset = (page - 1) * pageSize,
                        keyword = keyword,
                        searchPattern = $"%{keyword}%"
                    })).ToList();

                    return result;
                }
            }
            ;
        }
        public async Task<Attraction> GetAttractionByIdAsync(int id)
        {
            using (IDbConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM webpage WHERE id = @id";
                var result = (await connection.QueryAsync<Attraction>(sql, new
                {
                    id = id
                })).FirstOrDefault();

                return result;
            }
        }
    }

}
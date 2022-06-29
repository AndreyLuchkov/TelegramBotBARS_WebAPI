using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using TelegramBotBARS_WebAPI.Models;

namespace TelegramBotBARS_WebAPI.Services
{
    public class DbDataProvider : IDisposable
    {
        private readonly TGBot_BARS_DbContext _dbContext;
        private readonly string _connectionString;

        public DbDataProvider(TGBot_BARS_DbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _connectionString = configuration.GetConnectionString("PostgreSQL");
        }

        public IEnumerable<Statement> GetStatements(string? semester, string? type)
        {
            return _dbContext.Statements
                .Where(s
                    => s.Semester.Contains(semester ?? "")
                    && s.AttestationType.Contains(type ?? ""));
        }
        public Statement GetStatement(Guid statementId)
        {
            return _dbContext.Statements
                .Where(s => s.Id == statementId)
                .First();
        }
        public IEnumerable<MissedLessonRecord> GetMLRecords(Guid statementId)
        {
            return _dbContext.MissedLessonRecords
                .Where(record
                    => record.StatementId == statementId);
        }
        public IEnumerable<MissedLessonRecord> GetMLRecords(string semester)
        {
            string sqlQuery = $@"SELECT 
                    lesson_type,
                    lesson_date,
                    lesson_time,
                    reason,
                    st.discipline,
                    mlr.student_id,
                    statement_id
                FROM missed_lesson_records mlr
                LEFT JOIN statements st on st.id = mlr.statement_id
                WHERE st.semester LIKE @semester";

            try
            {
                using var dbConnection = new NpgsqlConnection(_connectionString);

                return dbConnection.Query<MissedLessonRecord>(
                    sqlQuery,
                    new { Semester = semester + '%' });
            }
            catch
            {
                return new List<MissedLessonRecord>();
            }
        }
        public IEnumerable<ControlEvent> GetControlEvents(Guid statementId)
        {
            return _dbContext.ControlEvents
                .Where(ce
                    => ce.StatementId == statementId);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}

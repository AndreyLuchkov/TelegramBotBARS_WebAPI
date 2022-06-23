using Microsoft.EntityFrameworkCore;
using TelegramBotBARS_WebAPI.Entities;

namespace TelegramBotBARS_WebAPI.Services
{
    public class DbDataProvider : IDisposable
    {
        private readonly TGBot_BARS_DbContext _dbContext;

        public DbDataProvider(TGBot_BARS_DbContext dbContext)
        {
            _dbContext = dbContext;
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
            var statements = _dbContext.Statements
                .Where(s => s.Semester.Contains(semester))
                .Include(s => s.MissedLessonRecords);

            foreach (var s in statements)
            {
                foreach (var record in s.MissedLessonRecords)
                {
                    record.Statement = s;
                }
            }

            return statements
                .SelectMany(s => s.MissedLessonRecords);
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

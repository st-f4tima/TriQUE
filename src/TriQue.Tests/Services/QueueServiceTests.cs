using Microsoft.VisualStudio.TestTools.UnitTesting;
using TriQue.Data.Database;
using TriQue.Data.Repositories;
using TriQue.Helpers;
using TriQue.Services;

namespace TriQue.Tests.Services
{
    [TestClass]
    public class QueueServiceTests
    {
        private DatabaseHelper _dbHelper;
        private QueueService _queueService;
        private QueueRepository _queueRepo;

        // seeded test data
        private const int TestDriverID = 1;
        private const int TestRouteID = 101;
        private const int TestQueueID = 1;

        [TestInitialize]
        public void Setup()
        {
            _dbHelper = new DatabaseHelper();
            var dbInitializer = new DatabaseInitializer(_dbHelper, AppConfig.Configuration);
            dbInitializer.Initialize();

            _queueService = new QueueService();
            _queueRepo = new QueueRepository();

            // clear queue entries
            _dbHelper.ExecuteNonQuery(
                "DELETE FROM QueueEntry WHERE QueueID = @queueID",
                new Microsoft.Data.Sqlite.SqliteParameter("@queueID", TestQueueID)
            );
        }

        [TestCleanup]
        public void Cleanup()
        {
            // remove test entries
            _dbHelper.ExecuteNonQuery(
                "DELETE FROM QueueEntry WHERE QueueID = @queueID",
                new Microsoft.Data.Sqlite.SqliteParameter("@queueID", TestQueueID)
            );

            // reset driver status
            _dbHelper.ExecuteNonQuery(
                "UPDATE Driver SET StatusID = 1 WHERE DriverID = @driverID",
                new Microsoft.Data.Sqlite.SqliteParameter("@driverID", TestDriverID)
            );
        }

        [TestMethod]
        public void JoinQueue_ShouldReturnAlreadyInQueue_WhenDriverIsAlreadyInQueue()
        {
            // first join
            _queueService.JoinQueue(TestDriverID, TestRouteID);

            // second join attempt
            string result = _queueService.JoinQueue(TestDriverID, TestRouteID);

            Assert.AreEqual("Already in queue.", result);
        }

        [TestMethod]
        public void JoinQueue_ShouldReturnCorrectPositionString_WhenSuccessfullyJoined()
        {
            // join queue
            string result = _queueService.JoinQueue(TestDriverID, TestRouteID);

            Assert.AreEqual("Joined queue. Position: #1", result);
        }

        [TestMethod]
        public void JoinQueue_ShouldUpdateDriverStatus_ToWaiting_AfterJoining()
        {
            // join queue
            _queueService.JoinQueue(TestDriverID, TestRouteID);

            // verify status update
            var result = _dbHelper.ExecuteScalar(
                "SELECT StatusID FROM Driver WHERE DriverID = @driverID",
                new Microsoft.Data.Sqlite.SqliteParameter("@driverID", TestDriverID)
            );

            Assert.AreEqual(1L, result, "Driver status should be 1 (Waiting) after joining queue");
        }

        [TestMethod]
        public void JoinQueue_ShouldIncrementPosition_ForEachNewEntry()
        {
            // first driver joins
            string first = _queueService.JoinQueue(1, TestRouteID);

            // second driver joins
            string second = _queueService.JoinQueue(2, TestRouteID);

            // remove test entry
            _dbHelper.ExecuteNonQuery(
                "DELETE FROM QueueEntry WHERE QueueID = @queueID AND DriverID = @driverID",
                new Microsoft.Data.Sqlite.SqliteParameter("@queueID", TestQueueID),
                new Microsoft.Data.Sqlite.SqliteParameter("@driverID", 2)
            );

            Assert.AreEqual("Joined queue. Position: #1", first);
            Assert.AreEqual("Joined queue. Position: #2", second);
        }

        [TestMethod]
        public void JoinQueue_ShouldAddEntryToDatabase_AfterJoining()
        {
            // join queue
            _queueService.JoinQueue(TestDriverID, TestRouteID);

            // verify database entry
            var count = _dbHelper.ExecuteScalar(
                "SELECT COUNT(*) FROM QueueEntry WHERE QueueID = @queueID AND DriverID = @driverID",
                new Microsoft.Data.Sqlite.SqliteParameter("@queueID", TestQueueID),
                new Microsoft.Data.Sqlite.SqliteParameter("@driverID", TestDriverID)
            );

            Assert.AreEqual(1L, count, "One queue entry should exist after joining");
        }

        // IsDriverInQueue() tests

        [TestMethod]
        public void IsDriverInQueue_ShouldReturnTrue_WhenDriverIsInQueue()
        {
            // join queue
            _queueService.JoinQueue(TestDriverID, TestRouteID);

            bool result = _queueService.IsDriverInQueue(TestDriverID, TestRouteID);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsDriverInQueue_ShouldReturnFalse_WhenDriverIsNotInQueue()
        {
            // empty queue
            bool result = _queueService.IsDriverInQueue(TestDriverID, TestRouteID);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsDriverInQueue_ShouldReturnFalse_AfterDriverIsRemoved()
        {
            // join queue
            _queueService.JoinQueue(TestDriverID, TestRouteID);

            // remove driver
            _queueRepo.RemoveDriverFromQueue(TestDriverID, TestQueueID);

            bool result = _queueService.IsDriverInQueue(TestDriverID, TestRouteID);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void JoinQueue_ShouldNotCreateDuplicateEntries_WhenJoinedTwice()
        {
            // duplicate join attempt
            _queueService.JoinQueue(TestDriverID, TestRouteID);
            _queueService.JoinQueue(TestDriverID, TestRouteID);

            var count = _dbHelper.ExecuteScalar(
                "SELECT COUNT(*) FROM QueueEntry WHERE QueueID = @queueID AND DriverID = @driverID",
                new Microsoft.Data.Sqlite.SqliteParameter("@queueID", TestQueueID),
                new Microsoft.Data.Sqlite.SqliteParameter("@driverID", TestDriverID)
            );

            Assert.AreEqual(1L, count, "Should only have one entry even after joining twice");
        }

        [TestMethod]
        public void JoinQueue_PositionShouldStartAtOne_WhenQueueIsEmpty()
        {
            // get initial position
            int nextPosition = _queueRepo.GetNextPosition(TestQueueID);

            Assert.AreEqual(1, nextPosition, "First position in an empty queue should be 1");
        }
    }
}